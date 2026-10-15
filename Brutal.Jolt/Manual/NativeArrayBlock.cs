namespace Brutal.JoltApi;

// The shim's JoltArrayBlock header (Generated/Shim/JoltInterop.h, which static_asserts this layout), read in place
// by NativeArray and NativeHandleArray instead of a native call per field
[StructLayout(LayoutKind.Sequential)]
internal struct NativeArrayBlock
{
    public nint Data;
    public ulong Count;
    public nint Owner;
    public nint Destroy;
    public ulong Stride;
}
