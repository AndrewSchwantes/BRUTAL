namespace Brutal.OpenAlApi;

public readonly unsafe partial struct AlBuffer
{
    public void SetData(AlFormat format, ReadOnlySpan<byte> data, int sampleRate)
    {
        // A successful upload clears any callback; evict its root only once the cookie is gone.
        var previous = PeekCookie(Name);
        fixed (byte* p = data)
            Al.PInvoke.BufferData(Name, (int)format, p, data.Length, sampleRate);
        if (previous != 0 && BoundCookie(Name) == 0)
            BufferCallbacks.TryRemove(previous, out _);
    }
}
