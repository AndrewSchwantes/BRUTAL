using System.Text;

namespace Brutal.SdlApi;

public static unsafe partial class Sdl
{
    public static unsafe partial class Keyboard
    {
        // SDL keeps the name pointer without copying it, so each name lives in unmanaged memory until replaced.
        private static readonly Dictionary<SdlScancode, nint> ScancodeNames = new();
        private static readonly Lock ScancodeNameLock = new();

        [LibraryImport(Dll, EntryPoint = "SDL_SetScancodeName")]
        private static partial byte _SetScancodeName(SdlScancode scancode, byte* name);

        public static void SetScancodeName(SdlScancode scancode, string name)
        {
            ArgumentNullException.ThrowIfNull(name);

            // Copy `name` into unmanaged memory we own: a NUL-terminated UTF-8 buffer.
            var byteCount = Encoding.UTF8.GetByteCount(name);
            var buffer = (byte*)NativeMemory.Alloc((nuint)(byteCount + 1));
            var span = new Span<byte>(buffer, byteCount + 1);
            Encoding.UTF8.GetBytes(name, span);
            span[byteCount] = 0;

            lock (ScancodeNameLock)
            {
                if (_SetScancodeName(scancode, buffer) == 0)
                {
                    // SDL did not take the pointer; free our copy and surface the error.
                    NativeMemory.Free(buffer);
                    throw SdlException.FromError("SDL_SetScancodeName failed");
                }

                // SDL no longer references the previous copy for this scancode.
                if (ScancodeNames.TryGetValue(scancode, out var previous))
                    NativeMemory.Free((void*)previous);
                ScancodeNames[scancode] = (nint)buffer;
            }
        }
    }
}
