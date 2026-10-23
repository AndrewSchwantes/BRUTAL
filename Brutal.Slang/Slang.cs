using System.Runtime.InteropServices.Marshalling;

namespace Brutal.SlangApi;

/// <summary>
///     The Slang class provides access to the slang library for compiling shaders.
/// </summary>
/// <remarks>
///     Release returned COM proxies with <see cref="Release"/> on the thread using
///     them, in reverse dependency order: code/diagnostics and components, modules,
///     session, then global session. Keep parents alive until their children have
///     been released. The garbage collector never releases a proxy, so one that is
///     not passed to <see cref="Release"/> keeps its native object alive.
///     Each returned proxy owns an independent reference; releasing one proxy does
///     not invalidate another proxy for the same native object.
/// </remarks>
public static partial class Slang
{
    /// <summary>
    ///     <see cref="ComWrappers"/> instance used to bridge raw COM pointers obtained from
    ///     <c>slang_*</c> entry points (e.g. <see cref="PInvoke.slang_createGlobalSession"/>)
    ///     into managed <c>[GeneratedComInterface]</c> proxies. <see cref="SlangComWrappers"/>
    ///     keeps <see cref="StrategyBasedComWrappers"/>' discovery of the proxy classes
    ///     emitted by the source generator from the type's IID, so no per-interface
    ///     registration is needed.
    /// </summary>
    internal static readonly StrategyBasedComWrappers ComWrappers = new SlangComWrappers();

    public static partial class PInvoke
    {
        static PInvoke()
        {
            var assembly = typeof(Slang).Assembly;

            var nativeDll = NativeLibrary.Load("slang", assembly, DllImportSearchPath.AssemblyDirectory);

            // Slang loads slang-glslang and slang-glsl-module by bare name at run time. Windows
            // searches only the application and system directories for them, which misses a
            // RID-less NuGet build's runtimes/win-x64/native folder, so load them from wherever
            // slang came from; Slang's own LoadLibrary then finds the loaded modules. macOS and
            // Linux resolve them through slang's rpath.
            if (OperatingSystem.IsWindows())
                foreach (var companion in (ReadOnlySpan<string>)["slang-glslang", "slang-glsl-module"])
                    NativeLibrary.TryLoad(companion, assembly, DllImportSearchPath.AssemblyDirectory, out _);

            NativeLibrary.SetDllImportResolver(
                assembly,
                (libraryName, _, _) => libraryName == "slang" ? nativeDll : IntPtr.Zero
            );
        }
    }

    /// <summary>
    ///     Throwing form of <see cref="TryCreateGlobalSession(Int, out IGlobalSession?)"/>:
    ///     returns a managed <see cref="IGlobalSession"/> COM proxy on success, throws
    ///     <see cref="SlangException"/> on failure or if the native call succeeds but yields
    ///     a null session pointer.
    /// </summary>
    public static IGlobalSession CreateGlobalSession(Int apiVersion = Constants.ApiVersion) =>
        Unwrap(TryCreateGlobalSession(apiVersion, out var session), session);

    /// <summary>
    ///     Result-returning form: invokes <c>slang_createGlobalSession</c> and wraps the raw
    ///     IUnknown pointer it produces into a managed <see cref="IGlobalSession"/>. The
    ///     <paramref name="session"/> out-parameter is null when the call fails or the
    ///     native side returns a null pointer.
    /// </summary>
    public static Result TryCreateGlobalSession(Int apiVersion, out IGlobalSession? session)
    {
        var hr = PInvoke.slang_createGlobalSession(apiVersion, out var raw);
        session = WrapComInstance<IGlobalSession>(hr, raw);
        return hr;
    }

    public static IGlobalSession CreateGlobalSession(in GlobalSessionDesc desc) =>
        Unwrap(TryCreateGlobalSession(in desc, out var session), session);

    public static Result TryCreateGlobalSession(in GlobalSessionDesc desc, out IGlobalSession? session)
    {
        var hr = PInvoke.slang_createGlobalSession2(in desc, out var raw);
        session = WrapComInstance<IGlobalSession>(hr, raw);
        return hr;
    }

    public static IGlobalSession CreateGlobalSessionWithoutCoreModule(Int apiVersion = Constants.ApiVersion) =>
        Unwrap(TryCreateGlobalSessionWithoutCoreModule(apiVersion, out var session), session);

    public static Result TryCreateGlobalSessionWithoutCoreModule(Int apiVersion, out IGlobalSession? session)
    {
        var hr = PInvoke.slang_createGlobalSessionWithoutCoreModule(apiVersion, out var raw);
        session = WrapComInstance<IGlobalSession>(hr, raw);
        return hr;
    }

    /// <summary>
    ///     Creates an <see cref="ISlangBlob"/> over a copy of <paramref name="data"/>. Slang
    ///     allocates and owns the blob's storage, so the input span doesn't need to outlive
    ///     the call. Throws if slang returns a null pointer (typically an out-of-memory
    ///     condition).
    /// </summary>
    public static unsafe ISlangBlob CreateBlob(ReadOnlySpan<byte> data)
    {
        nint raw;
        fixed (byte* p = data)
            raw = PInvoke.slang_createBlob(p, Brutal.ByteSize.Of(data));
        if (raw == 0)
            throw new SlangException((Result)(-1), "slang_createBlob returned a null pointer.");
        return WrapComInstance<ISlangBlob>((Result)0, raw)!;
    }

