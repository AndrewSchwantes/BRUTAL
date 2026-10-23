using System.Runtime.InteropServices.Marshalling;

namespace Brutal.SlangApi;

// Some Slang 2026.7 objects implement a public interface but answer QueryInterface
// only for a native interface derived from it, whose vtable therefore starts with the
// public one. Preserve the public IIDs and prefer them; only retry E_NOINTERFACE
// against the derived interface. See upstream
// source/compiler-core/slang-artifact-associated{,-impl}.{h,cpp} (IMetadata) and
// source/compiler-core/slang-artifact{,-impl}.{h,cpp} (ICompileResult).
internal sealed class SlangComWrappers : StrategyBasedComWrappers
{
    protected override IIUnknownStrategy GetOrCreateIUnknownStrategy() => DerivedInterfaceQueryStrategy.Instance;

    private sealed unsafe class DerivedInterfaceQueryStrategy : IIUnknownStrategy
    {
        internal static readonly DerivedInterfaceQueryStrategy Instance = new();
        private const int NoInterface = unchecked((int)0x80004002);

        private static readonly Dictionary<System.Guid, System.Guid> DerivedIds = new()
        {
            // IArtifactPostEmitMetadata
            [typeof(IMetadata).GUID] = new("5d03bce9-afb1-4fc8-a46f-3ce07b061b1b"),
            // IArtifact
            [typeof(ICompileResult).GUID] = new("f90acdb0-9a4a-414e-8545-8b26c92d9442"),
        };

        public void* CreateInstancePointer(void* unknown) => DefaultIUnknownStrategy.CreateInstancePointer(unknown);

        public int QueryInterface(void* instancePtr, in System.Guid iid, out void* ppObj)
        {
            var result = DefaultIUnknownStrategy.QueryInterface(instancePtr, in iid, out ppObj);
            if (result == NoInterface && DerivedIds.TryGetValue(iid, out var derived))
                return DefaultIUnknownStrategy.QueryInterface(instancePtr, in derived, out ppObj);
            return result;
        }

        public int Release(void* instancePtr) => DefaultIUnknownStrategy.Release(instancePtr);
    }
}
