namespace Brutal.VulkanApi.Abstractions;

/// <summary>
/// Alignment helpers for <see cref="ByteSize"/>.
/// </summary>
public static class ByteSizeExtensions
{
    /// <summary>
    /// Round the value up to the next multiple of <paramref name="alignment"/>.
    /// The alignment must be a power of two; an alignment of zero returns the value unchanged.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ByteSize AlignUp(this ByteSize value, ByteSize alignment)
    {
        if (alignment == 0)
            return value;

        return ByteSize.Of((value + alignment - 1) & ~(alignment - 1));
    }

    /// <summary>
    /// True when the value is a multiple of <paramref name="alignment"/> (an alignment of zero always matches).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAlignedTo(this ByteSize value, ByteSize alignment)
    {
        return value.AlignUp(alignment) == value;
    }
}
