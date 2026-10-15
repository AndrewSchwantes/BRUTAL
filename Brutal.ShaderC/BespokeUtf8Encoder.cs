using CommunityToolkit.HighPerformance;
using CommunityToolkit.HighPerformance.Buffers;

namespace Brutal.ShaderCApi;

using String16 = string;
using StringView8 = ReadOnlySpan<byte>;

/// <summary>
/// A helper struct to convert a set of strings into a single contiguous UTF-8 encoded byte array, with a null terminator after each string.
/// </summary>
internal readonly ref struct BespokeUtf8Encoder : IDisposable
{
    private readonly SpanOwner<byte> _storage;
    private readonly Span<int> _indexToEndLookup;
    private readonly Span<byte> _content;

    public BespokeUtf8Encoder(String16 i1, String16 i2, out StringView8 o1, out StringView8 o2)
        : this(i1, i2)
    {
        o1 = Read(0);
        o2 = Read(1);
    }

    public BespokeUtf8Encoder(
        String16 i1,
        String16 i2,
        String16 i3,
        out StringView8 o1,
        out StringView8 o2,
        out StringView8 o3
    )
        : this(i1, i2, i3)
    {
        o1 = Read(0);
        o2 = Read(1);
        o3 = Read(2);
    }

    private BespokeUtf8Encoder(params ReadOnlySpan<String16> strings)
    {
        // work out how many bytes we're going to need to encode everything (strings + lookup table)
        var headerByteCount = (int)ByteSize.Of<int>(strings.Length);
        var contentByteCount = 0;
        foreach (var t in strings)
            contentByteCount += Utf8.GetMaxByteCount(t.Length) + 1;

        // allocate the storage, then carve it into the lookup table and the content region
        _storage = SpanOwner<byte>.Allocate(headerByteCount + contentByteCount);
        _indexToEndLookup = _storage.Span[..headerByteCount].Cast<byte, int>();
        _content = _storage.Span[headerByteCount..];

        var offset = 0;
        for (var index = 0; index < strings.Length; index++)
        {
            offset += Utf8.GetBytes(strings[index], _content[offset..]);
            _content[offset] = 0;
            _indexToEndLookup[index] = offset++;
        }
    }

    private StringView8 Read(int index)
    {
        // grab the end index first, also serves as a bound check
        var endIndex = _indexToEndLookup[index];
        // the start index is the previous string's NUL terminator + 1, or zero if this is the first string
        var startIndex = index > 0 ? _indexToEndLookup[index - 1] + 1 : 0;
        return _content[startIndex..endIndex];
    }

    public void Dispose() => _storage.Dispose();
}
