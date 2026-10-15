namespace Brutal.JoltApi;

/// <summary>
/// An owned native buffer of objects Jolt stores by value and C# sees as handles - a query's
/// <see cref="TransformedShape"/> hits, for one. Dispose it when done.
/// </summary>
/// <remarks>
/// <para>
/// The buffer owns its elements, and whatever they reference: a <see cref="TransformedShape"/> holds a reference to
/// its shape, so the shapes stay alive until the buffer is disposed even if the bodies they came from are gone.
/// </para>
/// <para>
/// The handles it hands out are borrowed - each points at an element inside the buffer, so never Dispose one, and
/// don't use one after the buffer is disposed.
/// </para>
/// <para>
/// There is no finalizer, so an undisposed buffer leaks rather than being freed behind a handle that's still in
/// use. Dispose every one, typically with <c>using</c>, and before <see cref="Jolt.UnregisterTypes"/> /
/// <see cref="Factory.DestroyInstance"/>, since the block is freed through Jolt's allocator.
/// </para>
/// </remarks>
public sealed class NativeHandleArray<T> : IDisposable, INullable
    where T : unmanaged
{
    private nint _block;

    // Read once: data, count and stride are fixed for the life of the block
    private readonly nint _data;
    private readonly int _length;
    private readonly int _stride;

    internal unsafe NativeHandleArray(nint block)
    {
        _block = block;
        if (block == 0)
            return;

        var header = (NativeArrayBlock*)block;
        _data = header->Data;
        _length = checked((int)header->Count);
        _stride = checked((int)header->Stride);
    }

    /// <summary>Whether this is the empty (never allocated, or already disposed) buffer.</summary>
    public bool IsNull() => _block == 0;

    /// <summary>The number of elements; 0 once the buffer has been disposed.</summary>
    public int Length => _block == 0 ? 0 : _length;

    /// <summary>A borrowed handle onto element <paramref name="index"/>.</summary>
    /// <exception cref="ArgumentOutOfRangeException">The index is outside the buffer.</exception>
    public T this[int index] => AsSpan()[index];

    /// <summary>The elements as borrowed handles, valid until <see cref="Dispose"/>; empty afterwards.</summary>
    public NativeHandleSpan<T> AsSpan() => _block == 0 ? default : new NativeHandleSpan<T>(_data, _length, _stride);

    /// <summary>Walks the elements in order, as borrowed handles.</summary>
    public NativeHandleSpan<T>.Enumerator GetEnumerator() => AsSpan().GetEnumerator();

    /// <summary>Frees the buffer and releases what its elements reference. Idempotent.</summary>
    public void Dispose()
    {
        var block = Interlocked.Exchange(ref _block, 0);
        if (block != 0)
            Jolt.PInvoke.NativeArrayDestroy(block);
    }
}
