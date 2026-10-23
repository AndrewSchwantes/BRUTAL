using System.Runtime.InteropServices.Marshalling;

namespace Brutal.SlangApi;

// Slang's direct interface-returning getters/loaders lend a reference. The RCW
// takes its own reference, but the returned pointer must not be released by the stub.
[CustomMarshaller(
    typeof(CustomMarshallerAttribute.GenericPlaceholder),
    MarshalMode.Default,
    typeof(BorrowedSlangInterfaceMarshaller<>)
)]
internal static unsafe class BorrowedSlangInterfaceMarshaller<T>
    where T : class
{
    public static T? ConvertToManaged(void* unmanaged) => Slang.WrapBorrowedComInstance<T>((nint)unmanaged);

    public static void* ConvertToUnmanaged(T? managed) => ComInterfaceMarshaller<T>.ConvertToUnmanaged(managed);
}
