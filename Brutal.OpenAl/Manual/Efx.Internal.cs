using static Brutal.OpenAlApi.Al.Constants;

namespace Brutal.OpenAlApi;

public sealed partial class Efx
{
    // Creation drains the error latch first and deletes the half-built object if setting its type fails.

    // AL calls go to the current context, so it must belong to this Efx's device.
    private void ThrowIfNotCurrentOnDevice()
    {
        Device.ThrowIfDisposed();
        var device = Device.Handle;
        var current = Alc.PInvoke.GetCurrentContext();
        if (current.IsNull())
        {
            throw new InvalidOperationException(
                "No AL context is current. EFX objects are created on the current context; make a "
                    + "context on this Efx instance's device current first."
            );
        }

        if (Alc.PInvoke.GetContextsDevice(current).Pointer != device.Pointer)
        {
            throw new InvalidOperationException(
                "The current AL context belongs to a different device than this Efx instance, which "
                    + "belongs to the device it was obtained from. Make a context on that device "
                    + "current, or use the Efx of the device you are actually using."
            );
        }
    }

    internal AlEffect CreateEffectChecked(int type)
    {
        ThrowIfNotCurrentOnDevice();
        Al.GetError();
        var effect = GenEffect();
        Al.PInvoke.Effecti(effect.Name, AL_EFFECT_TYPE, type);
        try
        {
            Al.ThrowIfError();
        }
        catch
        {
            Rollback(effect);
            throw;
        }
        return effect;
    }

    internal AlFilter CreateFilterChecked(int type)
    {
        ThrowIfNotCurrentOnDevice();
        Al.GetError();
        var filter = GenFilter();
        Al.PInvoke.Filteri(filter.Name, AL_FILTER_TYPE, type);
        try
        {
            Al.ThrowIfError();
        }
        catch
        {
            Rollback(filter);
            throw;
        }
        return filter;
    }

    // Single-step, so nothing to roll back - but the stale-latch drain still applies.
    internal AlAuxiliaryEffectSlot CreateSlotChecked()
    {
        ThrowIfNotCurrentOnDevice();
        Al.GetError();
        var slot = GenAuxiliaryEffectSlot();
        Al.ThrowIfError();
        return slot;
    }

    // Undoes the generate step of a failed creation; a null name means generation itself failed.
    // The trailing drain discards any secondary error the cleanup delete latches.
    private void Rollback(AlEffect effect)
    {
        if (effect.IsNull())
            return;
        DeleteEffects([effect]);
        Al.GetError();
    }

    private void Rollback(AlFilter filter)
    {
        if (filter.IsNull())
            return;
        DeleteFilters([filter]);
        Al.GetError();
    }
}
