namespace Brutal.JoltApi;

// Hand-written wrapper for BodyInterface.PrepareAddBodies (the batch add API from JoltBodies.h).
public readonly partial struct BodyInterface
{
    /// <summary>
    ///   <para>
    ///     Starts adding many created bodies to the physics system at once, which is much cheaper than adding them
    ///     one at a time. The IDs are copied, so bodyIDs can be reused straight away. Preparing touches nothing the
    ///     simulation reads, so it can run on a background thread; BodyAddBatch.Commit then adds them all under one
    ///     lock. Bodies must have been created (CreateBody) and not added yet.
    ///   </para>
    /// </summary>
    /// <returns>The returned batch is yours: Commit it, then Dispose it (Dispose alone aborts the add).</returns>
    public unsafe BodyAddBatch PrepareAddBodies(ReadOnlySpan<uint> bodyIDs)
    {
        fixed (uint* bodyIDs_fixed = bodyIDs)
        {
            return new BodyAddBatch(
                Jolt.PInvoke.BodyInterface_PrepareAddBodies(this, bodyIDs_fixed, (nuint)bodyIDs.Length)
            );
        }
    }
}
