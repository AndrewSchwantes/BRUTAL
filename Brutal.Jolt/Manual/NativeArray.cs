namespace Brutal.JoltApi;

/// <summary>A contiguous buffer of <typeparamref name="T"/> the native side allocated for this call.</summary>
/// <remarks>
/// <para>
/// A copy of a native <c>JPH::Array</c>, so it stays valid after the physics system moves on. This object
/// is the block's sole owner: <see cref="Dispose"/> (idempotent) frees it, after which every span from
/// <see cref="AsSpan"/> dangles and the buffer reads as empty. Never dispose one while a span it handed out is
/// still in use.
/// </para>
/// <para>
/// There is no finalizer, so an undisposed buffer leaks rather than being freed behind a span that's still
/// being read. Dispose every one, typically with <c>using</c>.
/// </para>
/// <para>
/// When <typeparamref name="T"/> is a handle, what the buffer holds depends on how Jolt stored it. A
/// container of <c>Ref&lt;T&gt;</c> is copied with its references intact, so the handles live as long as the
/// buffer. A container of raw pointers is copied as pointers only - those handles belong to whatever owned
/// them originally (the <c>PhysicsSystem</c>, the <c>Factory</c>) and die with it, buffer or no buffer.
/// Either way the handles are borrowed: never dispose one individually.
/// </para>
/// <para>
/// The block is freed through Jolt's allocator, so dispose every buffer before
/// <see cref="Jolt.UnregisterTypes"/> / <see cref="Factory.DestroyInstance"/>.
/// </para>
/// </remarks>
public sealed class NativeArray<T> : IDisposable, INullable
    where T : unmanaged
{
    private nint _block;

    // Read once from the block's header: both are fixed for the life of the block, and holding them here means
    // nothing reads the header again once Dispose has freed it
    private readonly unsafe T* _data;
    private readonly int _length;

    internal unsafe NativeArray(nint block)
    {
        _block = block;
        if (block == 0)
            return;

        var header = (NativeArrayBlock*)block;
        _data = (T*)header->Data;
        _length = checked((int)header->Count);
    }

    /// <summary>Whether this is the empty (never allocated, or already disposed) buffer.</summary>
    public bool IsNull() => _block == 0;

    /// <summary>The number of elements in the buffer; 0 once it has been disposed.</summary>
    public int Length => _block == 0 ? 0 : _length;

    /// <summary>The buffer's contents, valid until <see cref="Dispose"/>; empty afterwards.</summary>
    [return: Brutal.Attributes.Fixed]
    public unsafe ReadOnlySpan<T> AsSpan() => _block == 0 ? default : new ReadOnlySpan<T>(_data, _length);

    /// <summary>Frees the buffer. Idempotent; the spans it handed out are dangling afterwards.</summary>
    public void Dispose()
    {
        var block = Interlocked.Exchange(ref _block, 0);
        if (block != 0)
            Jolt.PInvoke.NativeArrayDestroy(block);
    }
}
