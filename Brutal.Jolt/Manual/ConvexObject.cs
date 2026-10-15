using System.Runtime.InteropServices;

namespace Brutal.JoltApi;

/// <summary>
/// A convex object for <see cref="Jolt.GetPenetrationDepth"/>: a box (with an optional convex radius), a sphere, or a
/// point with a radius, placed by a transform.
/// </summary>
/// <remarks>
/// A sphere and a point with a radius describe the same volume, but the solver sees them differently: the sphere is
/// one support shape, the point hands the radius to GJK as a convex radius, the way Jolt's own shapes do.
/// </remarks>
[StructLayout(LayoutKind.Sequential)]
public readonly struct ConvexObject
{
    // Laid out like the shim's JoltConvexObject, 112 bytes (asserted there and in the tests)
    private readonly uint _kind;
    private readonly float _radius;
    private readonly float _convexRadius;
    private readonly float4x4 _transform;
    private readonly float3 _center;
    private readonly float3 _min;
    private readonly float3 _max;

    private ConvexObject(
        uint kind,
        float radius,
        float convexRadius,
        float4x4 transform,
        float3 center,
        float3 min,
        float3 max
    )
    {
        _kind = kind;
        _radius = radius;
        _convexRadius = convexRadius;
        _transform = transform;
        _center = center;
        _min = min;
        _max = max;
    }

    /// <summary>A box from <paramref name="min"/> to <paramref name="max"/> in <paramref name="transform"/>'s space,
    /// grown by <paramref name="convexRadius"/>.</summary>
    public static ConvexObject Box(float3 min, float3 max, float4x4 transform, float convexRadius = 0f) =>
        new(0, 0f, convexRadius, transform, float3.Zero, min, max);

    /// <summary>A box in <paramref name="transform"/>'s space, grown by <paramref name="convexRadius"/>.</summary>
    public static ConvexObject Box(AABox box, float4x4 transform, float convexRadius = 0f) =>
        Box(box.Min, box.Max, transform, convexRadius);

    /// <summary>A sphere in <paramref name="transform"/>'s space.</summary>
    public static ConvexObject Sphere(float3 center, float radius, float4x4 transform) =>
        new(1, radius, 0f, transform, center, float3.Zero, float3.Zero);

    /// <summary>A sphere in world space.</summary>
    public static ConvexObject Sphere(float3 center, float radius) => Sphere(center, radius, float4x4.Identity);

    /// <summary>A point in world space, grown by <paramref name="radius"/> as a convex radius.</summary>
    public static ConvexObject Point(float3 position, float radius) =>
        new(2, radius, 0f, float4x4.Identity, position, float3.Zero, float3.Zero);
}
