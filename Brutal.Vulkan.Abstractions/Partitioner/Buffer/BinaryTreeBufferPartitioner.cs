using System.Numerics;

namespace Brutal.VulkanApi.Abstractions;

/// <summary>
/// 1-D power-of-two binary-tree (buddy) partitioner for a buffer - the buffer analog of
/// <see cref="QuadTreePartitioner"/>. Partitions are power-of-two block sizes in
/// <c>[MinElementSize, MaxElementSize]</c>, and every block's offset is naturally aligned to its
/// own size (relative to the start of the backing partition).
///
/// Unlike <see cref="LinearBufferPartitioner"/>, partitions can be freed individually
/// (<see cref="IRecycleBufferPartitioner"/>): empty sibling pairs are merged back into their
/// parent on <see cref="Cleanup"/> (which <see cref="Alloc"/> retries through automatically), so
/// freed space coalesces into larger blocks again instead of fragmenting.
///
/// Nodes are only ever created (root, then pairs on subdivide) and freed (pairs on merge), so the
/// two children of a branch are always contiguous in the node array - child <c>j</c> of a branch
/// is <c>firstChild + j</c>.
/// </summary>
public class BinaryTreeBufferPartitioner : IRecycleBufferPartitioner, ISize
{
    /// <summary>
    /// Byte range of an allocated block, relative to the start of the backing partition.
    /// </summary>
    public readonly record struct Range(ByteSize Offset, ByteSize Size);

    private struct Node
    {
        /// <summary>Branch: index of first child node. Empty leaf: -1. Full leaf: element index.</summary>
        public int FirstChild;

        /// <summary>True only for a full leaf (one element occupies this node). Branches and empty leaves are false.</summary>
        public bool IsFull;
    }

    private struct Element
    {
        public ulong Offset;
        public ulong Size;
        public int NodeIndex;

        /// <summary>
        /// Bumped every time this slot is freed (and on <see cref="Reset"/>), so an id issued for an
        /// earlier occupant of the slot no longer matches. Survives slot reuse: InsertElement
        /// carries it over rather than resetting it.
        /// </summary>
        public int Generation;
    }

    private readonly record struct NodeRef(int Index, int Depth, ulong Offset, ulong Size);

    // Public ids pack a generation above the element index so a stale id cannot alias a later
    // occupant of the same slot: id = (generation << INDEX_BITS) | index. 20 + 11 bits keeps the
    // id non-negative (-1 stays the failure sentinel); the generation wraps after 2048 reuses of
    // one slot, which is the (documented) limit of the protection.
    private const int INDEX_BITS = 20;
    private const int INDEX_MASK = (1 << INDEX_BITS) - 1;
    private const int GENERATION_MASK = 0x7FF;

    private readonly List<Node> _nodes = [];
    private readonly Stack<int> _nodeFree = new(); // LIFO - preserves contiguous pairs

    // _elements, _elemFree and _elementsByOffset all hold element INDICES; public ids are made with MakeId
    private readonly List<Element> _elements = [];
    private readonly Stack<int> _elemFree = new();
    private readonly Dictionary<ulong, int> _elementsByOffset = new();

    // Reusable traversal scratch, cleared before each use
    private readonly Stack<int> _scratchIndices = new();
    private readonly Stack<NodeRef> _scratchRefs = new();

    private readonly ByteSize _requestedMaxElementSize;
    private readonly ByteSize _requestedMinElementSize;

    private int _totalDepth;
    private int _minDepth; // shallowest allocatable depth (= largest block, MaxElementSize)
    private int _maxDepth; // deepest allocatable depth  (= smallest block, MinElementSize)
    private int _allocationCount;
    private ulong _allocatedBytes;

    public BufferPartitionInfo BackingPartition { get; set; }

    /// <summary>Total capacity in bytes (power of two).</summary>
    public ByteSize Capacity { get; private set; }

    /// <summary>Largest allocatable block size (power of two).</summary>
    public ByteSize MaxElementSize { get; private set; }

    /// <summary>Smallest allocatable block size (power of two) - also the minimum block alignment.</summary>
    public ByteSize MinElementSize { get; private set; }

    /// <summary>Number of live allocations.</summary>
    public int Count => _allocationCount;

    /// <summary>
    /// Bytes not held by live blocks. Fragmentation may still prevent a single allocation of this
    /// size; <see cref="Cleanup"/> (retried automatically inside <see cref="Alloc"/>) coalesces
    /// what it can.
    /// </summary>
    public ByteSize FreeSpace => ByteSize.Of((nuint)((ulong)(nuint)Capacity - _allocatedBytes));

