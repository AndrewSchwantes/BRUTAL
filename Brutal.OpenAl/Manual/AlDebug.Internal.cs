using System.Collections.Concurrent;

namespace Brutal.OpenAlApi;

public static unsafe partial class AlDebug
{
    internal static readonly ConcurrentDictionary<nint, AlDebugCallback> DebugCallbacks = new();

    // Every pull overwrites the arena from offset 0, so returned spans are valid until the next pull.
    private static byte[]? _arena;

    // Matches the native log capacity; also bounds the stackalloc arrays.
    private const int LogChunk = 64;

    internal static ReadOnlySpan<byte> ArenaView => _arena;

    private static byte[] EnsureArena()
    {
        if (_arena is null)
        {
            var messageLength = Al.PInvoke.GetInteger(Al.Constants.AL_MAX_DEBUG_MESSAGE_LENGTH_EXT);
            var labelLength = Al.PInvoke.GetInteger(Al.Constants.AL_MAX_LABEL_LENGTH_EXT);
            var size = Math.Max(LogChunk * messageLength, labelLength);

            // Do not cache a zero size; the next call re-queries.
            if (size <= 0)
                return [];

            _arena = new byte[size];
        }
        return _arena;
    }

    // Called from Al.EvictContextRoots for every context address that stops being valid.
    internal static void EvictContextRoots(nint contextPointer) => DebugCallbacks.TryRemove(contextPointer, out _);

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void DebugTrampoline(
        int source,
        int type,
        uint id,
        int severity,
        int length,
        byte* message,
        void* userParam
    )
    {
        try
        {
            if (DebugCallbacks.TryGetValue((nint)userParam, out var callback))
            {
                callback(
                    (AlDebugSource)source,
                    (AlDebugType)type,
                    id,
                    (AlDebugSeverity)severity,
                    new ReadOnlySpan<byte>(message, length)
                );
            }
        }
        catch (Exception ex)
        {
            Al.CallbackExceptions.Push("Error in AlDebug callback.", ex);
            // the message is dropped
        }
    }
}
