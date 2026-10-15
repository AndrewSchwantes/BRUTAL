using System.Collections.Concurrent;

namespace Brutal.OpenAlApi;

// The process-wide latch for exceptions thrown by managed callback handlers. Capped so an
// undrained process cannot grow unboundedly: the first Capacity exceptions are kept, the rest
// only counted.
internal sealed class CallbackExceptionAggregator
{
    private const int Capacity = 16;
    private readonly List<Exception> _exceptions = [];
    private int _dropped;
    private readonly Lock _lock = new();

    public void Push(string message, Exception inner)
    {
        try
        {
            lock (_lock)
            {
                if (_exceptions.Count < Capacity)
                    _exceptions.Add(new Exception(message, inner));
                else
                    _dropped++;
            }
        }
        catch
        {
            // Never let the latch itself throw back into a native frame.
        }
    }

    public void ThrowIfAny()
    {
        Exception exception;
        lock (_lock)
        {
            if (_exceptions.Count == 0)
                return;
            if (_dropped > 0)
                _exceptions.Add(new Exception($"{_dropped} further callback exception(s) were dropped."));
            exception = _exceptions.Count == 1 ? _exceptions[0] : new AggregateException([.. _exceptions]);
            _exceptions.Clear();
            _dropped = 0;
        }
        throw exception;
    }
}

public static unsafe partial class Al
{
    internal static readonly CallbackExceptionAggregator CallbackExceptions = new();

    // Keyed by the current context's address, which is passed as userParam.
    internal static readonly ConcurrentDictionary<nint, AlEventCallback> EventCallbacks = new();

    // Called from Context teardown for every context address that stops being valid.
    internal static void EvictContextRoots(nint contextPointer)
    {
        EventCallbacks.TryRemove(contextPointer, out _);
        AlDebug.EvictContextRoots(contextPointer);
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void AlEventTrampoline(
        int eventType,
        uint obj,
        uint param,
        int length,
        byte* message,
        void* userParam
    )
    {
        try
        {
            if (EventCallbacks.TryGetValue((nint)userParam, out var callback))
                callback((AlEventType)eventType, obj, param, new ReadOnlySpan<byte>(message, length));
        }
        catch (Exception ex)
        {
            CallbackExceptions.Push("Error in Al event callback.", ex);
            // the event is dropped
        }
    }
}

public static unsafe partial class Alc
{
    // ALC_SOFT_system_events is process-global, so a single static field is enough rooting.
    internal static AlcEventCallback? SystemEventCallback => _systemEventCallback;
    private static AlcEventCallback? _systemEventCallback;

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void AlcEventTrampoline(
        int eventType,
        int deviceType,
        nint device,
        int length,
        byte* message,
        void* userParam
    )
    {
        try
        {
            _systemEventCallback?.Invoke(
                (AlcEventType)eventType,
                (AlcDeviceType)deviceType,
                (void*)device,
                new ReadOnlySpan<byte>(message, length)
            );
        }
        catch (Exception ex)
        {
            Al.CallbackExceptions.Push("Error in Alc event callback.", ex);
            // the event is dropped
        }
    }
}

public readonly unsafe partial struct AlBuffer
{
    // Keyed by a per-registration cookie, not the buffer name, since names are per-device.
    private static readonly ConcurrentDictionary<nint, (AlBufferCallback Callback, nint Device)> BufferCallbacks =
        new();

    private static long _cookieCounter;

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static int BufferTrampoline(void* userptr, void* sampleData, int numBytes)
    {
        try
        {
            return BufferCallbacks.TryGetValue((nint)userptr, out var entry)
                ? entry.Callback(new Span<byte>(sampleData, numBytes))
                : 0;
        }
        catch (Exception ex)
        {
            Al.CallbackExceptions.Push("Error in AlBuffer callback.", ex);
            return 0; // the stream goes silent
        }
    }

    // The cookie bound to this buffer name, or 0 when no callback is registered.
    private static nint BoundCookie(uint name)
    {
        void* existing = null;
        Al.PInvoke.GetBufferPtrSOFT(name, Al.Constants.AL_BUFFER_CALLBACK_USER_PARAM_SOFT, &existing);
        return (nint)existing;
    }

    private static nint PeekCookie(uint name) => BufferCallbacks.IsEmpty ? 0 : BoundCookie(name);

    // Read cookies before the delete and evict after, only for names that were actually deleted.
    internal static void DeleteAndEvictCallbackRoots(uint* names, int count)
    {
        if (BufferCallbacks.IsEmpty)
        {
            Al.PInvoke.DeleteBuffers(count, names);
            return;
        }

        // Deletes are not hot enough to justify pooling; the stackalloc covers every sane bulk.
        Span<nint> cookies = count <= 64 ? stackalloc nint[count] : new nint[count];
        for (var i = 0; i < count; i++)
            cookies[i] = BoundCookie(names[i]);

        Al.PInvoke.DeleteBuffers(count, names);

        for (var i = 0; i < count; i++)
        {
            if (cookies[i] != 0 && !Al.PInvoke.IsBuffer(names[i]))
                BufferCallbacks.TryRemove(cookies[i], out _);
        }
    }

    internal static void EvictDeviceRoots(nint devicePointer)
    {
        foreach (var entry in BufferCallbacks)
        {
            if (entry.Value.Device == devicePointer)
                BufferCallbacks.TryRemove(entry);
        }
    }
}
