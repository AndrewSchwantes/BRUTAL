namespace Brutal.OpenAlApi;

public sealed partial class CaptureDevice
{
    // The four standard formats are the only values AlFormat defines; anything else is an
    // extension or invalid format with no fixed frame size.
    internal static int? FrameSize(AlFormat format) =>
        format switch
        {
            AlFormat.Mono8 => 1,
            AlFormat.Mono16 => 2,
            AlFormat.Stereo8 => 2,
            AlFormat.Stereo16 => 4,
            _ => null,
        };

    // Multiply as long so the bound cannot overflow.
    internal static void CheckBufferLength(int bytesPerFrame, int samples, int bufferLength)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(samples);

        if (bufferLength < (long)samples * bytesPerFrame)
        {
            throw new ArgumentException(
                $"buffer must hold at least {samples} * {bytesPerFrame} = {(long)samples * bytesPerFrame} bytes, but was {bufferLength}.",
                "buffer"
            );
        }
    }
}
