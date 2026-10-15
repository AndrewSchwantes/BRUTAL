namespace Brutal.SdlApi;

public static unsafe partial class Sdl
{
    public static unsafe partial class PInvoke
    {
        // Hand-bound: SDL_vulkan.h is outside the generator's input. Vulkan handles are passed as nint.

        [LibraryImport(Dll, EntryPoint = "SDL_Vulkan_GetInstanceExtensions")]
        private static partial byte** _Vulkan_GetInstanceExtensions(uint* count);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte** Vulkan_GetInstanceExtensions(uint* count)
        {
            Sdl.TraceLog.Trace();
            return _Vulkan_GetInstanceExtensions(count);
        }

        [LibraryImport(Dll, EntryPoint = "SDL_Vulkan_CreateSurface")]
        private static partial byte _Vulkan_CreateSurface(
            SdlWindow window,
            nint instance,
            nint allocator,
            nint* surface
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte Vulkan_CreateSurface(SdlWindow window, nint instance, nint allocator, nint* surface)
        {
            Sdl.TraceLog.Trace();
            return _Vulkan_CreateSurface(window, instance, allocator, surface);
        }

        [LibraryImport(Dll, EntryPoint = "SDL_Vulkan_DestroySurface")]
        private static partial void _Vulkan_DestroySurface(nint instance, nint surface, nint allocator);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Vulkan_DestroySurface(nint instance, nint surface, nint allocator)
        {
            Sdl.TraceLog.Trace();
            _Vulkan_DestroySurface(instance, surface, allocator);
        }
    }

    public static unsafe class Vulkan
    {
        public static string[] GetInstanceExtensions()
        {
            uint count;
            var ptr = PInvoke.Vulkan_GetInstanceExtensions(&count);
            if (ptr is null)
                throw SdlException.FromError("SDL_Vulkan_GetInstanceExtensions failed");

            // Owned by SDL - must not be freed here (see SDL_vulkan.h's doc comment).
            var result = new string[count];
            for (var i = 0; i < count; i++)
                result[i] = Marshal.PtrToStringUTF8((nint)ptr[i])!;
            return result;
        }

        public static nint CreateSurface(SdlWindow window, nint instance)
        {
            nint surface;
            if (PInvoke.Vulkan_CreateSurface(window, instance, 0, &surface) == 0)
                throw SdlException.FromError("SDL_Vulkan_CreateSurface failed");
            return surface;
        }

        public static void DestroySurface(nint instance, nint surface) =>
            PInvoke.Vulkan_DestroySurface(instance, surface, 0);
    }
}
