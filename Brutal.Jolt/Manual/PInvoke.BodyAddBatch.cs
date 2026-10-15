using System.Runtime.InteropServices;

namespace Brutal.JoltApi;

// Hand-written LibraryImports for the batch body add (JoltBodyAddBatchShim.cpp).
// PrepareAddBodies is on BodyInterface; Commit and Destroy are free functions.
public static partial class Jolt
{
    public static unsafe partial class PInvoke
    {
        /// <summary>
        ///   <para>
        ///     Starts adding many created bodies to the physics system at once, which is much cheaper than adding them
        ///     one at a time. The IDs are copied, so bodyIDs can be reused straight away. Preparing touches nothing the
        ///     simulation reads, so it can run on a background thread; BodyAddBatch.Commit then adds them all under one
        ///     lock. Bodies must have been created (CreateBody) and not added yet.
        ///   </para>
        /// </summary>
        [LibraryImport("jolt", EntryPoint = "BodyInterface_PrepareAddBodies")]
        public static partial nint BodyInterface_PrepareAddBodies(
            BodyInterface self,
            uint* bodyIDs,
            nuint bodyIDsCount
        );

        /// <summary>
        ///   <para>
        ///     Finalizes a prepared batch add; a batch already committed is left alone.
        ///   </para>
        /// </summary>
        [LibraryImport("jolt", EntryPoint = "Jolt_BodyAddBatchCommit")]
        public static partial void BodyAddBatchCommit(nint batch, Activation activation);

        /// <summary>
        ///   <para>
        ///     Frees a batch add, aborting it first when it was never committed.
        ///   </para>
        /// </summary>
        [LibraryImport("jolt", EntryPoint = "Jolt_BodyAddBatchDestroy")]
        public static partial void BodyAddBatchDestroy(nint batch);
    }
}
