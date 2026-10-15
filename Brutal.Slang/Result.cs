using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Brutal.SlangApi;

/// <summary>
///     Slang's typed return code (originally <c>SlangResult</c>, an <c>int32_t</c>).
///     Negative values indicate failure; non-negative values indicate success.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
[NativeMarshalling(typeof(ResultMarshaller))]
public readonly struct Result : IEquatable<Result>
{
    public int Value { get; }

    public Result(int value) => Value = value;

    public bool IsSuccess => Value >= 0;
    public bool IsFailure => Value < 0;

    public void ThrowIfFailure()
    {
        if (IsFailure)
            throw new SlangException(this);
    }

    public static implicit operator int(Result r) => r.Value;

    public static explicit operator Result(int v) => new(v);

    public bool Equals(Result other) => Value == other.Value;

    public override bool Equals(object? obj) => obj is Result r && Equals(r);

    public override int GetHashCode() => Value;

    public static bool operator ==(Result a, Result b) => a.Value == b.Value;

    public static bool operator !=(Result a, Result b) => a.Value != b.Value;

    public override string ToString() => $"0x{Value:X8}";
}

/// <summary>
///     Marshals <see cref="Result"/> as a plain <c>int</c> across the interop boundary.
///     Without this, the <c>[GeneratedComInterface]</c> source generator emits
///     <c>delegate* unmanaged[MemberFunction]&lt;…, Result&gt;</c> which on x64 Windows
///     can cause the runtime to pass a hidden return pointer, shifting all arguments
///     and triggering an access violation in the native COM vtable call.
/// </summary>
[CustomMarshaller(typeof(Result), MarshalMode.Default, typeof(ResultMarshaller))]
internal static class ResultMarshaller
{
    public static int ConvertToUnmanaged(Result managed) => managed.Value;

    public static Result ConvertToManaged(int unmanaged) => new(unmanaged);
}
