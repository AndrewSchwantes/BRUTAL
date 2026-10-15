namespace Brutal.SdlApi;

public static unsafe partial class Sdl
{
    public static unsafe partial class Gamepad
    {
        public static bool GetGamepadTouchpadFinger(
            SdlGamepad gamepad,
            int touchpad,
            int finger,
            out bool down,
            out float x,
            out float y,
            out float pressure
        )
        {
            // SDL leaves the out-pointers untouched on failure, so pre-zero them.
            byte downByte = 0;
            bool result;
            x = default;
            y = default;
            pressure = default;
            fixed (float* xPtr = &x)
            fixed (float* yPtr = &y)
            fixed (float* pressurePtr = &pressure)
            {
                result =
                    PInvoke.GetGamepadTouchpadFinger(gamepad, touchpad, finger, &downByte, xPtr, yPtr, pressurePtr)
                    != 0;
            }
            down = downByte != 0;
            return result;
        }
    }
}
