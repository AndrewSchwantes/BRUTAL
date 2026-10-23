using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Brutal.SlangApi;

/// <summary>
///     Hand-written ergonomic overloads on top of the generated <c>ISession</c> COM
///     interface. The generated half (<c>InterfacesExt.gen.cs</c>) mirrors the native
///     vtable shape one-for-one; this partial adds the wrappers needed to keep callers
///     from having to deal with raw <c>nint</c>/<c>nint*</c> at points where the COM
///     ABI requires arrays of unmanaged COM pointers that managed marshalling cannot
///     represent on its own.
/// </summary>
public static unsafe partial class ISessionExtensions
{
    /// <summary>
    ///     Creates a composite <see cref="IComponentType"/> from the given component types,
    ///     hiding the <c>nint*</c> array marshalling that the underlying COM ABI requires.
    ///     Each input proxy is AddRef'd for the duration of the call and released afterwards;
    ///     slang retains its own references into the returned composite, so the inputs
    ///     remain usable after this call returns.
    /// </summary>
    /// <param name="self">The session creating the composite.</param>
    /// <param name="componentTypes">
    ///     Components to compose. Accepts arrays, <c>stackalloc</c> spans, collection
    ///     literals, or <see cref="System.Runtime.InteropServices.CollectionsMarshal.AsSpan{T}"/>.
    /// </param>
    /// <param name="outDiagnostics">
    ///     Diagnostic blob populated by slang. May be non-null even on success.
    /// </param>
    /// <returns>The composite component on success.</returns>
    /// <exception cref="SlangException">Thrown if slang reports a failing <see cref="Result"/>.</exception>
    public static IComponentType CreateCompositeComponentType(
        this ISession self,
        ReadOnlySpan<IComponentType> componentTypes,
        out ISlangBlob outDiagnostics
    )
    {
        var count = componentTypes.Length;
        Span<nint> pointers = count <= 64 ? stackalloc nint[count] : new nint[count];
        pointers.Clear();

        try
        {
            for (var i = 0; i < count; i++)
                pointers[i] = Slang.GetComInstance(componentTypes[i]);

            fixed (nint* p = pointers)
            {
                self.CreateCompositeComponentType(p, count, out var composite, out outDiagnostics).ThrowIfFailure();
                return composite;
            }
        }
        finally
        {
            for (var i = 0; i < count; i++)
                if (pointers[i] != 0)
                    Marshal.Release(pointers[i]);
        }
    }

    /// <summary>
    ///     Loads a previously-imported module by name. UTF-8 marshalling for
    ///     <paramref name="moduleName"/> is handled here so callers don't have to pin a
    ///     <c>byte*</c> by hand.
    /// </summary>
    public static IModule? LoadModule(this ISession self, string moduleName, out ISlangBlob? outDiagnostics)
    {
        Span<byte> nameBuf = stackalloc byte[ShortStringStackBytes];
        EncodeUtf8(moduleName, ref nameBuf);
        fixed (byte* nameP = nameBuf)
            return self.LoadModule(nameP, out outDiagnostics);
    }

    /// <summary>
    ///     Loads a module from an in-memory source blob, marshalling
    ///     <paramref name="moduleName"/> and <paramref name="path"/> from UTF-16 to UTF-8.
    /// </summary>
    public static IModule? LoadModuleFromSource(
        this ISession self,
        string moduleName,
        string path,
        ISlangBlob source,
        out ISlangBlob? outDiagnostics
    )
    {
        Span<byte> nameBuf = stackalloc byte[ShortStringStackBytes];
        Span<byte> pathBuf = stackalloc byte[ShortStringStackBytes];
        EncodeUtf8(moduleName, ref nameBuf);
        EncodeUtf8(path, ref pathBuf);
        fixed (byte* nameP = nameBuf)
        fixed (byte* pathP = pathBuf)
            return self.LoadModuleFromSource(nameP, pathP, source, out outDiagnostics);
    }

    /// <summary>
    ///     Loads a module from a UTF-16 string source, marshalling all three string
    ///     parameters to null-terminated UTF-8 for the underlying COM call.
    /// </summary>
    public static IModule? LoadModuleFromSourceString(
        this ISession self,
        string moduleName,
        string path,
        string source,
        out ISlangBlob? outDiagnostics
    )
    {
        Span<byte> nameBuf = stackalloc byte[ShortStringStackBytes];
        Span<byte> pathBuf = stackalloc byte[ShortStringStackBytes];
        Span<byte> sourceBuf = stackalloc byte[LongStringStackBytes];
        EncodeUtf8(moduleName, ref nameBuf);
        EncodeUtf8(path, ref pathBuf);
        EncodeUtf8(source, ref sourceBuf);
        fixed (byte* nameP = nameBuf)
        fixed (byte* pathP = pathBuf)
        fixed (byte* sourceP = sourceBuf)
            return self.LoadModuleFromSourceString(nameP, pathP, sourceP, out outDiagnostics);
    }

    /// <summary>
    ///     Loads a module from a precompiled IR blob, marshalling
    ///     <paramref name="moduleName"/> and <paramref name="path"/> to UTF-8.
    /// </summary>
    public static IModule? LoadModuleFromIRBlob(
        this ISession self,
        string moduleName,
        string path,
        ISlangBlob source,
        out ISlangBlob? outDiagnostics
    )
    {
        Span<byte> nameBuf = stackalloc byte[ShortStringStackBytes];
        Span<byte> pathBuf = stackalloc byte[ShortStringStackBytes];
        EncodeUtf8(moduleName, ref nameBuf);
        EncodeUtf8(path, ref pathBuf);
        fixed (byte* nameP = nameBuf)
        fixed (byte* pathP = pathBuf)
            return self.LoadModuleFromIRBlob(nameP, pathP, source, out outDiagnostics);
    }

    /// <summary>
    ///     Writes the RTTI bytes Slang uses to identify <paramref name="type"/>'s conformance to
    ///     <paramref name="interfaceType"/> into <paramref name="buffer"/>, which must hold at
    ///     least 16 bytes.
    /// </summary>
    /// <exception cref="SlangException">Thrown if slang reports a failing <see cref="Result"/>.</exception>
    public static void GetDynamicObjectRTTIBytes(
        this ISession self,
        TypeReflection type,
        TypeReflection interfaceType,
        Span<byte> buffer
    )
    {
        fixed (byte* p = buffer)
            self.GetDynamicObjectRTTIBytes(type, interfaceType, (uint*)p, checked((uint)buffer.Length))
                .ThrowIfFailure();
    }

    private const int ShortStringStackBytes = 256;
    private const int LongStringStackBytes = 8192;

    /// <summary>
    ///     Encodes <paramref name="value"/> as null-terminated UTF-8 into <paramref name="buffer"/>,
    ///     re-pointing the span at a heap-allocated array if the stackalloc'd buffer is too small.
    ///     The caller pins the resulting span via <c>fixed</c> for a single COM call.
    /// </summary>
    private static void EncodeUtf8(string value, ref Span<byte> buffer)
    {
        if (string.IsNullOrEmpty(value))
        {
            Debug.Assert(buffer.Length >= 1);
            buffer[0] = 0;
            return;
        }

        var byteCount = Encoding.UTF8.GetByteCount(value);
        var needed = byteCount + 1;
        if (needed > buffer.Length)
            buffer = new byte[needed];
        else
            buffer = buffer[..needed];
        Encoding.UTF8.GetBytes(value, buffer);
        buffer[byteCount] = 0;
    }
}
