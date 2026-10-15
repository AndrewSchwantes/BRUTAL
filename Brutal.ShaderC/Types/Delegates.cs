namespace Brutal.ShaderCApi;

/// <summary>
///     A function-pointer wrapper for the shaderc include-resolve callback.
/// </summary>
/// <remarks>
///     <para>
///         Wraps the native <c>shaderc_include_resolve_fn</c> callback, whose C signature is:
///     </para>
///     <code>
///         typedef shaderc_include_result* (*shaderc_include_resolve_fn)(
///             void* user_data,
///             const char* requested_source,
///             int type,
///             const char* requesting_source,
///             size_t include_depth);
///     </code>
///     <para>The parameters, in order, are:</para>
///     <list type="bullet">
///         <item><description><c>user_data</c>: opaque client context pointer passed through from the options.</description></item>
///         <item><description><c>requested_source</c>: NUL-terminated UTF-8 C string; the path being included.</description></item>
///         <item><description><c>type</c>: an <see cref="IncludeType"/> value indicating relative vs. standard inclusion.</description></item>
///         <item><description><c>requesting_source</c>: NUL-terminated UTF-8 C string; the source file containing the <c>#include</c>.</description></item>
///         <item><description><c>include_depth</c>: nesting depth (1 = top-level include).</description></item>
///     </list>
///     <para>
///         <strong>Ownership.</strong> The callee allocates the returned <see cref="IncludeResult"/> and its
///         buffers; shaderc calls the matching <see cref="IncludeResultReleaseFn"/> when it is done with the
///         result. The result and its contents must remain valid until that release call.
///     </para>
///     <para>
///         The full consumer-visible contract is documented on
///         <see cref="ShaderC.Callbacks.Resolve"/> and <see cref="CompileOptions.SetDefaultIncludeCallbacks"/>.
///     </para>
/// </remarks>
public readonly unsafe struct IncludeResolveFn(
    delegate* unmanaged[Cdecl]<void*, byte*, IncludeType, byte*, nuint, IncludeResult*> handle
)
{
    /// <summary>The underlying unmanaged function pointer.</summary>
    public readonly delegate* unmanaged[Cdecl]<void*, byte*, IncludeType, byte*, nuint, IncludeResult*> Invoke = handle;
}

/// <summary>
///     A function-pointer wrapper for the shaderc include-result release callback.
/// </summary>
/// <remarks>
///     <para>
///         Wraps the native <c>shaderc_include_result_release_fn</c> callback, whose C signature is:
///     </para>
///     <code>
///         typedef void (*shaderc_include_result_release_fn)(
///             void* user_data,
///             shaderc_include_result* include_result);
///     </code>
///     <para>
///         This callback is the matching deallocator for <see cref="IncludeResolveFn"/>: it must free the
///         <see cref="IncludeResult"/> and all buffers allocated by the resolver. Use the same
///         allocator/deallocator pair; do not mix allocators between the resolve and release callbacks.
///     </para>
///     <para>
///         The full consumer-visible contract is documented on
///         <see cref="ShaderC.Callbacks.Release"/> and <see cref="CompileOptions.SetDefaultIncludeCallbacks"/>.
///     </para>
/// </remarks>
public readonly unsafe struct IncludeResultReleaseFn(delegate* unmanaged[Cdecl]<void*, IncludeResult*, void> handle)
{
    /// <summary>The underlying unmanaged function pointer.</summary>
    public readonly delegate* unmanaged[Cdecl]<void*, IncludeResult*, void> Invoke = handle;
}
