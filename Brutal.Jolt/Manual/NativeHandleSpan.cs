using System.Runtime.CompilerServices;

namespace Brutal.JoltApi;

/// <summary>
/// A borrowed view over a native container that stores <typeparamref name="T"/> <em>by value</em>, handing
/// out one borrowed handle per element.
/// </summary>
/// <remarks>
/// <para>
/// Jolt stores some containers as whole objects laid end to end (<c>PhysicsScene</c>'s body settings, for
/// instance), while C# only ever sees those types as handles - an address. A <see cref="Span{T}"/> of
/// handles would be wrong, because the memory holds N objects of the native element size rather than N
/// pointers, and a copied buffer would be worse: an edit through a copy never reaches the container. So
/// this type keeps the storage base together with the element stride the native side measured, and
/// <see cref="this[int]"/> forms the handle for element <c>i</c> as <c>base + i * stride</c>. Reading or
/// writing through that handle touches the owner's own element.
/// </para>
/// <para>
/// Every handle it hands out is <em>borrowed</em>: it belongs to the container, so it must never be
/// disposed. <c>Dispose</c> on one of these handles is a native <c>delete</c>, and the pointer is an
/// element <em>inside</em> the container's own allocation rather than an object of its own, so deleting it
/// corrupts the heap. Never write <c>using var settings = scene.GetBodies()[0];</c> and never pass one to
/// anything that takes ownership - the same rule every borrowed handle in this binding carries
/// (<see cref="NativeArray{T}"/>, <c>BodyManager.GetBodies()</c>, and every getter whose doc says "the
/// returned handle is borrowed from its owner").
/// </para>
/// <para>
/// A handle also dangles the moment the container is resized (anything that adds or removes elements) or
/// its owner is destroyed. The view itself has the same lifetime: being a <c>ref struct</c> keeps it off
/// the heap and out of closures, but the compiler can't prove the owner outlives the stack frame, so treat
/// it exactly like a <c>[Fixed]</c> span - read it inside the scope that produced it, and take it again
/// after anything that could have grown the container rather than reusing the old one.
/// </para>
/// </remarks>
public readonly ref struct NativeHandleSpan<T>
    where T : unmanaged
{
    private readonly nint _data;
    private readonly int _count;
    private readonly int _stride;

    internal NativeHandleSpan(NativeStridedView view)
    {
        _data = view.Data;
        _count = view.Data == 0 ? 0 : checked((int)view.Count);
        _stride = checked((int)view.Stride);
    }

    // A view of an owned NativeHandleArray's storage
    internal NativeHandleSpan(nint data, int count, int stride)
    {
        _data = data;
        _count = data == 0 ? 0 : count;
        _stride = stride;
    }

    /// <summary>The number of elements the container holds.</summary>
    public int Count => _count;

    /// <summary>Whether the container is empty (or the view was never filled).</summary>
    public bool IsEmpty => _count == 0;

    /// <summary>
    /// A borrowed handle onto element <paramref name="index"/> of the container's own storage. Writing
    /// through it edits the container; never Dispose it - the handle addresses an element inside the
    /// container's allocation, so deleting it corrupts the heap.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The index is outside the container.</exception>
    public T this[int index]
    {
        get
        {
            if ((uint)index >= (uint)_count)
                throw new ArgumentOutOfRangeException(nameof(index), index, $"Expected an index below {_count}.");
            // Every handle struct is a single nint, so the bit cast is the whole conversion
            return Unsafe.BitCast<nint, T>(_data + (nint)index * _stride);
        }
    }

    /// <summary>Walks the elements in order, yielding the same borrowed handles as <see cref="this[int]"/>.</summary>
    public Enumerator GetEnumerator() => new(this);

    /// <summary>
    /// Enumerates a <see cref="NativeHandleSpan{T}"/>. Each <see cref="Current"/> is the same borrowed
    /// handle the indexer gives: never Dispose one, so never <c>foreach</c> with a <c>using</c> body.
    /// </summary>
    public ref struct Enumerator
    {
        private readonly NativeHandleSpan<T> _view;
        private int _index;

        internal Enumerator(NativeHandleSpan<T> view)
        {
            _view = view;
            _index = -1;
        }

        /// <summary>The borrowed handle at the current position; never Dispose it.</summary>
        public readonly T Current => _view[_index];

        /// <summary>Advances to the next element; false at the end.</summary>
        public bool MoveNext() => ++_index < _view.Count;
    }
}
