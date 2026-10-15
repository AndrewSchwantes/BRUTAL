using Brutal.Strings;

namespace Brutal.OpenAlApi;

internal static unsafe class StringUtils
{
    internal static string? Utf8ToString(byte* p) => p is null ? null : Utf8.GetString(p);

    internal static string[] Utf8ListToStrings(byte* p)
    {
        if (p is null)
            return [];

        var result = new List<string>();
        while (*p != 0)
        {
            var entry = Utf8.GetNullTerminatedSpan(p);
            result.Add(Utf8.GetString(entry));
            p += entry.Length + 1; // step over this entry's terminator
        }
        return [.. result];
    }
}
