using System.Reflection;
using Brutal.Logging;

namespace Brutal.SdlApi.Mixer;

/// <summary>
/// Provides access to the SDL_mixer library.
/// </summary>
public static unsafe partial class SdlMixer
{
    /// <summary>
    /// The name the generated P/Invoke surface imports against. The <see cref="PInvoke"/> static
    /// constructor registers a <see cref="NativeLibrary.SetDllImportResolver"/> that normalizes
    /// this name to the platform-specific SDL3_mixer shared library.
    /// </summary>
    internal const string Dll = "SDL3_mixer";

    /// <summary>
    /// The logging category for the per-call trace logs the generated P/Invoke surface emits. An
    /// internal implementation detail mirroring core's <c>Sdl.TraceLog</c>; the generated
    /// <c>SdlMixer.PInvoke</c> wrappers call <c>SdlMixer.TraceLog.Trace()</c> on every invocation.
    /// </summary>
    internal static readonly LogCategory TraceLog = LogCategory.Make("SdlMixer");

    /// <summary>
    /// Captures exceptions thrown inside SDL_mixer callbacks, which run on the audio thread, so they
    /// can be rethrown from managed code. Drained by every ergonomic mixer/track/group method and by
    /// <see cref="ThrowPendingCallbackExceptions"/>.
    /// </summary>
    internal static readonly ExceptionAggregator ExceptionAggregator = new();

    /// <summary>
    /// Rethrows any exception captured inside an SDL_mixer callback since the last drain, then clears
    /// the pending set. Callbacks fire on SDL_mixer's audio thread(s), where a managed exception cannot
    /// legally unwind; each is caught, logged, and parked on the shared aggregator instead. Every
    /// ergonomic method that touches a mixer, track or group drains automatically, but during long
    /// device-driven playback that makes no such calls, invoke this periodically (e.g. once per frame)
    /// to surface a failing callback promptly.
    /// </summary>
    /// <exception cref="SdlCallbackException">Exactly one callback exception was pending.</exception>
    /// <exception cref="AggregateException">More than one callback exception was pending, or some were dropped.</exception>
    public static void ThrowPendingCallbackExceptions() => ExceptionAggregator.CheckExceptions();

    /// <summary>
    /// Hosts the raw generated P/Invoke surface and the native library resolver for <c>SDL3_mixer</c>.
    /// </summary>
    public static unsafe partial class PInvoke
    {
        // The native SDL3_mixer handle, resolved lazily by the DllImportResolver below and cached
        // so the library is loaded at most once regardless of how many entry points are resolved.
        private static nint _nativeHandle;

        private static readonly Lock ResolveLock = new();

        static PInvoke()
        {
            // Load libSDL3 before SDL3_mixer, which links against it; only a real P/Invoke triggers Sdl's resolver.
            _ = Sdl.PInvoke.GetVersion();

            NativeLibrary.SetDllImportResolver(typeof(SdlMixer).Assembly, Resolve);
        }

        // Normalizes the "SDL3_mixer" import name to the platform-specific shared library filename
        // (SDL3_mixer.dll on Windows, libSDL3_mixer.dylib on macOS, libSDL3_mixer.so.0 on Linux).
        // SDL_mixer's Linux build installs a versioned soname (libSDL3_mixer.so.0), same convention
        // as core's SDL3, so the candidate list is spelled out explicitly, most-specific first, with
        // the default probing convention name as a fallback.
        private static nint Resolve(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            // Only ever asked to resolve SDL3_mixer; anything else is left to the default resolver.
            if (libraryName != Dll)
                return IntPtr.Zero;

            lock (ResolveLock)
            {
                if (_nativeHandle != IntPtr.Zero)
                    return _nativeHandle;

                var search = searchPath ?? DllImportSearchPath.AssemblyDirectory;

                // Load (not TryLoad) so each candidate's OS error text reaches the final exception.
                var failures = new List<string>();
                foreach (var candidate in Candidates())
                {
                    try
                    {
                        _nativeHandle = NativeLibrary.Load(candidate, assembly, search);
                        return _nativeHandle;
                    }
                    catch (Exception ex)
                    {
                        failures.Add($"'{candidate}': {ex.Message}");
                    }
                }

                throw new DllNotFoundException(
                    "Could not load the SDL3_mixer native library. Attempts (most specific first): "
                        + string.Join("; ", failures)
                        + "."
                );
            }
        }

        private static string[] Candidates()
        {
            if (OperatingSystem.IsWindows())
                return ["SDL3_mixer.dll", "SDL3_mixer"];
            if (OperatingSystem.IsMacOS())
                return ["libSDL3_mixer.dylib", "SDL3_mixer"];
            // Linux and other Unix-likes.
            return ["libSDL3_mixer.so.0", "libSDL3_mixer.so", "SDL3_mixer"];
        }
    }
}
