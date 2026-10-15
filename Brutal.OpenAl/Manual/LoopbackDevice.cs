namespace Brutal.OpenAlApi;

public sealed class LoopbackDevice : IDisposable, INullable
{
    private readonly Device _device;

    // Bytes per sample frame of the device's render format, captured on each CreateContext; 0
    // until the first context establishes a format. Render's span bound - see CheckBufferLength.
    private int _bytesPerFrame;

    private LoopbackDevice(AlcDevice handle) => _device = new Device(handle, ownedByLoopback: true);

    public static unsafe LoopbackDevice Open(string? deviceName = null)
    {
        // No trustworthy open device yet, so a null return is diagnosed via the global error - same
        // protocol as Alc.OpenDevice. A null name pins as a null native pointer (the default).
        using var utf8 = new Utf8Buffer(deviceName, stackalloc byte[Utf8Buffer.StackBufferSize]);
        AlcDevice handle;
        fixed (byte* p = utf8.Span)
            handle = Alc.PInvoke.LoopbackOpenDeviceSOFT(p);
        if (handle.IsNull())
            throw new AlcException(Alc.GetError(), "alcLoopbackOpenDeviceSOFT failed.");
        return new LoopbackDevice(handle);
    }

    public AlcDevice Handle => _device.Handle;

    public bool IsNull() => _device.IsNull();

    private AlcDevice CheckedHandle
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsNull(), this);
            return Handle;
        }
    }

    public bool IsRenderFormatSupported(int frequency, AlcChannelLayout channels, AlcSampleType sampleType) =>
        Alc.PInvoke.IsRenderFormatSupportedSOFT(CheckedHandle, frequency, (int)channels, (int)sampleType);

    public Context CreateContext(Context.CreateInfo info = default)
    {
        ObjectDisposedException.ThrowIf(IsNull(), this);
        var context = _device.CreateContext(info);
        _bytesPerFrame = QueryBytesPerFrame();
        return context;
    }

    // Queried back from the device so it reflects the format native chose; unknown values yield 0.
    private unsafe int QueryBytesPerFrame()
    {
        var channelCount = (AlcChannelLayout)Query(Alc.Constants.ALC_FORMAT_CHANNELS_SOFT) switch
        {
            AlcChannelLayout.Mono => 1,
            AlcChannelLayout.Stereo => 2,
            AlcChannelLayout.Quad => 4,
            AlcChannelLayout.Surround51 => 6,
            AlcChannelLayout.Surround61 => 7,
            AlcChannelLayout.Surround71 => 8,
            // B-Format with ambisonic order N carries (N + 1)^2 channels.
            AlcChannelLayout.Bformat3d => AmbisonicChannels(Query(Alc.Constants.ALC_AMBISONIC_ORDER_SOFT)),
            _ => 0,
        };
        var sampleSize = (AlcSampleType)Query(Alc.Constants.ALC_FORMAT_TYPE_SOFT) switch
        {
            AlcSampleType.Byte or AlcSampleType.UnsignedByte => 1,
            AlcSampleType.Short or AlcSampleType.UnsignedShort => 2,
            AlcSampleType.Int or AlcSampleType.UnsignedInt or AlcSampleType.Float => 4,
            _ => 0,
        };
        return channelCount * sampleSize;

        static int AmbisonicChannels(int order) => (order + 1) * (order + 1);

        int Query(int param)
        {
            var value = 0;
            Alc.PInvoke.GetIntegerv(Handle, param, 1, &value);
            return value;
        }
    }

    public unsafe void Render(Span<byte> buffer, int sampleFrames)
    {
        ObjectDisposedException.ThrowIf(IsNull(), this);

        // alcRenderSamplesSOFT takes no buffer size, so the length must be checked here.
        if (_bytesPerFrame == 0)
        {
            throw new InvalidOperationException(
                "No render format is established. Create a context on this device (with Frequency/"
                    + "FormatChannels/FormatType attributes) before rendering."
            );
        }
        CheckBufferLength(_bytesPerFrame, sampleFrames, buffer.Length);

        fixed (byte* p = buffer)
            Alc.PInvoke.RenderSamplesSOFT(Handle, p, sampleFrames);

        Al.ThrowCallbackExceptions();
    }

    // Multiply as long so the bound cannot overflow.
    private static void CheckBufferLength(int bytesPerFrame, int sampleFrames, int bufferLength)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(sampleFrames);

        if (bufferLength < (long)sampleFrames * bytesPerFrame)
        {
            throw new ArgumentException(
                $"buffer must hold at least {sampleFrames} frames * {bytesPerFrame} bytes = {(long)sampleFrames * bytesPerFrame} bytes, but was {bufferLength}.",
                "buffer"
            );
        }
    }

    public void Render(Span<short> buffer, int sampleFrames) => Render(MemoryMarshal.AsBytes(buffer), sampleFrames);

    public void Render(Span<float> buffer, int sampleFrames) => Render(MemoryMarshal.AsBytes(buffer), sampleFrames);

    public void Close()
    {
        // Guarded here so the exception reports the LoopbackDevice, not the plumbing Device.
        ObjectDisposedException.ThrowIf(IsNull(), this);
        _device.CloseUnchecked();
    }

    public void Dispose() => _device.DisposeUnchecked();

    public static implicit operator AlcDevice(LoopbackDevice device) => device.Handle;
}
