namespace Brutal.JoltApi;

/// <summary>
/// A contact listener that records contacts natively for you to <see cref="Drain"/> after
/// <see cref="PhysicsSystem.Update"/>, instead of calling into C# for every contact.
/// </summary>
/// <remarks>
/// <para>
/// Observe-only: it can't reject a contact or change its <see cref="ContactSettings"/>. Use a
/// <see cref="ManagedContactListener"/> for that. What it saves is the managed callback per contact, which Jolt makes
/// from its worker threads while holding body locks, so it suits audio, damage, sensors and gameplay triggers.
/// </para>
/// <para>
/// Install <see cref="Handle"/> with <see cref="PhysicsSystem.SetContactListener"/>. The buffer holds
/// <see cref="Capacity"/> events; ones past that are counted in <see cref="Dropped"/> and lost, so size it for your
/// busiest step or drain every step. Drain, <see cref="Clear"/> and <see cref="Pending"/> must only be called between
/// updates, never while one is running. Take the listener off the system before disposing it; it has no finalizer
/// for that reason.
/// </para>
/// </remarks>
public sealed unsafe class ContactEventListener : IDisposable, INullable
{
    private nint _listener;

    /// <summary>Creates a listener buffering up to <paramref name="capacity"/> events of the kinds in
    /// <paramref name="mask"/>.</summary>
    /// <param name="capacity">How many events the buffer holds between drains.</param>
    /// <param name="mask">Which kinds to record. Leaving out <see cref="ContactEventMask.Persisted"/> keeps only
    /// the starts and ends of contacts, which is usually what gameplay wants.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is zero.</exception>
    public ContactEventListener(uint capacity, ContactEventMask mask = ContactEventMask.AddedAndRemoved)
    {
        ArgumentOutOfRangeException.ThrowIfZero(capacity);
        _listener = Jolt.PInvoke.ContactEventListenerCreate(capacity, (uint)mask);
        Handle = Jolt.PInvoke.ContactEventListenerGetBase(_listener);
        Mask = mask;
    }

    /// <summary>The listener as the <see cref="ContactListener"/> <see cref="PhysicsSystem.SetContactListener"/>
    /// takes. Borrowed from this listener: don't Dispose it.</summary>
    public ContactListener Handle { get; private set; }

    /// <summary>Which kinds of event are recorded.</summary>
    public ContactEventMask Mask { get; }

    /// <summary>Whether this listener has been disposed.</summary>
    public bool IsNull() => _listener == 0;

    /// <summary>How many events the buffer holds.</summary>
    public uint Capacity => Jolt.PInvoke.ContactEventListenerGetCapacity(Live());

    /// <summary>How many events are waiting to be drained.</summary>
    public uint Pending => Jolt.PInvoke.ContactEventListenerGetPending(Live());

    /// <summary>How many events were lost over this listener's life because the buffer was full. It only grows,
    /// so compare it with the last value you read.</summary>
    public uint Dropped => Jolt.PInvoke.ContactEventListenerGetDropped(Live());

    /// <summary>
    /// Copies pending events into <paramref name="events"/>, oldest first, and returns how many were copied. What
    /// doesn't fit stays for the next call, so draining in a loop until <paramref name="remaining"/> is zero is fine.
    /// </summary>
    /// <param name="events">Where to copy the events.</param>
    /// <param name="remaining">How many events are still pending after this call.</param>
    public uint Drain(Span<ContactEvent> events, out uint remaining)
    {
        var listener = Live();
        fixed (ContactEvent* eventsFixed = events)
        fixed (uint* remainingFixed = &remaining)
            return Jolt.PInvoke.ContactEventListenerDrain(listener, eventsFixed, (uint)events.Length, remainingFixed);
    }

    /// <summary>Throws away every pending event.</summary>
    public void Clear() => Jolt.PInvoke.ContactEventListenerClear(Live());

    /// <summary>Frees the listener. It must not still be installed on a physics system. Idempotent.</summary>
    public void Dispose()
    {
        var listener = Interlocked.Exchange(ref _listener, 0);
        if (listener == 0)
            return;
        Handle = default;
        Jolt.PInvoke.ContactEventListenerDestroy(listener);
    }

    /// <summary>The listener as the <see cref="ContactListener"/> a physics system takes.</summary>
    public static implicit operator ContactListener(ContactEventListener listener) => listener.Handle;

    private nint Live()
    {
        ObjectDisposedException.ThrowIf(_listener == 0, this);
        return _listener;
    }
}