    /// <inheritdoc />
    int ISize.Size => (int)Capacity;

    /// <summary>
    /// Construct a binary-tree partitioner over the backing partition, whose size must be a power
    /// of two. Note: block offsets are aligned relative to the backing partition's start; for
    /// absolute (bind-time) alignment guarantees the backing partition itself must start at a
    /// suitably aligned offset.
    /// </summary>
    /// <param name="backingPartition">Backing buffer partition to slice up</param>
    /// <param name="maxElementSize">Largest allocatable block (power of two); default means the full capacity</param>
    /// <param name="minElementSize">Smallest allocatable block (power of two); default means one byte</param>
    public BinaryTreeBufferPartitioner(
        BufferPartitionInfo backingPartition,
        ByteSize maxElementSize = default,
        ByteSize minElementSize = default
    )
    {
        _requestedMaxElementSize = maxElementSize;
        _requestedMinElementSize = minElementSize;
        BackingPartition = backingPartition;
        Initialize(backingPartition);
    }

    /// <inheritdoc />
    public void Reset(BufferPartitionInfo info)
    {
        BackingPartition = info;
        Initialize(info);
    }

    private void Initialize(BufferPartitionInfo backingPartition)
    {
        var capacity = (ulong)(nuint)backingPartition.PartitionSize;
        if (!BitOperations.IsPow2(capacity))
            throw new ArgumentException("Backing partition size must be a power of two");

        var maxElementSize = _requestedMaxElementSize == 0 ? capacity : (ulong)(nuint)_requestedMaxElementSize;
        var minElementSize = _requestedMinElementSize == 0 ? 1UL : (ulong)(nuint)_requestedMinElementSize;

        if (!BitOperations.IsPow2(maxElementSize) || !BitOperations.IsPow2(minElementSize))
            throw new ArgumentException("Element sizes must be powers of two");
        if (capacity < maxElementSize || maxElementSize < minElementSize || minElementSize < 1)
            throw new ArgumentException("Bad / mismatched element sizes");

        Capacity = ByteSize.Of((nuint)capacity);
        MaxElementSize = ByteSize.Of((nuint)maxElementSize);
        MinElementSize = ByteSize.Of((nuint)minElementSize);

        _totalDepth = BitOperations.Log2(capacity);
        _minDepth = _totalDepth - BitOperations.Log2(maxElementSize);
        _maxDepth = _totalDepth - BitOperations.Log2(minElementSize);

        _nodes.Clear();
        _nodeFree.Clear();
        InvalidateAllElements();
        _elementsByOffset.Clear();
        _allocationCount = 0;
        _allocatedBytes = 0;

        // Root: an empty, unsubdivided leaf covering the whole range
        _nodes.Add(new Node { FirstChild = -1, IsFull = false });
    }

    /// <inheritdoc />
    /// <remarks>Requests larger than <see cref="MaxElementSize"/> fail rather than clamp.</remarks>
    public bool TryCreatePartition(BufferPartitionInfo.CreateInfo createInfo, out BufferPartitionInfo partition)
    {
        // MaxElementSize is a power of two, so the rounded-up request exceeds it exactly when the raw one does
        if ((ulong)(nuint)createInfo.Size > (ulong)(nuint)MaxElementSize)
        {
            partition = default;
            return false;
        }

        var id = Alloc(createInfo.Size);
        if (id < 0)
        {
            partition = default;
            return false;
        }

        var range = GetRange(id);

        partition = new BufferPartitionInfo(
            Partitioner: this,
            BufferInfo: BackingPartition.BufferInfo,
            PartitionOffset: BackingPartition.PartitionOffset + range.Offset,
            PartitionSize: range.Size
        );

        return true;
    }

    /// <inheritdoc />
    public void FreePartition(in BufferPartitionInfo inAllocation)
    {
        var localOffset = (ulong)(nuint)(inAllocation.PartitionOffset - BackingPartition.PartitionOffset);

        if (!_elementsByOffset.TryGetValue(localOffset, out var index))
            throw new ArgumentException("Partition offset does not match a live block of this partitioner");

        FreeIndex(index);
    }

