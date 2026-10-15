using System.Collections.ObjectModel;
using CommunityToolkit.HighPerformance.Buffers;

namespace Brutal.OpenAlApi;

public readonly record struct AlcClockLatency(long ClockTime, long Latency);

public sealed class Device : IDisposable, INullable
{
    private readonly bool _ownedByLoopback;

    public AlcDevice Handle { get; private set; }

    internal Device(AlcDevice handle, bool ownedByLoopback = false)
    {
        Handle = handle;
        _ownedByLoopback = ownedByLoopback;
    }

    private AlcDevice CheckedHandle
    {
        get
        {
            ThrowIfDisposed();
            return Handle;
        }
    }

    internal void ThrowIfDisposed() => ObjectDisposedException.ThrowIf(IsNull(), this);

    private Efx? _efx;

    // Reopen invalidates the specifier caches; Reset and Reopen invalidate the HRTF cache.
    private string? _specifier;
    private string? _allDevicesSpecifier;
    private ReadOnlyCollection<string>? _extensions;
    private ReadOnlyCollection<string>? _hrtfSpecifiers;
    private int? _majorVersion;
    private int? _minorVersion;

    public Efx Efx
    {
        get
        {
            ThrowIfDisposed();
            return _efx ??= new Efx(this);
        }
    }

    public bool IsNull() => Handle.IsNull();

