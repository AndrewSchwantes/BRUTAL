namespace Brutal.OpenAlApi;

public static unsafe partial class Alc
{
    public static Device OpenDevice(string? name = null)
    {
        // A null name pins as a null native pointer: the default-device case.
        using var utf8 = new Utf8Buffer(name, stackalloc byte[Utf8Buffer.StackBufferSize]);
        AlcDevice device;
        fixed (byte* p = utf8.Span)
            device = PInvoke.OpenDevice(p);

        // No trustworthy open device yet, so a null return is diagnosed via the global error.
        if (device.IsNull())
            throw new AlcException(GetError(), "alcOpenDevice failed.");
        return new Device(device);
    }

    public static CaptureDevice CaptureOpenDevice(string? name, uint frequency, AlFormat format, int bufferSize)
    {
        // Without a known frame size, CaptureDevice.Samples cannot bound its destination buffer
        // (alcCaptureSamples writes however many frames it is asked for). Extension formats go
        // through the overload below, which takes the frame size explicitly.
        if (CaptureDevice.FrameSize(format) is not { } bytesPerFrame)
        {
            throw new ArgumentOutOfRangeException(
                nameof(format),
                format,
                "Not one of the four standard AlFormat values, so its frame size is unknown. Use the "
                    + "CaptureOpenDevice overload taking bytesPerFrame for an extension format."
            );
        }

        return CaptureOpenDevice(name, frequency, format, bufferSize, bytesPerFrame);
    }

    public static CaptureDevice CaptureOpenDevice(
        string? name,
        uint frequency,
        AlFormat format,
        int bufferSize,
        int bytesPerFrame
    )
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(bytesPerFrame);

        // A null name pins as a null native pointer: the default capture device case.
        using var utf8 = new Utf8Buffer(name, stackalloc byte[Utf8Buffer.StackBufferSize]);
        AlcCaptureDevice handle;
        fixed (byte* p = utf8.Span)
            handle = PInvoke.CaptureOpenDevice(p, frequency, (int)format, bufferSize);

        if (handle.IsNull())
            throw new AlcException(GetError(), "alcCaptureOpenDevice failed.");

        return new CaptureDevice(handle, bytesPerFrame);
    }

    public static string[] GetDeviceSpecifiers() => EnumerateDevices(AlcGetString.DeviceSpecifier);

    public static string[] GetAllDeviceSpecifiers() => EnumerateDevices(AlcGetString.AllDevicesSpecifier);

    public static string[] GetCaptureDeviceSpecifiers() => EnumerateDevices(AlcGetString.CaptureDeviceSpecifier);

    public static string? GetDefaultDeviceSpecifier() => GetNullDeviceString(AlcGetString.DefaultDeviceSpecifier);

    public static string? GetDefaultAllDevicesSpecifier() =>
        GetNullDeviceString(AlcGetString.DefaultAllDevicesSpecifier);

    public static string? GetCaptureDefaultDeviceSpecifier() =>
        GetNullDeviceString(AlcGetString.CaptureDefaultDeviceSpecifier);
}
