using System.Reflection;
using Brutal.Logging;

namespace Brutal.SdlApi.Image;

/// <summary>
/// Provides access to the SDL_image library.
/// </summary>
public static unsafe partial class SdlImage
{
    /// <summary>
    /// The name the generated P/Invoke surface imports against. The <see cref="PInvoke"/> static
    /// constructor registers a <see cref="NativeLibrary.SetDllImportResolver"/> that normalizes
    /// this name to the platform-specific SDL3_image shared library.
    /// </summary>
    internal const string Dll = "SDL3_image";

    /// <summary>
    /// The logging category for the per-call trace logs the generated P/Invoke surface emits. An
    /// internal implementation detail mirroring core's <c>Sdl.TraceLog</c>; the generated
    /// <c>SdlImage.PInvoke</c> wrappers call <c>SdlImage.TraceLog.Trace()</c> on every invocation.
    /// </summary>
    internal static readonly LogCategory TraceLog = LogCategory.Make("SdlImage");

    /// <summary>
    /// Hosts the raw generated P/Invoke surface and the native library resolver for <c>SDL3_image</c>.
    /// </summary>
    public static unsafe partial class PInvoke
    {
        // The native SDL3_image handle, resolved lazily by the DllImportResolver below and cached
        // so the library is loaded at most once regardless of how many entry points are resolved.
        private static nint _nativeHandle;

        private static readonly Lock ResolveLock = new();

        static PInvoke()
        {
            // Load libSDL3 before SDL3_image, which links against it; only a real P/Invoke triggers Sdl's resolver.
            _ = Sdl.PInvoke.GetVersion();

            NativeLibrary.SetDllImportResolver(typeof(SdlImage).Assembly, Resolve);
        }

        // Normalizes the "SDL3_image" import name to the platform-specific shared library filename
        // (SDL3_image.dll on Windows, libSDL3_image.dylib on macOS, libSDL3_image.so.0 on Linux).
        // SDL_image's Linux build installs a versioned soname (libSDL3_image.so.0), same convention
        // as core's SDL3, so the candidate list is spelled out explicitly, most-specific first, with
        // the default probing convention name as a fallback.
        private static nint Resolve(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            // Only ever asked to resolve SDL3_image; anything else is left to the default resolver.
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
                    "Could not load the SDL3_image native library. Attempts (most specific first): "
                        + string.Join("; ", failures)
                        + "."
                );
            }
        }

        private static string[] Candidates()
        {
            if (OperatingSystem.IsWindows())
                return ["SDL3_image.dll", "SDL3_image"];
            if (OperatingSystem.IsMacOS())
                return ["libSDL3_image.dylib", "SDL3_image"];
            // Linux and other Unix-likes.
            return ["libSDL3_image.so.0", "libSDL3_image.so", "SDL3_image"];
        }
    }
}
