namespace Brutal.GltfApi;

public partial class Accessor
{
    /// <summary>
    /// Gets the size in bytes of a single element of this accessor
    /// </summary>
    /// <returns>Size in bytes of accessor element, accounting for component type, and it's vector element count</returns>
    /// <exception cref="InvalidOperationException">Unsupported type</exception>
    /// <exception cref="NotSupportedException">
    /// Matrix accessor whose columns are not naturally 4-byte aligned (Mat2/byte, Mat3/byte, Mat3/short).
    /// The glTF spec requires such columns to be padded to 4-byte boundaries, which this library does not implement.
    /// </exception>
    public ByteSize ElementSize()
    {
        var componentSize = ComponentType switch
        {
            ComponentTypeEnum.Byte => 1,
            ComponentTypeEnum.UnsignedByte => 1,
            ComponentTypeEnum.Short => 2,
            ComponentTypeEnum.UnsignedShort => 2,
            ComponentTypeEnum.UnsignedInt => 4,
            ComponentTypeEnum.Float => 4,
            _ => throw new InvalidOperationException($"Unsupported component type {ComponentType}"),
        };
        var typeCount = Type switch
        {
            TypeEnum.Scalar => 1,
            TypeEnum.Vec2 => 2,
            TypeEnum.Vec3 => 3,
            TypeEnum.Vec4 => 4,
            TypeEnum.Mat2 => 4,
            TypeEnum.Mat3 => 9,
            TypeEnum.Mat4 => 16,
            _ => throw new InvalidOperationException($"Unsupported accessor type {Type}"),
        };

        // glTF requires each matrix column to start on a 4-byte boundary. All float matrices (and
        // Mat2/short, Mat4/any) are naturally aligned; the remaining combinations would need per-column
        // padding that componentSize * typeCount does not account for, so reject them outright.
        var matrixRows = Type switch
        {
            TypeEnum.Mat2 => 2,
            TypeEnum.Mat3 => 3,
            TypeEnum.Mat4 => 4,
            _ => 0,
        };
        if (matrixRows != 0 && (matrixRows * componentSize) % 4 != 0)
            throw new NotSupportedException(
                $"Matrix accessor {Type}/{ComponentType} requires column padding, which is not supported"
            );

        return ByteSize.Of(componentSize * typeCount);
    }
}
