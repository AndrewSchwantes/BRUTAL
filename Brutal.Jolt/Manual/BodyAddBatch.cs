namespace Brutal.JoltApi;

/// <summary>
/// A batch of created bodies being added to the physics system in one go, from
/// <see cref="BodyInterface.PrepareAddBodies"/>. <see cref="Commit"/> adds them; <see cref="Dispose"/> frees the
/// batch, and aborts the add first if it was never committed. Dispose it either way.
/// </summary>
/// <remarks>
/// Adding bodies in a batch builds their broad-phase nodes once for the whole set, which is much cheaper than one
/// <see cref="BodyInterface.AddBody"/> per body, and the prepare step can run on a background thread. The batch
/// keeps its own copy of the IDs, in the order Jolt prepared them. It holds on to the body interface it came from,
/// so dispose it before the physics system goes; it has no finalizer for that reason. <see cref="Commit"/> and
/// <see cref="Dispose"/> are safe to call from different threads: whichever runs second sees the first's result.
/// </remarks>
public sealed class BodyAddBatch : IDisposable, INullable
{
    // Commit and Dispose both reach the native batch, so they take turns: Dispose can't free it mid-commit
    private readonly Lock _gate = new();
    private nint _batch;
    private bool _committed;

    internal BodyAddBatch(nint batch) => _batch = batch;

    /// <summary>Whether this is the empty (already disposed) batch.</summary>
    public bool IsNull() => _batch == 0;

    /// <summary>Whether <see cref="Commit"/> has run; until then, <see cref="Dispose"/> aborts the add.</summary>
    public bool IsCommitted => _committed;

    /// <summary>
    /// Adds every body in the batch to the physics system, under one lock. Committing again does nothing.
    /// </summary>
    /// <param name="activation">Whether the bodies start active.</param>
    /// <exception cref="ObjectDisposedException">The batch has been disposed.</exception>
    public void Commit(Activation activation = Activation.Activate)
    {
        lock (_gate)
        {
            ObjectDisposedException.ThrowIf(_batch == 0, this);
            if (_committed)
                return;
            Jolt.PInvoke.BodyAddBatchCommit(_batch, activation);
            _committed = true;
        }
    }

    /// <summary>Frees the batch, aborting the add if it was never committed. Idempotent.</summary>
    public void Dispose()
    {
        lock (_gate)
        {
            if (_batch == 0)
                return;
            Jolt.PInvoke.BodyAddBatchDestroy(_batch);
            _batch = 0;
        }
    }
}
