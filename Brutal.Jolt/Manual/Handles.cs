namespace Brutal.JoltApi;

// Hand-written bits the generator can't express (see Generator.Interop.cs for what it does emit):
// Factory.Instance, and the one down-cast Jolt keeps no type information for (the checked AsX() casts on
// Shape, Constraint and VehicleController are generated).

public readonly partial struct Factory
{
    /// <summary>Mirrors <c>JPH::Factory::sInstance</c>.</summary>
    /// <remarks>
    /// Assigning hands the factory to the global, and the getter borrows it back. Tear it down with
    /// <see cref="DestroyInstance"/>; never <c>Dispose</c> the handle you assigned or the one you get back.
    /// </remarks>
    public static Factory Instance
    {
        get => GetInstance();
        set => SetInstance(value);
    }

    /// <summary>Deletes the global factory instance and clears <see cref="Instance"/>.</summary>
    public static void DestroyInstance()
    {
        var instance = GetInstance();
        if (!instance.IsNull())
            instance.Dispose();
        SetInstance(default);
    }
}

public readonly partial struct MotionProperties
{
    /// <summary>Views these motion properties as the soft-body ones they are.</summary>
    /// <remarks>
    /// <see cref="Body.GetMotionProperties"/> hands back the base type, so this is how a soft body's
    /// vertices are reached. Same handle, not a new reference, so don't <c>Dispose</c> it. Unchecked:
    /// only valid on a soft body.
    /// </remarks>
    public SoftBodyMotionProperties AsSoftBodyMotionProperties() =>
        Unsafe.BitCast<MotionProperties, SoftBodyMotionProperties>(this);
}