    /// <summary>
    ///     Throwing form of <see cref="TryDisassembleByteCode(ISlangBlob, out ISlangBlob?)"/>.
    ///     Returns the textual-disassembly blob produced by slang from
    ///     <paramref name="moduleBlob"/>. Throws on failure or if slang reports success with
    ///     a null pointer.
    /// </summary>
    public static ISlangBlob DisassembleByteCode(ISlangBlob moduleBlob) =>
        Unwrap(TryDisassembleByteCode(moduleBlob, out var disassembly), disassembly);

    /// <summary>
    ///     Result-returning form: converts <paramref name="moduleBlob"/> to a raw COM pointer
    ///     for the duration of the flat-C <c>slang_disassembleByteCode</c> call (releasing
    ///     the extra reference afterwards) and wraps the returned pointer back into a managed
    ///     <see cref="ISlangBlob"/>. The <paramref name="disassembly"/> out-parameter is null
    ///     when the call fails or slang returns a null pointer.
    /// </summary>
    public static Result TryDisassembleByteCode(ISlangBlob moduleBlob, out ISlangBlob? disassembly)
    {
        var rawIn = GetComInstance(moduleBlob);
        try
        {
            var hr = PInvoke.slang_disassembleByteCode(rawIn, out var rawOut);
            disassembly = WrapComInstance<ISlangBlob>(hr, rawOut);
            return hr;
        }
        finally
        {
            Marshal.Release(rawIn);
        }
    }

    /// <summary>
    ///     Throws if the result is a failure, or if a successful result was paired with a
    ///     null COM proxy (which indicates the native side handed back a null pointer
    ///     despite reporting success).
    /// </summary>
    private static T Unwrap<T>(Result hr, T? session)
        where T : class
    {
        hr.ThrowIfFailure();
        return session
            ?? throw new SlangException(hr, $"slang returned a success Result with a null {typeof(T).Name}.");
    }

    /// <summary>
    ///     Wraps a raw COM <c>IUnknown</c> pointer into a managed proxy of the requested
    ///     interface type. Returns <c>null</c> when the underlying call failed or the
    ///     native side handed back a null pointer.
    /// </summary>
    internal static T? WrapComInstance<T>(Result hr, nint comInstance)
        where T : class
    {
        if (comInstance == 0)
            return null;
        try
        {
            return hr.IsFailure ? null : WrapBorrowedComInstance<T>(comInstance);
        }
        finally
        {
            // The RCW retains its own reference. Consume the reference transferred by
            // the native export, including when interface conversion throws.
            Marshal.Release(comInstance);
        }
    }

    internal static T? WrapBorrowedComInstance<T>(nint pointer)
        where T : class
    {
        if (pointer == 0)
            return null;
        var wrapper = ComWrappers.GetOrCreateObjectForComInstance(pointer, CreateObjectFlags.UniqueInstance);
        // Only Release frees a proxy's native references. Its finalizer would release them
        // on the finalizer thread, in no order relative to the proxy's session.
        GC.SuppressFinalize(wrapper);
        try
        {
            return (T)wrapper;
        }
        catch
        {
            ((ComObject)wrapper).FinalRelease();
            throw;
        }
    }

    /// <summary>
    ///     Releases a proxy returned by this binding immediately. This is the only way a
    ///     proxy's native reference is released: the garbage collector never does it.
    ///     Null and repeated releases are harmless. Do not use the proxy afterwards. Release children
    ///     before their owning session and release sessions before their global session.
    ///     Do not race release against another call using the same proxy.
    /// </summary>
    public static void Release(object? instance)
    {
        if (instance is null)
            return;
        if (instance is not ComObject wrapper)
            throw new ArgumentException("Expected a Slang COM proxy.", nameof(instance));
        wrapper.FinalRelease();
    }

    /// <summary>
    ///     Returns the underlying COM <c>IUnknown</c> pointer for a managed proxy. Used by
    ///     callers passing managed COM objects into deprecated <c>sp*</c> PInvokes which
    ///     take raw <c>nint</c> pointers. The returned pointer carries an extra reference
    ///     count. Call <see cref="Marshal.Release"/> on it when finished, or pass it to
    ///     APIs that take ownership.
    /// </summary>
    public static nint GetComInstance(object comObject)
    {
        ArgumentNullException.ThrowIfNull(comObject);
        if (comObject is ComObject)
            return GetComInstance<ISlangUnknown>((ISlangUnknown)comObject);
        return System.Runtime.InteropServices.ComWrappers.TryGetComInstance(comObject, out var native)
            ? native
            : ComWrappers.GetOrCreateComInterfaceForObject(comObject, CreateComInterfaceFlags.None);
    }

    /// <summary>
    ///     Gets an owned pointer to the requested native interface, including its correct
    ///     subobject address. Release the returned reference after the native call.
    /// </summary>
    public static unsafe nint GetComInstance<T>(T comObject)
        where T : class
    {
        if (comObject is ComObject wrapper)
        {
            var table = ((IUnmanagedVirtualMethodTableProvider)wrapper).GetVirtualMethodTableInfoForKey(typeof(T));
            var pointer = (nint)table.ThisPointer;
            Marshal.AddRef(pointer);
            GC.KeepAlive(wrapper);
            return pointer;
        }
        return (nint)ComInterfaceMarshaller<T>.ConvertToUnmanaged(comObject);
    }
}
