namespace Brutal.JoltApi;

/// <summary>Which contact callback a <see cref="ContactEvent"/> was recorded from.</summary>
public enum ContactEventKind : uint
{
    /// <summary>Two bodies started touching (<see cref="ContactListener.OnContactAdded"/>).</summary>
    Added = 0,

    /// <summary>Two bodies are still touching (<see cref="ContactListener.OnContactPersisted"/>).</summary>
    Persisted = 1,

    /// <summary>Two bodies stopped touching (<see cref="ContactListener.OnContactRemoved"/>).</summary>
    Removed = 2,
}

/// <summary>Which kinds of event a <see cref="ContactEventListener"/> records.</summary>
[Flags]
public enum ContactEventMask : uint
{
    /// <summary>Nothing.</summary>
    None = 0,

    /// <summary>Contacts starting.</summary>
    Added = 1 << (int)ContactEventKind.Added,

    /// <summary>Contacts continuing, one event per touching pair per step: the noisy one.</summary>
    Persisted = 1 << (int)ContactEventKind.Persisted,

    /// <summary>Contacts ending.</summary>
    Removed = 1 << (int)ContactEventKind.Removed,

    /// <summary>Contacts starting and ending, without the per-step persisted events.</summary>
    AddedAndRemoved = Added | Removed,

    /// <summary>Every kind.</summary>
    All = Added | Persisted | Removed,
}

/// <summary>One contact event, as <see cref="ContactEventListener.Drain"/> hands it back.</summary>
/// <remarks>
/// Everything is copied at the time of the callback, so an event stays valid after the bodies move or go away.
/// A <see cref="ContactEventKind.Removed"/> event only carries the body and sub-shape IDs, since that's all Jolt
/// reports when a contact ends; its geometry and user data are zero.
/// </remarks>
[StructLayout(LayoutKind.Explicit, Size = 64)]
public struct ContactEvent
{
    // Laid out like the shim's JoltContactEvent, 64 bytes (asserted there and in the tests)

    /// <summary>Whether the contact started, continued or ended.</summary>
    [FieldOffset(0)]
    public ContactEventKind Kind;

    /// <summary>The first body's ID.</summary>
    [FieldOffset(4)]
    public uint Body1;

    /// <summary>The second body's ID.</summary>
    [FieldOffset(8)]
    public uint Body2;

    /// <summary>The sub-shape of the first body that's touching.</summary>
    [FieldOffset(12)]
    public uint SubShape1;

    /// <summary>The sub-shape of the second body that's touching.</summary>
    [FieldOffset(16)]
    public uint SubShape2;

    /// <summary>How far the shapes overlap along <see cref="Normal"/>.</summary>
    [FieldOffset(20)]
    public float PenetrationDepth;

    /// <summary>The contact normal in world space, pointing from body 1 towards body 2.</summary>
    [FieldOffset(24)]
    public float3 Normal;

    /// <summary>The first contact point, on body 1, in world space.</summary>
    [FieldOffset(36)]
    public float3 Point;

    /// <summary>The first body's user data.</summary>
    [FieldOffset(48)]
    public ulong UserData1;

    /// <summary>The second body's user data.</summary>
    [FieldOffset(56)]
    public ulong UserData2;
}
