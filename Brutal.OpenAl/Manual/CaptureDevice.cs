namespace Brutal.OpenAlApi;

public sealed partial class CaptureDevice : IDisposable, INullable
{
    public AlcCaptureDevice Handle { get; private set; }

    // Bytes per sample frame of the capture format; bounds Samples().
    private readonly int _bytesPerFrame;

    private string? _specifier;

    internal CaptureDevice(AlcCaptureDevice handle, int bytesPerFrame)
    {
        Handle = handle;
        _bytesPerFrame = bytesPerFrame;
    }

    private AlcCaptureDevice CheckedHandle
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsNull(), this);
            return Handle;
        }
    }

    public bool IsNull() => Handle.IsNull();

    public string? Specifier
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsNull(), this);
            return _specifier ??= Alc.GetString(Handle, AlcGetString.CaptureDeviceSpecifier);
        }
    }

    public int AvailableSamples => Alc.GetInteger(CheckedHandle, AlcInteger.CaptureSamples);

    public AlcError GetError()
    {
        ObjectDisposedException.ThrowIf(IsNull(), this);
        return Alc.GetError(Handle);
    }

    public void ThrowIfError()
    {
        ObjectDisposedException.ThrowIf(IsNull(), this);
        Alc.ThrowIfError(Handle);
    }

    public void Start() => Alc.PInvoke.CaptureStart(CheckedHandle);

    public void Stop() => Alc.PInvoke.CaptureStop(CheckedHandle);

    public void Samples(Span<byte> buffer, int samples)
    {
        ObjectDisposedException.ThrowIf(IsNull(), this);
        CheckBufferLength(_bytesPerFrame, samples, buffer.Length);

        unsafe
        {
            fixed (byte* p = buffer)
                Alc.PInvoke.CaptureSamples(Handle, p, samples);
        }
    }

    public void Close()
    {
        ObjectDisposedException.ThrowIf(IsNull(), this);
        if (!CloseCore())
            throw new AlcException(Alc.GetError(), "alcCaptureCloseDevice failed.");
    }

    public void Dispose()
    {
        if (IsNull())
            return;

        // Dispose must not throw; Close() is the path that reports a failed close.
        CloseCore();
    }

    private bool CloseCore()
    {
        var closed = Alc.PInvoke.CaptureCloseDevice(Handle);
        Handle = AlcCaptureDevice.Null;
        return closed;
    }

    public static implicit operator AlcCaptureDevice(CaptureDevice capture) => capture.Handle;
}
