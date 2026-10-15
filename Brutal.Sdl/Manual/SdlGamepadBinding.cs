namespace Brutal.SdlApi;

[StructLayout(LayoutKind.Explicit, Size = 32)]
public struct SdlGamepadBinding
{
    [FieldOffset(0)]
    public SdlGamepadBindingType InputType;

    [FieldOffset(4)]
    public int InputButton;

    [FieldOffset(4)]
    public int InputAxis;

    [FieldOffset(8)]
    public int InputAxisMin;

    [FieldOffset(12)]
    public int InputAxisMax;

    [FieldOffset(4)]
    public int InputHat;

    [FieldOffset(8)]
    public int InputHatMask;

    [FieldOffset(16)]
    public SdlGamepadBindingType OutputType;

    [FieldOffset(20)]
    public SdlGamepadButton OutputButton;

    [FieldOffset(20)]
    public SdlGamepadAxis OutputAxis;

    [FieldOffset(24)]
    public int OutputAxisMin;

    [FieldOffset(28)]
    public int OutputAxisMax;
}
