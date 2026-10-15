namespace Brutal.JoltApi;

// Jolt's inline world-space accessors, done in C# over the fields the struct already holds rather than a native
// call per point. The generator leaves them out (see _manualValueMembers).
public partial struct ContactManifold
{
    /// <summary>The world-space position of contact point <paramref name="index"/> on shape 1.</summary>
    /// <exception cref="ArgumentOutOfRangeException">The index is at or past the live contact point count.</exception>
    public readonly float3 GetWorldSpaceContactPointOn1(uint index) =>
        BaseOffset + PointAt(in RelativeContactPointsOn1, index, "contact points on shape 1");

    /// <summary>The world-space position of contact point <paramref name="index"/> on shape 2.</summary>
    /// <exception cref="ArgumentOutOfRangeException">The index is at or past the live contact point count.</exception>
    public readonly float3 GetWorldSpaceContactPointOn2(uint index) =>
        BaseOffset + PointAt(in RelativeContactPointsOn2, index, "contact points on shape 2");

    // Only the first Size slots are live, so the bound is the count rather than the 64-slot capacity
    private static float3 PointAt(in StaticArrayVec3_64 points, uint index, string what)
    {
        if (index >= points.Size)
            throw new ArgumentOutOfRangeException(
                nameof(index),
                index,
                $"Expected an index below the number of {what}."
            );
        return points.Elements[(int)index].XYZ;
    }
}
