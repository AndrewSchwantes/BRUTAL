using System.Runtime.InteropServices.Marshalling;

namespace Brutal.SlangApi;

[CustomMarshaller(
    typeof(CustomMarshallerAttribute.GenericPlaceholder),
    MarshalMode.Default,
    typeof(SlangInterfaceMarshaller<>)
)]
internal static unsafe class SlangInterfaceMarshaller<T>
    where T : class
{
    public static T? ConvertToManaged(void* unmanaged) => Slang.WrapBorrowedComInstance<T>((nint)unmanaged);

    public static void* ConvertToUnmanaged(T? managed) =>
        managed is null ? null : (void*)Slang.GetComInstance<T>(managed);

    public static void Free(void* unmanaged)
    {
        if (unmanaged != null)
            Marshal.Release((nint)unmanaged);
    }
}
