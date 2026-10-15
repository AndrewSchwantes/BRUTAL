namespace Brutal.OpenAlApi;

public sealed partial class Efx
{
    internal Device Device { get; }

    internal Efx(Device device) => Device = device;

    // Device.Reset/Reopen invalidate the send count; the versions never change.
    private int? _efxMajorVersion;
    private int? _efxMinorVersion;
    private int? _maxAuxiliarySends;

    internal void InvalidateDeviceCaches() => _maxAuxiliarySends = null;

    public int EfxMajorVersion
    {
        get
        {
            Device.ThrowIfDisposed();
            var device = Device.Handle;
            return _efxMajorVersion ??= Alc.GetInteger(device, AlcInteger.EfxMajorVersion);
        }
    }

    public int EfxMinorVersion
    {
        get
        {
            Device.ThrowIfDisposed();
            var device = Device.Handle;
            return _efxMinorVersion ??= Alc.GetInteger(device, AlcInteger.EfxMinorVersion);
        }
    }

    public int MaxAuxiliarySends
    {
        get
        {
            Device.ThrowIfDisposed();
            var device = Device.Handle;
            return _maxAuxiliarySends ??= Alc.GetInteger(device, AlcInteger.MaxAuxiliarySends);
        }
    }
}
