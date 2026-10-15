using System.Reflection;
using Brutal.Logging;

namespace Brutal.SdlApi.Ttf;

/// <summary>
/// Provides access to the SDL_ttf library.
/// </summary>
public static unsafe partial class SdlTtf
{
    /// <summary>
    /// The name the generated P/Invoke surface imports against. The <see cref="PInvoke"/> static
    /// constructor registers a <see cref="NativeLibrary.SetDllImportResolver"/> that normalizes
    /// this name to the platform-specific SDL3_ttf shared library.
    /// </summary>
    internal const string Dll = "SDL3_ttf";

    /// <summary>
    /// The logging category for the per-call trace logs the generated P/Invoke surface emits. An
    /// internal implementation detail mirroring core's <c>Sdl.TraceLog</c>; the generated
    /// <c>SdlTtf.PInvoke</c> wrappers call <c>SdlTtf.TraceLog.Trace()</c> on every invocation.
    /// </summary>
    internal static readonly LogCategory TraceLog = LogCategory.Make("SdlTtf");

    /// <summary>
    /// Hosts the raw generated P/Invoke surface and the native library resolver for <c>SDL3_ttf</c>.
    /// </summary>
    public static unsafe partial class PInvoke
    {
        // The native SDL3_ttf handle, resolved lazily by the DllImportResolver below and cached
        // so the library is loaded at most once regardless of how many entry points are resolved.
        private static nint _nativeHandle;

        private static readonly Lock ResolveLock = new();

        static PInvoke()
        {
            // Load libSDL3 before SDL3_ttf, which links against it; only a real P/Invoke triggers Sdl's resolver.
            _ = Sdl.PInvoke.GetVersion();

            NativeLibrary.SetDllImportResolver(typeof(SdlTtf).Assembly, Resolve);
        }

        // Normalizes the "SDL3_ttf" import name to the platform-specific shared library filename
        // (SDL3_ttf.dll on Windows, libSDL3_ttf.dylib on macOS, libSDL3_ttf.so.0 on Linux).
        // SDL_ttf's Linux build installs a versioned soname (libSDL3_ttf.so.0), same convention as
        // core's SDL3, so the candidate list is spelled out explicitly, most-specific first, with
        // the default probing convention name as a fallback.
        private static nint Resolve(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            // Only ever asked to resolve SDL3_ttf; anything else is left to the default resolver.
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
                    "Could not load the SDL3_ttf native library. Attempts (most specific first): "
                        + string.Join("; ", failures)
                        + "."
                );
            }
        }

        private static string[] Candidates()
        {
            if (OperatingSystem.IsWindows())
                return ["SDL3_ttf.dll", "SDL3_ttf"];
            if (OperatingSystem.IsMacOS())
                return ["libSDL3_ttf.dylib", "SDL3_ttf"];
            // Linux and other Unix-likes.
            return ["libSDL3_ttf.so.0", "libSDL3_ttf.so", "SDL3_ttf"];
        }
    }
}
