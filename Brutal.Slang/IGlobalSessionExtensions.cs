using System.Runtime.InteropServices;

namespace Brutal.SlangApi;

/// <summary>
///     Hand-written ergonomic helpers on top of the generated <see cref="IGlobalSession"/>
///     COM interface. Wraps <c>byte*</c>-returning UTF-8 string accessors into managed
///     <see cref="string"/> returns so callers don't have to call
///     <see cref="Marshal.PtrToStringUTF8(IntPtr)"/> by hand.
/// </summary>
public static unsafe partial class IGlobalSessionExtensions
{
    /// <summary>
    ///     Slang's build tag string (e.g. <c>"slang-2026.7"</c>). Returns <c>null</c> if
    ///     the underlying call yields a null pointer.
    /// </summary>
    public static string? GetBuildTag(this IGlobalSession self) =>
        Marshal.PtrToStringUTF8((nint)self.GetBuildTagString());
}
