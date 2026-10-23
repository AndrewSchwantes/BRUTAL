using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;
using Brutal;

namespace Brutal.SlangApi;

[CustomMarshaller(
    typeof(CustomMarshallerAttribute.GenericPlaceholder),
    MarshalMode.Default,
    typeof(SlangHandleMarshaller<>)
)]
internal static class SlangHandleMarshaller<T>
    where T : unmanaged
{
    public static nint ConvertToUnmanaged(T managed) => Unsafe.BitCast<T, nint>(managed);

    public static T ConvertToManaged(nint unmanaged) => Unsafe.BitCast<nint, T>(unmanaged);
}

[CustomMarshaller(typeof(ByteSize), MarshalMode.Default, typeof(SlangSizeMarshaller))]
internal static class SlangSizeMarshaller
{
    public static nuint ConvertToUnmanaged(ByteSize managed) => managed.Value;

    public static ByteSize ConvertToManaged(nuint unmanaged) => (ByteSize)unmanaged;
}

[CustomMarshaller(typeof(Bool8), MarshalMode.Default, typeof(SlangBoolMarshaller))]
internal static class SlangBoolMarshaller
{
    public static byte ConvertToUnmanaged(Bool8 managed) => Unsafe.BitCast<Bool8, byte>(managed);

    public static Bool8 ConvertToManaged(byte unmanaged) => Unsafe.BitCast<byte, Bool8>(unmanaged);
}
