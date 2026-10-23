namespace Brutal.SlangApi;

/// <summary>
///     Hand-written ergonomic helpers on top of the generated <see cref="ISlangBlob"/>
///     COM interface. The interface itself only exposes a raw <c>void*</c> + size pair;
///     callers shouldn't have to assemble a span by hand to read the contents.
/// </summary>
public static unsafe partial class ISlangBlobExtensions
{
    /// <summary>
    ///     Returns the blob's contents as a <see cref="ReadOnlySpan{Byte}"/>. The span is
    ///     valid until the blob is passed to <see cref="Slang.Release"/>. Copy if you need
    ///     the bytes to outlive it.
    /// </summary>
    public static ReadOnlySpan<byte> AsSpan(this ISlangBlob self) =>
        new(self.GetBufferPointer(), checked((int)self.GetBufferSize().Value));
}
