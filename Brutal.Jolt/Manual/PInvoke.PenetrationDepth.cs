using System.Runtime.InteropServices;
using Brutal.Numerics;

namespace Brutal.JoltApi;

// Hand-written LibraryImport for the GJK/EPA penetration depth test (JoltPenetrationDepthShim.cpp).
// EPAPenetrationDepth is a template the parse can't bind.
public static partial class Jolt
{
    public static unsafe partial class PInvoke
    {
        [LibraryImport("jolt", EntryPoint = "Jolt_GetPenetrationDepth")]
        public static partial Brutal.Bool8 GetPenetrationDepth(
            ref ConvexObject a,
            ref ConvexObject b,
            float collisionToleranceSq,
            float penetrationTolerance,
            ref float3 v,
            out float3 pointA,
            out float3 pointB
        );
    }
}
