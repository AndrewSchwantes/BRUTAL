using CommunityToolkit.HighPerformance.Buffers;

namespace Brutal.OpenAlApi;

internal ref struct Utf8Buffer
{
    public const int StackBufferSize = 0x100;

    private SpanOwner<byte> _owner;
    private readonly ReadOnlySpan<byte> _bytes;

    public ReadOnlySpan<byte> Span => _bytes;

    public int Length => _bytes.IsEmpty ? 0 : _bytes.Length - 1;

    public Utf8Buffer(string? managed, Span<byte> buffer)
    {
        _owner = SpanOwner<byte>.Empty;
        if (managed is null)
            return;

        const int MaxUtf8BytesPerChar = 3;

        // >= for the NUL terminator; the long cast avoids a checked multiply.
        if ((long)MaxUtf8BytesPerChar * managed.Length + 1 >= buffer.Length)
        {
            // The estimate overflows the buffer - only now pay for an exact count.
            var exactByteCount = checked(Encoding.UTF8.GetByteCount(managed) + 1); // + 1 for the NUL
            if (exactByteCount > buffer.Length)
            {
                _owner = SpanOwner<byte>.Allocate(exactByteCount);
                var spillLength = Encoding.UTF8.GetBytes(managed, _owner.Span);
                _owner.Span[spillLength] = 0;
                _bytes = _owner.Span[..(spillLength + 1)];
                return;
            }
        }

        var length = Encoding.UTF8.GetBytes(managed, buffer);
        buffer[length] = 0;
        _bytes = buffer[..(length + 1)];
    }

    public void Dispose() => _owner.Dispose();
}
