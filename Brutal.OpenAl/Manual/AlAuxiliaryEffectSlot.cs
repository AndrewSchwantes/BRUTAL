namespace Brutal.OpenAlApi;

public readonly partial struct AlAuxiliaryEffectSlot
{
    public void Load(AlEffect effect)
    {
        Al.PInvoke.AuxiliaryEffectSloti(Name, Al.Constants.AL_EFFECTSLOT_EFFECT, (int)effect.Name);
    }
}
