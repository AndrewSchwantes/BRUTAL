using System.Runtime.InteropServices;
using Brutal.Numerics;

namespace Brutal.JoltApi;

// Hand-written LibraryImports for JPH::ClosestPoint free functions (JoltClosestPointShim.cpp).
// These are templates the parse can't bind.
public static partial class Jolt
{
    public static unsafe partial class PInvoke
    {
        [LibraryImport("jolt", EntryPoint = "Jolt_ClosestPointGetBaryCentricCoordinates")]
        public static partial Brutal.Bool8 ClosestPointGetBaryCentricCoordinates(
            float3 a,
            float3 b,
            out float u,
            out float v
        );

        [LibraryImport("jolt", EntryPoint = "Jolt_ClosestPointGetBaryCentricCoordinates_2")]
        public static partial Brutal.Bool8 ClosestPointGetBaryCentricCoordinates_2(
            float3 a,
            float3 b,
            float3 c,
            out float u,
            out float v,
            out float w
        );

        [LibraryImport("jolt", EntryPoint = "Jolt_ClosestPointGetClosestPointOnLine")]
        public static partial float3 ClosestPointGetClosestPointOnLine(float3 a, float3 b, out uint set);

        [LibraryImport("jolt", EntryPoint = "Jolt_ClosestPointGetClosestPointOnTriangle")]
        public static partial float3 ClosestPointGetClosestPointOnTriangle(float3 a, float3 b, float3 c, out uint set);

        [LibraryImport("jolt", EntryPoint = "Jolt_ClosestPointGetClosestPointOnTetrahedron")]
        public static partial float3 ClosestPointGetClosestPointOnTetrahedron(
            float3 a,
            float3 b,
            float3 c,
            float3 d,
            out uint set
        );
    }
}
