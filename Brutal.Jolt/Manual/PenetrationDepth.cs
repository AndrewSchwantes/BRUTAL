using Brutal.Numerics;

namespace Brutal.JoltApi;

// Hand-written wrapper for GetPenetrationDepth (EPAPenetrationDepth is a template the parse can't bind).
public static partial class Jolt
{
    /// <summary>
    ///   <para>
    ///     Whether two convex objects intersect, by GJK and then EPA when GJK can't tell, and if so the
    ///     penetration: v is the separating direction (initialise it to any non-zero guess, it is refined in
    ///     place), pointA and pointB the deepest points on each (zero when they don't intersect).
    ///     collisionToleranceSq is the squared distance counted as touching, penetrationTolerance the EPA
    ///     convergence tolerance (at least FLT_EPSILON, 1.1920929E-07; C#'s float.Epsilon is far smaller and
    ///     throws).
    ///   </para>
    /// </summary>
    public static bool GetPenetrationDepth(
        ConvexObject a,
        ConvexObject b,
        float collisionToleranceSq,
        float penetrationTolerance,
        ref float3 v,
        out float3 pointA,
        out float3 pointB
    )
    {
        if (!(penetrationTolerance >= 1.1920929E-07f))
            throw new ArgumentOutOfRangeException(
                nameof(penetrationTolerance),
                penetrationTolerance,
                "Expected at least FLT_EPSILON (1.1920929E-07) - float.Epsilon is far smaller."
            );
        return Jolt.PInvoke.GetPenetrationDepth(
            ref a,
            ref b,
            collisionToleranceSq,
            penetrationTolerance,
            ref v,
            out pointA,
            out pointB
        );
    }
}