    /// <summary>
    /// Allocate a block for the requested size (rounded up to a power of two and clamped to
    /// <c>[MinElementSize, MaxElementSize]</c>). Returns the block id, or -1 if the range can't
    /// fit it even after a cleanup pass.
    /// </summary>
    public int Alloc(ByteSize size)
    {
        var rounded = BitOperations.RoundUpToPowerOf2(Math.Max(1UL, (ulong)(nuint)size));
        var targetDepth = _totalDepth - BitOperations.Log2(rounded);
        if (targetDepth < _minDepth)
            targetDepth = _minDepth;
        else if (targetDepth > _maxDepth)
            targetDepth = _maxDepth;

        if (!TryFindSlot(targetDepth, out var cand))
        {
            Cleanup();
            if (!TryFindSlot(targetDepth, out cand))
                return -1;
        }

        // Subdivide the candidate down to the target depth, always descending into the first child
        while (cand.Depth < targetDepth)
        {
            var fc = SubdivideNode(cand.Index);
            var half = cand.Size / 2;
            cand = new NodeRef(fc, cand.Depth + 1, cand.Offset + half, half);
        }

        var elem = InsertElement(
            new Element
            {
                Offset = cand.Offset,
                Size = cand.Size,
                NodeIndex = cand.Index,
            }
        );
        var node = _nodes[cand.Index];
        node.IsFull = true;
        node.FirstChild = elem;
        _nodes[cand.Index] = node;
        _elementsByOffset[cand.Offset] = elem;
        _allocationCount++;
        _allocatedBytes += cand.Size;
        return MakeId(elem, _elements[elem].Generation);
    }

    /// <summary>
    /// Release a block. Its node becomes an empty leaf, reclaimable by <see cref="Cleanup"/>.
    /// Returns false for an invalid, already-freed, or stale id: ids carry a generation that is
    /// bumped when their slot is freed or the partitioner is <see cref="Reset"/>, so an old id
    /// cannot release a later block that reused the slot (the generation wraps after 2048 reuses
    /// of one slot, which is the limit of the protection).
    /// </summary>
    public bool Free(int id)
    {
        if (!TryResolve(id, out var index))
            return false;

        FreeIndex(index);
        return true;
    }

    /// <summary>Byte range of an allocated block. Returns a zero range for an invalid or stale id.</summary>
    public Range GetRange(int id)
    {
        if (!TryResolve(id, out var index))
            return default;
        var e = _elements[index];
        return new Range(ByteSize.Of((nuint)e.Offset), ByteSize.Of((nuint)e.Size));
    }

    private static int MakeId(int index, int generation) => (generation << INDEX_BITS) | index;

    /// <summary>
    /// Map a public id to a live element index: the index must be in range, the id's generation
    /// must match the slot's, and the slot's node must still be a full leaf pointing back at it.
    /// </summary>
    private bool TryResolve(int id, out int index)
    {
        index = id & INDEX_MASK;
        if (id < 0 || index >= _elements.Count)
            return false;

        var element = _elements[index];
        if (element.Generation != (id >> INDEX_BITS))
            return false;

        if ((uint)element.NodeIndex >= (uint)_nodes.Count)
            return false;

        var node = _nodes[element.NodeIndex];
        return node.IsFull && node.FirstChild == index;
    }

    /// <summary>Release a live element by index, bumping its generation so outstanding ids for it go stale.</summary>
    private void FreeIndex(int index)
    {
        var element = _elements[index];
        var node = _nodes[element.NodeIndex];

        node.IsFull = false;
        node.FirstChild = -1;
        _nodes[element.NodeIndex] = node;

        _elementsByOffset.Remove(element.Offset);
        element.Generation = (element.Generation + 1) & GENERATION_MASK;
        _elements[index] = element;
        _elemFree.Push(index);
        _allocationCount--;
        _allocatedBytes -= element.Size;
    }

    /// <summary>
    /// Mark every element slot free without discarding it, bumping each generation so ids issued
    /// before a <see cref="Reset"/> are rejected afterwards instead of aliasing new blocks.
    /// </summary>
    private void InvalidateAllElements()
    {
        _elemFree.Clear();
        for (var i = _elements.Count - 1; i >= 0; i--)
        {
            var element = _elements[i];
            element.Generation = (element.Generation + 1) & GENERATION_MASK;
            _elements[i] = element;
            _elemFree.Push(i);
        }
    }

    /// <summary>
    /// Merge empty sibling pairs back into their parent, repeating until nothing more merges, so
    /// freed space coalesces all the way up the tree (<see cref="Alloc"/> runs this before giving
    /// up). Cheap; safe to call any time.
    /// </summary>
    public void Cleanup()
    {
        // Each pass visits parents before children, so a merge at depth d only exposes its parent
        // at depth d-1 to the next pass. Looping until a pass merges nothing is bounded by the depth.
        bool merged;
        do
        {
            merged = CleanupPass();
        } while (merged);
    }

