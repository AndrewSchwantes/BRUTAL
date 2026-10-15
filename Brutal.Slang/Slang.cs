using System.Runtime.InteropServices.Marshalling;

namespace Brutal.SlangApi;

/// <summary>
///     The Slang class provides access to the slang library for compiling shaders.
/// </summary>
/// <remarks>
///     Slang COM proxies (returned by <see cref="CreateGlobalSession(Int)"/>,
///     <see cref="ISessionExtensions.LoadModule"/>, etc.) are released by the ComWrappers
///     finalizer when GC reclaims the managed proxy. There is no deterministic
///     <c>Dispose</c>. Callers don't need to do anything special. COM proxies in this
///     binding represent shared-ownership references; the underlying slang object stays
///     alive as long as anything (slang internals, other proxies, or the global session)
///     holds a reference, regardless of when any single proxy is collected.
/// </remarks>
public static partial class Slang
{
    /// <summary>
    ///     <see cref="ComWrappers"/> instance used to bridge raw COM pointers obtained from
    ///     <c>slang_*</c> entry points (e.g. <see cref="PInvoke.slang_createGlobalSession"/>)
    ///     into managed <c>[GeneratedComInterface]</c> proxies. The default
    ///     <see cref="StrategyBasedComWrappers"/> implementation discovers proxy classes
    ///     emitted by the source generator from the type's IID, so no per-interface
    ///     registration is needed.
    /// </summary>
    internal static readonly StrategyBasedComWrappers ComWrappers = new();

    public static partial class PInvoke
    {
        static PInvoke()
        {
            var assembly = typeof(Slang).Assembly;

            var nativeDll = NativeLibrary.Load("slang", assembly, DllImportSearchPath.AssemblyDirectory);

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
            raw = PInvoke.slang_createBlob(p, (Brutal.ByteSize64)data.Length);
        if (raw == 0)
            throw new SlangException((Result)(-1), "slang_createBlob returned a null pointer.");
        return (ISlangBlob)ComWrappers.GetOrCreateObjectForComInstance(raw, CreateObjectFlags.None);
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
        if (hr.IsFailure || comInstance == 0)
            return null;
        return (T)ComWrappers.GetOrCreateObjectForComInstance(comInstance, CreateObjectFlags.None);
    }

    /// <summary>
    ///     Returns the underlying COM <c>IUnknown</c> pointer for a managed proxy. Used by
    ///     callers passing managed COM objects into deprecated <c>sp*</c> PInvokes which
    ///     take raw <c>nint</c> pointers. The returned pointer carries an extra reference
    ///     count. Call <see cref="Marshal.Release"/> on it when finished, or pass it to
    ///     APIs that take ownership.
    /// </summary>
    public static nint GetComInstance(object comObject) =>
        ComWrappers.GetOrCreateComInterfaceForObject(comObject, CreateComInterfaceFlags.None);
}
