namespace Brutal.JoltApi;

/// <summary>What <see cref="BodyState.Flags"/> says about the body it was read from.</summary>
[Flags]
public enum BodyStateFlags : uint
{
    /// <summary>The ID named no body (removed, or a stale sequence number); the rest of the state is zeroed.</summary>
    None = 0,

    /// <summary>The ID named a body and the state was read from it.</summary>
    Valid = 1,

    /// <summary>The body is awake and being simulated.</summary>
    Active = 2,
}

/// <summary>
/// One body's pose, velocities, user data and activation, as <see cref="BodyLockInterface.GetBodyStates"/> reads them
/// for many bodies under one lock.
/// </summary>
/// <remarks>
/// A snapshot for syncing an ECS, a network state or render interpolation from the simulation. A static body reads
/// as zero velocity. An ID that no longer names a body reads as the origin with the identity rotation and
/// <see cref="Flags"/> clear of <see cref="BodyStateFlags.Valid"/>.
/// </remarks>
[StructLayout(LayoutKind.Explicit, Size = 64)]
public struct BodyState
{
    // Laid out like the shim's JoltBodyState, 64 bytes (asserted there and in the tests)

    /// <summary>The body's position (its centre of mass is offset from this by the shape).</summary>
    [FieldOffset(0)]
    public float3 Position;

    /// <summary>Whether the ID named a body, and whether that body is active.</summary>
    [FieldOffset(12)]
    public BodyStateFlags Flags;

    /// <summary>The body's rotation.</summary>
    [FieldOffset(16)]
    public floatQuat Rotation;

    /// <summary>Linear velocity of the centre of mass, in m/s.</summary>
    [FieldOffset(32)]
    public float3 LinearVelocity;

    /// <summary>Angular velocity, in rad/s.</summary>
    [FieldOffset(44)]
    public float3 AngularVelocity;

    /// <summary>The body's user data, as <see cref="BodyInterface.GetUserData"/> reads it.</summary>
    [FieldOffset(56)]
    public ulong UserData;

    /// <summary>Whether the ID named a body.</summary>
    public readonly bool IsValid => (Flags & BodyStateFlags.Valid) != 0;

    /// <summary>Whether the body is awake and being simulated.</summary>
    public readonly bool IsActive => (Flags & BodyStateFlags.Active) != 0;
}
