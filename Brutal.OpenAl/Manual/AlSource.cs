namespace Brutal.OpenAlApi;

public readonly unsafe partial struct AlSource
{
    public void QueueBuffers(ReadOnlySpan<AlBuffer> buffers)
    {
        if (buffers.IsEmpty)
            return;

        var names = MemoryMarshal.Cast<AlBuffer, uint>(buffers);
        fixed (uint* p = names)
            Al.PInvoke.SourceQueueBuffers(Name, names.Length, p);
    }

    public void UnqueueBuffers(Span<AlBuffer> buffers)
    {
        if (buffers.IsEmpty)
            return;

        var names = MemoryMarshal.Cast<AlBuffer, uint>(buffers);
        fixed (uint* p = names)
            Al.PInvoke.SourceUnqueueBuffers(Name, names.Length, p);
    }

    public void SetAuxiliarySend(AlAuxiliaryEffectSlot slot, int send, AlFilter filter = default)
    {
        Al.PInvoke.Source3i(Name, Al.Constants.AL_AUXILIARY_SEND_FILTER, (int)slot.Name, send, (int)filter.Name);
    }
}
