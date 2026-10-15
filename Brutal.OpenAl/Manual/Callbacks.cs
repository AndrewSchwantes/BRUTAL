using unsafe AlcEventTrampolineFn = delegate* unmanaged[Cdecl]<int, int, nint, int, byte*, void*, void>;
using unsafe AlEventTrampolineFn = delegate* unmanaged[Cdecl]<int, uint, uint, int, byte*, void*, void>;
using unsafe BufferTrampolineFn = delegate* unmanaged[Cdecl]<void*, void*, int, int>;

namespace Brutal.OpenAlApi;

public delegate int AlBufferCallback(Span<byte> destination);

public delegate void AlEventCallback(AlEventType eventType, uint @object, uint parameter, ReadOnlySpan<byte> message);

public delegate void AlcEventCallback(
    AlcEventType eventType,
    AlcDeviceType deviceType,
    AlcDevice device,
    ReadOnlySpan<byte> message
);

public static unsafe partial class Al
{
    public static void ThrowCallbackExceptions() => CallbackExceptions.ThrowIfAny();

    public static void EventControl(ReadOnlySpan<AlEventType> types, bool enable)
    {
        var raw = MemoryMarshal.Cast<AlEventType, int>(types);
        fixed (int* p = raw)
            PInvoke.EventControlSOFT(raw.Length, p, enable);
    }

    public static void SetEventCallback(AlEventCallback? callback)
    {
        var context = Alc.PInvoke.GetCurrentContext().Pointer.Value;
        if (callback is null)
        {
            PInvoke.EventCallbackSOFT(null, null);
            EventCallbacks.TryRemove(context, out _);
            return;
        }

        // Store before the native call so no early event is missed; restore if the call throws.
        var hadPrevious = EventCallbacks.TryGetValue(context, out var previous);
        EventCallbacks[context] = callback;
        try
        {
            // (&Trampoline) is parenthesized because `(AliasName)&expr` parses as a binary AND, not a cast.
            PInvoke.EventCallbackSOFT((void*)(AlEventTrampolineFn)(&AlEventTrampoline), (void*)context);
        }
        catch
        {
            if (hadPrevious)
                EventCallbacks[context] = previous!;
            else
                EventCallbacks.TryRemove(context, out _);
            throw;
        }
    }
}

public static unsafe partial class Alc
{
    public static bool IsEventSupported(AlcEventType eventType, AlcDeviceType deviceType) =>
        PInvoke.EventIsSupportedSOFT((int)eventType, (int)deviceType) == Constants.ALC_EVENT_SUPPORTED_SOFT;

    public static bool EventControl(ReadOnlySpan<AlcEventType> events, bool enable)
    {
        var raw = MemoryMarshal.Cast<AlcEventType, int>(events);
        fixed (int* p = raw)
            return PInvoke.EventControlSOFT(raw.Length, p, enable);
    }

    public static void SetEventCallback(AlcEventCallback? callback)
    {
        if (callback is null)
        {
            PInvoke.EventCallbackSOFT(null, null);
            _systemEventCallback = null;
            return;
        }

        // Store before the native call so no early event is missed; restore if the call throws.
        var previous = _systemEventCallback;
        _systemEventCallback = callback;
        try
        {
            PInvoke.EventCallbackSOFT((AlcEventTrampolineFn)(&AlcEventTrampoline), null);
        }
        catch
        {
            _systemEventCallback = previous;
            throw;
        }
    }
}

public readonly unsafe partial struct AlBuffer
{
    public void SetCallback(AlFormat format, int frequency, AlBufferCallback callback)
    {
        ArgumentNullException.ThrowIfNull(callback);
        var previous = BoundCookie(Name);
        var cookie = (nint)Interlocked.Increment(ref _cookieCounter);
        BufferCallbacks[cookie] = (callback, 0);
        Al.PInvoke.BufferCallbackSOFT(
            Name,
            (int)format,
            frequency,
            (BufferTrampolineFn)(&BufferTrampoline),
            (void*)cookie
        );

        // Failure is reported only on the error latch, so read back the bound cookie to see if it took.
        if (BoundCookie(Name) == cookie)
        {
            var device = Alc.PInvoke.GetContextsDevice(Alc.PInvoke.GetCurrentContext());
            BufferCallbacks[cookie] = (callback, device.Pointer.Value);
            if (previous != 0)
                BufferCallbacks.TryRemove(previous, out _);
        }
        else
        {
            BufferCallbacks.TryRemove(cookie, out _);
        }
    }

    public void ClearCallback()
    {
        // A NULL callback is rejected, so detach by re-specifying empty data.
        var previous = BoundCookie(Name);
        Al.PInvoke.BufferData(Name, (int)AlFormat.Mono16, null, 0, 1);
        if (previous != 0 && BoundCookie(Name) == 0)
            BufferCallbacks.TryRemove(previous, out _);
    }
}
