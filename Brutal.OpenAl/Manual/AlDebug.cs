using unsafe DebugTrampolineFn = delegate* unmanaged[Cdecl]<int, int, uint, int, int, byte*, void*, void>;

namespace Brutal.OpenAlApi;

public delegate void AlDebugCallback(
    AlDebugSource source,
    AlDebugType type,
    uint id,
    AlDebugSeverity severity,
    ReadOnlySpan<byte> message
);

public readonly record struct AlDebugMessageInfo(
    AlDebugSource Source,
    AlDebugType Type,
    uint Id,
    AlDebugSeverity Severity,
    int Offset,
    int Length
)
{
    public ReadOnlySpan<byte> Message => AlDebug.ArenaView.Slice(Offset, Length);
}

public readonly ref struct DebugGroupScope
{
    public void Dispose() => AlDebug.PopGroup();
}

public static unsafe partial class AlDebug
{
    public static void Enable() => Al.PInvoke.Enable(Al.Constants.AL_DEBUG_OUTPUT_EXT);

    public static void Disable() => Al.PInvoke.Disable(Al.Constants.AL_DEBUG_OUTPUT_EXT);

    public static void SetCallback(AlDebugCallback? callback)
    {
        var context = Alc.PInvoke.GetCurrentContext().Pointer.Value;
        if (callback is null)
        {
            Al.PInvoke.DebugMessageCallbackEXT(null, null);
            DebugCallbacks.TryRemove(context, out _);
            return;
        }

        // Store before the native call so no early event is missed; restore if the call throws.
        var hadPrevious = DebugCallbacks.TryGetValue(context, out var previous);
        DebugCallbacks[context] = callback;
        try
        {
            // (&DebugTrampoline) is parenthesized because `(AliasName)&expr` parses as a binary AND, not a cast.
            Al.PInvoke.DebugMessageCallbackEXT((void*)(DebugTrampolineFn)(&DebugTrampoline), (void*)context);
        }
        catch
        {
            if (hadPrevious)
                DebugCallbacks[context] = previous!;
            else
                DebugCallbacks.TryRemove(context, out _);
            throw;
        }
    }

    public static void MessageInsert(
        AlDebugSource source,
        AlDebugType type,
        uint id,
        AlDebugSeverity severity,
        string message
    )
    {
        ArgumentNullException.ThrowIfNull(message);
        using var utf8 = new Utf8Buffer(message, stackalloc byte[Utf8Buffer.StackBufferSize]);
        fixed (byte* p = utf8.Span)
            Al.PInvoke.DebugMessageInsertEXT((int)source, (int)type, id, (int)severity, utf8.Length, p);
    }

    public static void MessageInsert(
        AlDebugSource source,
        AlDebugType type,
        uint id,
        AlDebugSeverity severity,
        ReadOnlySpan<byte> message
    )
    {
        fixed (byte* p = message)
        {
            // An empty span pins as null, which the native call rejects even at length 0.
            byte nul = 0;
            Al.PInvoke.DebugMessageInsertEXT(
                (int)source,
                (int)type,
                id,
                (int)severity,
                message.Length,
                p != null ? p : &nul
            );
        }
    }

    public static void MessageControl(
        AlDebugSource source,
        AlDebugType type,
        AlDebugSeverity severity,
        ReadOnlySpan<uint> ids,
        bool enable
    )
    {
        fixed (uint* p = ids)
            Al.PInvoke.DebugMessageControlEXT((int)source, (int)type, (int)severity, ids.Length, p, enable);
    }

    public static void PushGroup(AlDebugSource source, uint id, string message)
    {
        ArgumentNullException.ThrowIfNull(message);
        using var utf8 = new Utf8Buffer(message, stackalloc byte[Utf8Buffer.StackBufferSize]);
        fixed (byte* p = utf8.Span)
            Al.PInvoke.PushDebugGroupEXT((int)source, id, utf8.Length, p);
    }

    public static void PushGroup(AlDebugSource source, uint id, ReadOnlySpan<byte> message)
    {
        fixed (byte* p = message)
        {
            // Same null-substitution rationale as the span MessageInsert above.
            byte nul = 0;
            Al.PInvoke.PushDebugGroupEXT((int)source, id, message.Length, p != null ? p : &nul);
        }
    }

    public static void PopGroup() => Al.PInvoke.PopDebugGroupEXT();

    public static DebugGroupScope GroupScope(AlDebugSource source, uint id, string message)
    {
        PushGroup(source, id, message);
        return new DebugGroupScope();
    }

    public static DebugGroupScope GroupScope(AlDebugSource source, uint id, ReadOnlySpan<byte> message)
    {
        PushGroup(source, id, message);
        return new DebugGroupScope();
    }

    public static int GetMessageLog(Span<AlDebugMessageInfo> messages)
    {
        if (messages.IsEmpty)
            return 0;

        var arena = EnsureArena();
        var chunk = Math.Min(messages.Length, LogChunk);
        Span<int> sources = stackalloc int[chunk];
        Span<int> types = stackalloc int[chunk];
        Span<uint> ids = stackalloc uint[chunk];
        Span<int> severities = stackalloc int[chunk];
        Span<int> lengths = stackalloc int[chunk];

        uint count;
        fixed (int* sourcesPtr = sources)
        fixed (int* typesPtr = types)
        fixed (uint* idsPtr = ids)
        fixed (int* severitiesPtr = severities)
        fixed (int* lengthsPtr = lengths)
        fixed (byte* textPtr = arena)
        {
            count = Al.PInvoke.GetDebugMessageLogEXT(
                (uint)chunk,
                arena.Length,
                sourcesPtr,
                typesPtr,
                idsPtr,
                severitiesPtr,
                lengthsPtr,
                textPtr
            );
        }

        var offset = 0;
        for (var i = 0; i < count; i++)
        {
            // lengths[i] includes the NUL terminator.
            messages[i] = new AlDebugMessageInfo(
                (AlDebugSource)sources[i],
                (AlDebugType)types[i],
                ids[i],
                (AlDebugSeverity)severities[i],
                offset,
                lengths[i] - 1
            );
            offset += lengths[i];
        }
        return (int)count;
    }

    public static void ObjectLabel(AlObjectLabelIdentifier identifier, uint name, string? label)
    {
        // A null label pins as a null pointer with length 0, which the native call treats as "clear".
        using var utf8 = new Utf8Buffer(label, stackalloc byte[Utf8Buffer.StackBufferSize]);
        fixed (byte* p = utf8.Span)
            Al.PInvoke.ObjectLabelEXT((int)identifier, name, utf8.Length, p);
    }

    public static void ObjectLabel(AlObjectLabelIdentifier identifier, uint name, ReadOnlySpan<byte> label)
    {
        fixed (byte* p = label)
        {
            // An empty span pins as null with length 0, which the native call accepts as "clear" -
            // no substitution needed here, unlike the message calls.
            Al.PInvoke.ObjectLabelEXT((int)identifier, name, label.Length, p);
        }
    }

    public static ReadOnlySpan<byte> GetObjectLabel(AlObjectLabelIdentifier identifier, uint name)
    {
        var arena = EnsureArena();
        // Native may leave length unwritten or report more than the arena holds.
        var length = 0;
        fixed (byte* p = arena)
            Al.PInvoke.GetObjectLabelEXT((int)identifier, name, arena.Length, &length, p);
        return arena.AsSpan(0, Math.Clamp(length, 0, arena.Length));
    }
}
