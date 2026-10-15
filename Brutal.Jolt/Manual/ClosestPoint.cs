using Brutal.Numerics;

namespace Brutal.JoltApi;

// Hand-written wrappers for JPH::ClosestPoint free functions (templates the parse can't bind).
public static partial class Jolt
{
    /// <summary>
    ///   <para>
    ///     The barycentric coordinates of the point on segment ab closest to the origin, so that u * a + v * b is
    ///     that point. False when the segment is degenerate, which leaves the closest vertex.
    ///   </para>
    /// </summary>
    public static bool GetBaryCentricCoordinates(float3 a, float3 b, out float u, out float v) =>
        Jolt.PInvoke.ClosestPointGetBaryCentricCoordinates(a, b, out u, out v);

    /// <summary>
    ///   <para>
    ///     The barycentric coordinates of the origin projected onto triangle abc, so that u * a + v * b + w * c is
    ///     that point. False when the triangle is degenerate.
    ///   </para>
    /// </summary>
    public static bool GetBaryCentricCoordinates(float3 a, float3 b, float3 c, out float u, out float v, out float w) =>
        Jolt.PInvoke.ClosestPointGetBaryCentricCoordinates_2(a, b, c, out u, out v, out w);

    /// <summary>
    ///   <para>
    ///     The point on segment ab closest to the origin; set gets a bit per vertex that is part of the result (1 =
    ///     a, 2 = b).
    ///   </para>
    /// </summary>
    public static float3 GetClosestPointOnLine(float3 a, float3 b, out uint set) =>
        Jolt.PInvoke.ClosestPointGetClosestPointOnLine(a, b, out set);

    /// <summary>
    ///   <para>
    ///     The point on triangle abc closest to the origin; set gets a bit per vertex that is part of the result.
    ///   </para>
    /// </summary>
    public static float3 GetClosestPointOnTriangle(float3 a, float3 b, float3 c, out uint set) =>
        Jolt.PInvoke.ClosestPointGetClosestPointOnTriangle(a, b, c, out set);

    /// <summary>
    ///   <para>
    ///     The point on (or in) tetrahedron abcd closest to the origin; set gets a bit per vertex that is part of
    ///     the result, all four when the origin is inside.
    ///   </para>
    /// </summary>
    public static float3 GetClosestPointOnTetrahedron(float3 a, float3 b, float3 c, float3 d, out uint set) =>
        Jolt.PInvoke.ClosestPointGetClosestPointOnTetrahedron(a, b, c, d, out set);
}
