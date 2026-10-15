using System.Numerics;

namespace Brutal.VulkanApi.Abstractions;

/// <summary>
/// 2-D power-of-two square quad-tree partitioner for a dynamic texture atlas.
///
/// The atlas is a square of <see cref="TotalSize"/>; slots are power-of-two squares in
/// <c>[MinElementSize, MaxElementSize]</c>. Compared to <see cref="Atlas2DPartitioner"/>'s row
/// buckets, the quad tree is very fragmentation-resistant: empty sibling groups are merged back
/// into their parent on <see cref="Cleanup"/> (which <see cref="Alloc"/> retries through
/// automatically), so space freed by mixed-size allocations coalesces into larger slots again.
///
/// Two APIs are available:
/// <list type="bullet">
///     <item>The <see cref="IRecycleImageViewPartitioner"/> contract
///     (<see cref="TryCreatePartition"/> / <see cref="FreePartition"/>), interchangeable with the
///     other image partitioners.</item>
///     <item>A direct slot API (<see cref="Alloc"/> / <see cref="Free"/> / <see cref="GetRect"/>)
///     returning an opaque slot id that is stable for the slot's lifetime - query
///     <see cref="GetRect"/> each frame rather than caching the rect, so a future atlas
///     resize/squash can relocate slots transparently. When <see cref="Alloc"/> returns -1 the
///     documented fallback is to retry at half size; cascades simply request the largest size and
///     take what they get.</item>
/// </list>
///
/// Nodes are only ever created (root, then groups of four on subdivide) and freed (groups of four
/// on merge), so the four children of a branch are always contiguous in the node array - child
/// <c>j</c> of a branch is <c>firstChild + j</c>.
/// </summary>
public class QuadTreePartitioner : IRecycleImageViewPartitioner, ISize2D
{
    /// <summary>
    /// Pixel rectangle of an allocated atlas slot, relative to the backing partition's offset.
    /// Slots are always square.
    /// </summary>
    public readonly record struct Rect(int X, int Y, int Size);

    private struct Node
    {
        /// <summary>Branch: index of first child node. Empty leaf: -1. Full leaf: element index.</summary>
        public int FirstChild;

        /// <summary>True only for a full leaf (one element occupies this node). Branches and empty leaves are false.</summary>
        public bool IsFull;
    }

    private struct Element
    {
        public int X;
        public int Y;
        public int Size;
        public int NodeIndex;

        /// <summary>
        /// Bumped every time this slot is freed (and on <see cref="Reset"/>), so an id issued for an
        /// earlier occupant of the slot no longer matches. Survives slot reuse: InsertElement
        /// carries it over rather than resetting it.
        /// </summary>
        public int Generation;
    }

    private readonly record struct NodeRef(int Index, int Depth, int X, int Y, int Size);

    // Public ids pack a generation above the element index so a stale id cannot alias a later
    // occupant of the same slot: id = (generation << INDEX_BITS) | index. 20 + 11 bits keeps the
    // id non-negative (-1 stays the failure sentinel); the generation wraps after 2048 reuses of
    // one slot, which is the (documented) limit of the protection.
    private const int INDEX_BITS = 20;
    private const int INDEX_MASK = (1 << INDEX_BITS) - 1;
    private const int GENERATION_MASK = 0x7FF;

    private readonly List<Node> _nodes = [];
    private readonly Stack<int> _nodeFree = new(); // LIFO - preserves contiguous groups of four

    // _elements, _elemFree and _elementsByPosition all hold element INDICES; public ids are made with MakeId
    private readonly List<Element> _elements = [];
    private readonly Stack<int> _elemFree = new();
    private readonly Dictionary<(int X, int Y), int> _elementsByPosition = new();

    // Reusable traversal scratch, cleared before each use
    private readonly Stack<int> _scratchIndices = new();
    private readonly Stack<NodeRef> _scratchRefs = new();

    private readonly int _requestedMaxElementSize;
    private readonly int _requestedMinElementSize;

    private int _totalDepth;
    private int _minDepth; // shallowest allocatable depth (= largest slot, MaxElementSize)
    private int _maxDepth; // deepest allocatable depth  (= smallest slot, MinElementSize)
    private int _allocationCount;

    public ImageViewPartitionInfo BackingPartition { get; set; }

    /// <summary>Atlas edge length in pixels (square, power of two).</summary>
    public int TotalSize { get; private set; }

    /// <summary>Largest allocatable slot edge (power of two).</summary>
    public int MaxElementSize { get; private set; }

    /// <summary>Smallest allocatable slot edge (power of two).</summary>
    public int MinElementSize { get; private set; }