    /// <summary>One top-down merge pass; returns true if any pair was merged.</summary>
    private bool CleanupPass()
    {
        // Root is a full leaf or an empty unsubdivided leaf → nothing to merge
        if (_nodes[0].IsFull || _nodes[0].FirstChild < 0)
            return false;

        var merged = false;
        var toProcess = _scratchIndices;
        toProcess.Clear();
        toProcess.Push(0);

        while (toProcess.Count > 0)
        {
            var idx = toProcess.Pop();
            var fc = _nodes[idx].FirstChild;
            var emptyLeaves = 0;

            for (var j = 0; j < 2; j++)
            {
                var child = _nodes[fc + j];
                if (child.IsFull)
                    continue; // occupied leaf - never empty
                if (child.FirstChild == -1)
                    emptyLeaves++; // empty leaf
                else
                    toProcess.Push(fc + j); // branch - descend
            }

            if (emptyLeaves == 2)
            {
                // Free the pair in reverse so the LIFO free list keeps it contiguous, in order,
                // reclaimable by the next subdivide
                _nodeFree.Push(fc + 1);
                _nodeFree.Push(fc);
                var node = _nodes[idx];
                node.FirstChild = -1;
                node.IsFull = false;
                _nodes[idx] = node;
                merged = true;
            }
        }

        return merged;
    }

    /// <summary>
    /// Find an empty leaf at <paramref name="targetDepth"/> (perfect fit), or the deepest empty leaf
    /// shallower than it (which <see cref="Alloc"/> subdivides down). Returns false if none exists.
    /// </summary>
    private bool TryFindSlot(int targetDepth, out NodeRef best)
    {
        best = new NodeRef(-1, 0, 0, 0); // Index == -1 ⇒ nothing found

        var root = _nodes[0];
        if (root.IsFull)
            return false;

        var rootRef = new NodeRef(0, 0, 0, (ulong)(nuint)Capacity);
        if (root.FirstChild == -1)
        {
            // Empty unsubdivided root - usable for any target (subdivided down by the caller)
            best = rootRef;
            return true;
        }

        var toProcess = _scratchRefs;
        toProcess.Clear();
        toProcess.Push(rootRef);

        while (toProcess.Count > 0)
        {
            var np = toProcess.Pop();
            if (np.Depth >= targetDepth)
                continue;

            var childDepth = np.Depth + 1;
            var fc = _nodes[np.Index].FirstChild;
            var childSize = np.Size / 2;

            for (var j = 0; j < 2; j++)
            {
                // Child offsets in order (high half),(low half), matching the subdivide descent
                var offset = np.Offset + (j == 0 ? childSize : 0);

                var childIdx = fc + j;
                var child = _nodes[childIdx];
                if (child.IsFull)
                    continue;

                var childRef = new NodeRef(childIdx, childDepth, offset, childSize);
                if (child.FirstChild == -1) // empty leaf
                {
                    if (childDepth == targetDepth)
                    {
                        best = childRef; // perfect fit
                        return true;
                    }
                    if (childDepth > best.Depth)
                        best = childRef; // deepest subdividable candidate so far
                }
                else
                {
                    toProcess.Push(childRef); // branch - descend
                }
            }
        }

        return best.Index != -1;
    }

    /// <summary>Turn a leaf into a branch with two contiguous empty-leaf children; returns the first child index.</summary>
    private int SubdivideNode(int nodeIndex)
    {
        var fc = InsertNode(new Node { FirstChild = -1, IsFull = false });
        InsertNode(new Node { FirstChild = -1, IsFull = false });

        var node = _nodes[nodeIndex];
        node.FirstChild = fc;
        node.IsFull = false;
        _nodes[nodeIndex] = node;
        return fc;
    }

    private int InsertNode(Node node)
    {
        if (_nodeFree.Count > 0)
        {
            var i = _nodeFree.Pop();
            _nodes[i] = node;
            return i;
        }
        _nodes.Add(node);
        return _nodes.Count - 1;
    }

    private int InsertElement(Element element)
    {
        if (_elemFree.Count > 0)
        {
            var i = _elemFree.Pop();
            // Keep the slot's generation: it was bumped when the previous occupant was freed
            element.Generation = _elements[i].Generation;
            _elements[i] = element;
            return i;
        }
        if (_elements.Count > INDEX_MASK)
            throw new InvalidOperationException("Too many live blocks for the id encoding");
        element.Generation = 0;
        _elements.Add(element);
        return _elements.Count - 1;
    }
}
