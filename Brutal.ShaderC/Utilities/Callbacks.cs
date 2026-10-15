using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Brutal.Memory;
using Brutal.Pointers.Extensions;

namespace Brutal.ShaderCApi;

public static partial class ShaderC
{
    /// <summary>
    /// Default callbacks for the shader compiler.
    /// </summary>
    public static class Callbacks
    {
        // Guards against #include cycles by bounding nesting depth (shaderc increments depth per
        // nesting level). 200 is far above any realistic include tree, so a legitimate compile never
        // reaches it; a self- or mutually-recursive #include climbs to the limit and is reported as a
        // cycle instead of looping until the stack overflows.
        private const nuint MaxIncludeDepth = 200;

        /// <summary>
        ///     Resolves an <c>#include</c> request by reading the requested file from disk relative to the
        ///     requesting source's directory.
        /// </summary>
        /// <param name="data">Opaque user-data pointer passed through from the options (unused by this implementation).</param>
        /// <param name="source">NUL-terminated UTF-8 C string naming the file being requested (the include path).</param>
        /// <param name="type">The kind of inclusion request (<see cref="IncludeType.Relative"/> or <see cref="IncludeType.Standard"/>).</param>
        /// <param name="requestingSource">NUL-terminated UTF-8 C string naming the source file that contains the <c>#include</c> directive.</param>
        /// <param name="depth">Include nesting depth; 1 indicates a top-level include from the root source.</param>
        /// <returns>
        ///     A pointer to a heap-allocated <see cref="IncludeResult"/> whose <c>Content</c> and
        ///     <c>SourceName</c> buffers are also heap-allocated. The caller (shaderc) must invoke
        ///     <see cref="Release"/> on the result when it is no longer needed.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         This method and <see cref="Release"/> form a matching allocator/deallocator pair: all
        ///         allocations are performed with <c>Mem.Alloc</c> and must be freed by <see cref="Release"/>
        ///         using <c>Mem.Free</c>. A custom resolver that replaces this implementation must honor the
        ///         same contract.
        ///     </para>
        ///     <para>
        ///         The returned <see cref="IncludeResult"/> and its buffers must remain valid until
        ///         <see cref="Release"/> is called on it. Do not free or modify the result after returning it.
        ///     </para>
        ///     <para>
        ///         Any exception thrown during resolution (e.g., <see cref="FileNotFoundException"/>) is
        ///         caught and surfaced as a shaderc include-failure result whose <c>Content</c> field
        ///         contains the exception message. The exception does <strong>not</strong> propagate as a
        ///         managed exception; include-resolution failures appear only in the compiler's error output.
        ///     </para>
        /// </remarks>
        [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
        public static unsafe IncludeResult* Resolve(
            void* data,
            byte* source,
            IncludeType type,
            byte* requestingSource,
            nuint depth
        )
        {
            // Allocate a new result
            var result = Mem.AllocZeroed<IncludeResult>();

            try
            {
                ResolveInternal(
                    ref result.Value,
                    data,
                    Utf8.GetNullTerminatedSpan(source),
                    type,
                    Utf8.GetNullTerminatedSpan(requestingSource),
                    depth
                );
            }
            catch (Exception ex)
            {
                // Free any previously allocated memory in Content (we are going to replace it)
                if (result.Value.Content != null)
                {
                    Mem.Free(result.Value.Content);
                    result.Value.Content = null;
                    result.Value.ContentLength = ByteSize.Zero;
                }

                // Free and reset SourceName (API says it should be empty on failure)
                if (result.Value.SourceName != null)
                {
                    Mem.Free(result.Value.SourceName);
                    result.Value.SourceName = null;
                    result.Value.SourceNameLength = ByteSize.Zero;
                }

                // Write the error message to Content
                var byteCount = Utf8.GetByteCount(ex.Message);
                var span = Mem.Alloc<byte>(byteCount);
                Utf8.GetBytes(ex.Message, span);
                result.Value.Content = span.AsPointer();
                result.Value.ContentLength = ByteSize.Of(byteCount);
            }

            return result;
        }

        private static unsafe void ResolveInternal(
            ref IncludeResult result,
            void* data,
            ReadOnlySpan<byte> source,
            IncludeType type,
            ReadOnlySpan<byte> requestingSource,
            nuint depth
        )
        {
            if (depth > MaxIncludeDepth)
                throw new InvalidOperationException(
                    $"Include depth {depth} exceeded the maximum of {MaxIncludeDepth}; possible #include cycle."
                );

            // Get the path to the include file, relative to the requesting source.
            // Decode the path once and reuse it for the error message and file open.
            var path = Utf8.Path.Combine(Utf8.Path.GetDirectoryName(requestingSource), source);
            var pathStr = Utf8.GetString(path);

            // Open directly instead of File.Exists + open, which would stat the file twice on every
            // include. A missing file surfaces as the framework's own FileNotFoundException /
            // DirectoryNotFoundException, which is caught by Resolve and reported as an include failure.
            using (var handle = File.OpenHandle(pathStr))
            {
                // Allocate null-terminated storage for SourceName.
                // shaderc treats source_name as a C string and re-feeds it as requesting_source,
                // so the buffer must be null-terminated even though SourceNameLength stays path.Length.
                var copy = Mem.Alloc<byte>(path.Length + 1);
                path.CopyTo(copy);
                copy[path.Length] = 0;
                result.SourceName = copy.AsPointer();
                result.SourceNameLength = ByteSize.Of(path.Length);

                // Read the include file into memory using RandomAccess to avoid the FileStream buffer.
                var length = RandomAccess.GetLength(handle);
                if (length > int.MaxValue)
                    throw new IOException($"Include file is too large to read ({length} bytes): {pathStr}");

                var dataSize = (int)length;
                var memory = Mem.Alloc<byte>(dataSize);
                result.Content = memory.AsPointer();
                result.ContentLength = ByteSize.Of(dataSize);

                // RandomAccess.Read is permitted to return a short count, so loop until the buffer fills.
                Span<byte> destination = memory;
                var totalRead = 0;
                while (totalRead < dataSize)
                {
                    var read = RandomAccess.Read(handle, destination[totalRead..], totalRead);
                    if (read == 0)
                        break;
                    totalRead += read;
                }

                if (totalRead != dataSize)
                    throw new IOException($"Failed to read expected {dataSize} bytes, read {totalRead} bytes instead.");
            }
        }

        /// <summary>
        ///     Frees an <see cref="IncludeResult"/> previously returned by <see cref="Resolve"/>.
        /// </summary>
        /// <param name="data">Opaque user-data pointer passed through from the options (unused by this implementation).</param>
        /// <param name="result">Pointer to the <see cref="IncludeResult"/> to release, or <c>null</c> (no-op).</param>
        /// <remarks>
        ///     <para>
        ///         This method is the matching deallocator for <see cref="Resolve"/>: it frees the
        ///         <c>SourceName</c> buffer, the <c>Content</c> buffer, and the <see cref="IncludeResult"/>
        ///         struct itself, all previously allocated via <c>Mem.Alloc</c>.
        ///     </para>
        ///     <para>
        ///         A custom resolver that replaces <see cref="Resolve"/> must provide a matching release
        ///         callback that frees whatever its resolver allocated. Do not mix allocators (e.g., do not
        ///         allocate in one callback and free in the other).
        ///     </para>
        /// </remarks>
        [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
        public static unsafe void Release(void* data, IncludeResult* result)
        {
            if (result != null)
            {
                // UserData ownership belongs to whoever allocates it; do not free it here.
                Mem.Free(result->SourceName);
                Mem.Free(result->Content);
                Mem.Free(result);
            }
        }
    }
}