    /// <summary>Number of live allocations.</summary>
    public int Count => _allocationCount;

    /// <inheritdoc />
    public int Width => TotalSize;

    /// <inheritdoc />
    public int Height => TotalSize;

    /// <summary>
    /// Construct a quad-tree partitioner over the backing partition, whose extent must be a
    /// square power of two with a depth of one.
    /// </summary>
    /// <param name="backingPartition">Backing image partition to slice up</param>
    /// <param name="maxElementSize">Largest allocatable slot edge (power of two); 0 means the full atlas size</param>
    /// <param name="minElementSize">Smallest allocatable slot edge (power of two)</param>
    public QuadTreePartitioner(ImageViewPartitionInfo backingPartition, int maxElementSize = 0, int minElementSize = 1)
    {
        _requestedMaxElementSize = maxElementSize;
        _requestedMinElementSize = minElementSize;
        BackingPartition = backingPartition;
        Initialize(backingPartition);
    }

    /// <inheritdoc />
    public void Reset(ImageViewPartitionInfo info)
    {
        BackingPartition = info;
        Initialize(info);
    }

    private void Initialize(ImageViewPartitionInfo backingPartition)
    {
        var extent = backingPartition.Extent;
        if (extent.Depth != 1)
            throw new ArgumentException("Backing partition depth must be 1");
        if (extent.Width != extent.Height)
            throw new ArgumentException("Backing partition must be square");
        if (!BitOperations.IsPow2((uint)extent.Width))
            throw new ArgumentException("Atlas size must be a power of two");

        var totalSize = extent.Width;
        var maxElementSize = _requestedMaxElementSize == 0 ? totalSize : _requestedMaxElementSize;
        var minElementSize = _requestedMinElementSize;

        if (!BitOperations.IsPow2((uint)maxElementSize) || !BitOperations.IsPow2((uint)minElementSize))
            throw new ArgumentException("Atlas element sizes must be powers of two");
        if (totalSize < maxElementSize || maxElementSize < minElementSize || minElementSize < 1)
            throw new ArgumentException("Bad / mismatched atlas element sizes");

        TotalSize = totalSize;
        MaxElementSize = maxElementSize;
        MinElementSize = minElementSize;

        _totalDepth = BitOperations.Log2((uint)totalSize);
        _minDepth = _totalDepth - BitOperations.Log2((uint)maxElementSize);
        _maxDepth = _totalDepth - BitOperations.Log2((uint)minElementSize);

        _nodes.Clear();
        _nodeFree.Clear();
        InvalidateAllElements();
        _elementsByPosition.Clear();
        _allocationCount = 0;

        // Root: an empty, unsubdivided leaf covering the whole atlas
        _nodes.Add(new Node { FirstChild = -1, IsFull = false });
    }

    /// <inheritdoc />
    /// <remarks>Requests whose larger edge exceeds <see cref="MaxElementSize"/> fail rather than clamp.</remarks>
    public bool TryCreatePartition(
        in ImageViewPartitionInfo.CreateInfo createInfo,
        out ImageViewPartitionInfo partition
    )
    {
        if (createInfo.Extent.Depth != 1)
            throw new ArgumentException("Depth must be 1");
        if (createInfo.LayerCount != 1)
            throw new ArgumentException("LayerCount must be 1");

        var size = Math.Max(createInfo.Extent.Width, createInfo.Extent.Height);

        // MaxElementSize is a power of two, so the rounded-up request exceeds it exactly when the raw one does
        if (size > MaxElementSize)
        {
            partition = default;
            return false;
        }

        var id = Alloc(size);
        if (id < 0)
        {
            partition = default;
            return false;
        }

        var rect = GetRect(id);

        partition = BackingPartition with
        {
            Partitioner = this,
            Extent = createInfo.Extent,
            Offset = new VkOffset3D
            {
                X = BackingPartition.Offset.X + rect.X,
                Y = BackingPartition.Offset.Y + rect.Y,
                Z = BackingPartition.Offset.Z,
            },
            ArrayCount = 1,
            ArrayOffset = BackingPartition.ArrayOffset,
        };

        return true;
    }

    /// <inheritdoc />
    public void FreePartition(in ImageViewPartitionInfo inAllocation)
    {
        var position = (
            inAllocation.Offset.X - BackingPartition.Offset.X,
            inAllocation.Offset.Y - BackingPartition.Offset.Y
        );

        if (!_elementsByPosition.TryGetValue(position, out var index))
            throw new ArgumentException("Partition offset does not match a live slot of this partitioner");

        FreeIndex(index);
    }

