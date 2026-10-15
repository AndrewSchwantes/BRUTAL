using System.Reflection;

namespace Brutal.SdlApi;

public static unsafe partial class Sdl
{
    public static unsafe partial class PInvoke
    {
        // The native SDL3 handle, resolved lazily by the DllImportResolver below and cached so the
        // library is loaded at most once regardless of how many entry points are resolved.
        private static nint _nativeHandle;

        // Serializes resolution. The .NET resolver can be invoked concurrently from multiple threads
        // resolving different entry points; without this two threads could both observe a zero handle
        // and race to load, leaking one of the two library handles.
        private static readonly Lock ResolveLock = new();

        // Registered here so the resolver is installed before any P/Invoke in this assembly resolves.
        static PInvoke()
        {
            NativeLibrary.SetDllImportResolver(typeof(Sdl).Assembly, Resolve);
        }

        // Normalizes the "SDL3" import name to the platform-specific shared library filename
        // (SDL3.dll on Windows, libSDL3.dylib on macOS, libSDL3.so.0 on Linux). SDL's Linux build
        // installs a versioned soname (libSDL3.so.0) that the default probing conventions do not
        // try, so the candidate list is spelled out explicitly, most-specific first, with the
        // default convention name as a fallback.
        private static nint Resolve(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            // Only ever asked to resolve SDL3; anything else is left to the default resolver.
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
                    "Could not load the SDL3 native library. Attempts (most specific first): "
                        + string.Join("; ", failures)
                        + "."
                );
            }
        }

        private static string[] Candidates()
        {
            if (OperatingSystem.IsWindows())
                return ["SDL3.dll", "SDL3"];
            if (OperatingSystem.IsMacOS())
                return ["libSDL3.dylib", "SDL3"];
            // Linux and other Unix-likes.
            return ["libSDL3.so.0", "libSDL3.so", "SDL3"];
        }

        // SDL_LogMessage is variadic; it is bound fixed-arity and never passed variadic arguments.
        [LibraryImport(
            Dll,
            EntryPoint = "SDL_LogMessage",
            StringMarshalling = StringMarshalling.Custom,
            StringMarshallingCustomType = typeof(CustomStringMarshaller)
        )]
        private static partial void _LogMessage(int category, SdlLogPriority priority, string fmt);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LogMessage(int category, SdlLogPriority priority, string preformatted)
        {
            ArgumentNullException.ThrowIfNull(preformatted);
            Sdl.TraceLog.Trace();
            _LogMessage(category, priority, preformatted);
        }
    }
}