    public string? Specifier
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsNull(), this);
            return _specifier ??= Alc.GetString(Handle, AlcGetString.DeviceSpecifier);
        }
    }

    public string? AllDevicesSpecifier
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsNull(), this);
            return _allDevicesSpecifier ??= Alc.GetString(Handle, AlcGetString.AllDevicesSpecifier);
        }
    }

    public IReadOnlyList<string> Extensions
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsNull(), this);
            if (_extensions is null)
            {
                var s = Alc.GetString(Handle, AlcGetString.Extensions);
                _extensions = Array.AsReadOnly(
                    string.IsNullOrEmpty(s) ? [] : s.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                );
            }
            return _extensions;
        }
    }

    public int MajorVersion
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsNull(), this);
            return _majorVersion ??= Alc.GetInteger(Handle, AlcInteger.MajorVersion);
        }
    }

    public int MinorVersion
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsNull(), this);
            return _minorVersion ??= Alc.GetInteger(Handle, AlcInteger.MinorVersion);
        }
    }

    public unsafe (AlcContextAttribute Attribute, int Value)[] Attributes
    {
        get
        {
            // Two-call native protocol, encapsulated: ALC_ATTRIBUTES_SIZE reports the list length
            // (including its zero terminator), ALC_ALL_ATTRIBUTES fills the buffer. The terminator
            // is trimmed and the flat ints paired up.
            var size = Alc.GetInteger(CheckedHandle, AlcInteger.AttributesSize);
            if (size <= 0)
                return [];

            using var owner = SpanOwner<int>.Allocate(size);
            var flat = owner.Span;
            fixed (int* p = flat)
                Alc.PInvoke.GetIntegerv(Handle, (int)AlcInteger.AllAttributes, size, p);

            var count = flat[size - 1] == 0 ? size - 1 : size;
            var pairs = new (AlcContextAttribute, int)[count / 2];
            for (var i = 0; i < pairs.Length; i++)
                pairs[i] = ((AlcContextAttribute)flat[i * 2], flat[i * 2 + 1]);
            return pairs;
        }
    }

    public bool IsExtensionPresent(string name) => Alc.IsExtensionPresent(CheckedHandle, name);

    public unsafe void* GetProcAddress(string name) => Alc.GetProcAddress(CheckedHandle, name);

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

    public Context CreateContext(Context.CreateInfo info = default)
    {
        ObjectDisposedException.ThrowIf(IsNull(), this);
        return new(this, info);
    }

    public void Pause() => Alc.PInvoke.DevicePauseSOFT(CheckedHandle);

    public void Resume() => Alc.PInvoke.DeviceResumeSOFT(CheckedHandle);

    public bool Connected => Alc.GetInteger(CheckedHandle, AlcInteger.Connected) != 0;

    public unsafe long ClockTime
    {
        get
        {
            long value = 0;
            Alc.PInvoke.GetInteger64vSOFT(CheckedHandle, Alc.Constants.ALC_DEVICE_CLOCK_SOFT, 1, &value);
            return value;
        }
    }

    public unsafe long Latency
    {
        get
        {
            long value = 0;
            Alc.PInvoke.GetInteger64vSOFT(CheckedHandle, Alc.Constants.ALC_DEVICE_LATENCY_SOFT, 1, &value);
            return value;
        }
    }

    public unsafe AlcClockLatency ClockLatency
    {
        get
        {
            var pair = stackalloc long[2];
            Alc.PInvoke.GetInteger64vSOFT(CheckedHandle, Alc.Constants.ALC_DEVICE_CLOCK_LATENCY_SOFT, 2, pair);
            return new AlcClockLatency(pair[0], pair[1]);
        }
    }

    public AlcHrtfStatus HrtfStatus => (AlcHrtfStatus)Alc.GetInteger(CheckedHandle, AlcInteger.HrtfStatus);

    public IReadOnlyList<string> HrtfSpecifiers
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsNull(), this);
            if (_hrtfSpecifiers is null)
            {
                var count = Alc.GetInteger(Handle, AlcInteger.NumHrtfSpecifiers);
                var names = new string[count];
                for (var i = 0; i < count; i++)
                {
                    unsafe
                    {
                        names[i] =
                            StringUtils.Utf8ToString(
                                Alc.PInvoke.GetStringiSOFT(Handle, Alc.Constants.ALC_HRTF_SPECIFIER_SOFT, i)
                            ) ?? string.Empty;
                    }
                }
                _hrtfSpecifiers = Array.AsReadOnly(names);
            }
            return _hrtfSpecifiers;
        }
    }

    public unsafe void Reset(ReadOnlySpan<(AlcContextAttribute Attribute, int Value)> attributes = default)
    {
        ObjectDisposedException.ThrowIf(IsNull(), this);
        using var owner = Alc.FlattenAttributes(attributes);
        fixed (int* p = owner.Span)
        {
            if (!Alc.PInvoke.ResetDeviceSOFT(Handle, p))
                throw new AlcException(Alc.GetError(Handle), "alcResetDeviceSOFT failed.");
        }

        // A reset renegotiates the attribute list: the HRTF set and the auxiliary-send count can
        // both change. The device's identity (specifiers) cannot.
        _hrtfSpecifiers = null;
        _efx?.InvalidateDeviceCaches();
    }

    public unsafe void Reopen(
        string? deviceName,
        ReadOnlySpan<(AlcContextAttribute Attribute, int Value)> attributes = default
    )
    {
        ObjectDisposedException.ThrowIf(IsNull(), this);
        using var owner = Alc.FlattenAttributes(attributes);
        using var utf8 = new Utf8Buffer(deviceName, stackalloc byte[Utf8Buffer.StackBufferSize]);
        fixed (int* attribs = owner.Span)
        fixed (byte* name = utf8.Span)
        {
            if (!Alc.PInvoke.ReopenDeviceSOFT(Handle, name, attribs))
                throw new AlcException(Alc.GetError(Handle), "alcReopenDeviceSOFT failed.");
        }

        // A reopen can change the underlying device (new specifiers) and renegotiates the
        // attribute list like Reset does. Versions and the extension list are properties of the
        // loaded implementation, not the device, and survive.
        _specifier = null;
        _allDevicesSpecifier = null;
        _hrtfSpecifiers = null;
        _efx?.InvalidateDeviceCaches();
    }

    public AlcOutputMode OutputMode => (AlcOutputMode)Alc.GetInteger(CheckedHandle, AlcInteger.OutputMode);

    public void Close()
    {
        if (_ownedByLoopback)
            throw new InvalidOperationException("Close the owning LoopbackDevice instead.");
        ObjectDisposedException.ThrowIf(IsNull(), this);
        CloseUnchecked();
    }

    public void Dispose()
    {
        if (_ownedByLoopback)
            return;
        DisposeUnchecked();
    }

    // Close() without the ownership and disposal guards. Also the entry point LoopbackDevice
    // uses on its plumbing Device, whose public Close() rejects direct use.
    internal void CloseUnchecked()
    {
        if (!CloseCore())
            throw new AlcException(Alc.GetError(), "alcCloseDevice failed (invalid or already-closed handle).");
    }

    // Dispose() without the ownership guard. Also the entry point LoopbackDevice uses on its
    // plumbing Device, whose public Dispose() is a no-op.
    internal void DisposeUnchecked()
    {
        if (IsNull())
            return;

        // Dispose must not throw; Close() is the path that reports a failed close.
        CloseCore();
    }

    private bool CloseCore()
    {
        // Evict before the native close so a freed context address is never left mapped.
        var orphans = Context.EvictDeviceEntries(this);

        // Closing also destroys the device's buffers without any per-buffer delete running, so
        // the buffer-callback handler roots recorded under this device are swept with it.
        AlBuffer.EvictDeviceRoots(Handle.Pointer.Value);

        // A successful close frees the device, so it must not be queried afterwards; on failure
        // the global error holds the reason.
        var closed = Alc.PInvoke.CloseDevice(Handle);

        // On failure the contexts are still alive natively, so only mark them orphaned on success.
        if (closed)
            Context.MarkOrphaned(orphans);

        Handle = AlcDevice.Null;
        return closed;
    }

    public static implicit operator AlcDevice(Device device) => device.Handle;
}