    /// <summary>
    /// Allocate a slot for the requested edge size (rounded up to a power of two and clamped to
    /// <c>[MinElementSize, MaxElementSize]</c>). Returns the slot id, or -1 if the atlas can't fit
    /// it even after a cleanup pass.
    /// </summary>
    public int Alloc(int size)
    {
        var targetDepth = _totalDepth - BitOperations.Log2(BitOperations.RoundUpToPowerOf2((uint)Math.Max(1, size)));
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
            cand = new NodeRef(fc, cand.Depth + 1, cand.X + half, cand.Y + half, half);
        }

        var elem = InsertElement(
            new Element
            {
                X = cand.X,
                Y = cand.Y,
                Size = cand.Size,
                NodeIndex = cand.Index,
            }
        );
        var node = _nodes[cand.Index];
        node.IsFull = true;
        node.FirstChild = elem;
        _nodes[cand.Index] = node;
        _elementsByPosition[(cand.X, cand.Y)] = elem;
        _allocationCount++;
        return MakeId(elem, _elements[elem].Generation);
    }

    /// <summary>
    /// Release a slot. Its node becomes an empty leaf, reclaimable by <see cref="Cleanup"/>.
    /// Returns false for an invalid, already-freed, or stale id: ids carry a generation that is
    /// bumped when their slot is freed or the partitioner is <see cref="Reset"/>, so an old id
    /// cannot release a later slot that reused it (the generation wraps after 2048 reuses of one
    /// slot, which is the limit of the protection).
    /// </summary>
    public bool Free(int id)
    {
        if (!TryResolve(id, out var index))
            return false;

        FreeIndex(index);
        return true;
    }

    /// <summary>Pixel rectangle of an allocated slot. Returns a zero rect for an invalid or stale id.</summary>
    public Rect GetRect(int id)
    {
        if (!TryResolve(id, out var index))
            return default;
        var e = _elements[index];
        return new Rect(e.X, e.Y, e.Size);
    }

    /// <summary>Slot edge size for an allocated id, or 0 for an invalid or stale id.</summary>
    public int GetSize(int id) => TryResolve(id, out var index) ? _elements[index].Size : 0;

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

        _elementsByPosition.Remove((element.X, element.Y));
        element.Generation = (element.Generation + 1) & GENERATION_MASK;
        _elements[index] = element;
        _elemFree.Push(index);
        _allocationCount--;
    }

    /// <summary>
    /// Mark every element slot free without discarding it, bumping each generation so ids issued
    /// before a <see cref="Reset"/> are rejected afterwards instead of aliasing new slots.
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
    /// Merge empty sibling groups back into their parent, repeating until nothing more merges, so
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

    /// <summary>One top-down merge pass; returns true if any group was merged.</summary>
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

            for (var j = 0; j < 4; j++)
            {
                var child = _nodes[fc + j];
                if (child.IsFull)
                    continue; // occupied leaf - never empty
                if (child.FirstChild == -1)
                    emptyLeaves++; // empty leaf
                else
                    toProcess.Push(fc + j); // branch - descend
            }

            if (emptyLeaves == 4)
            {
                // Free the four children in reverse so the LIFO free list keeps them as a contiguous
                // group reclaimable, in order, by the next subdivide
                for (var j = 3; j >= 0; j--)
                    _nodeFree.Push(fc + j);
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
        best = new NodeRef(-1, 0, 0, 0, 0); // Index == -1 ⇒ nothing found

        var root = _nodes[0];
        if (root.IsFull)
            return false;

        var rootRef = new NodeRef(0, 0, 0, 0, TotalSize);
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
            var childSize = np.Size / 2; // also the child min-corner offset within the parent

            for (var j = 0; j < 4; j++)
            {
                // Child min corners, in corner order (max,max),(min,max),(max,min),(min,min)
                var cx = np.X + (j == 0 || j == 2 ? childSize : 0);
                var cy = np.Y + (j == 0 || j == 1 ? childSize : 0);

                var childIdx = fc + j;
                var child = _nodes[childIdx];
                if (child.IsFull)
                    continue;

                var childRef = new NodeRef(childIdx, childDepth, cx, cy, childSize);
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

    /// <summary>Turn a leaf into a branch with four contiguous empty-leaf children; returns the first child index.</summary>
    private int SubdivideNode(int nodeIndex)
    {
        var fc = InsertNode(new Node { FirstChild = -1, IsFull = false });
        for (var i = 1; i < 4; i++)
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
            throw new InvalidOperationException("Too many live slots for the id encoding");
        element.Generation = 0;
        _elements.Add(element);
        return _elements.Count - 1;
    }
}
