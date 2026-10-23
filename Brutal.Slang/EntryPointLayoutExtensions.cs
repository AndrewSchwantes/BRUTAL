namespace Brutal.SlangApi;

/// <summary>
///     Hand-written helpers on entry-point reflection handles for flat-C getters that fill
///     output arrays, which the generated signatures expose as raw pointers.
/// </summary>
public static unsafe class EntryPointLayoutExtensions
{
    /// <summary>
    ///     Returns a compute entry point's thread-group size, its <c>[numthreads]</c>, along
    ///     X, Y and Z. The handle borrows from its program layout, so call this only while the
    ///     component that produced the layout is alive.
    /// </summary>
    public static (ulong X, ulong Y, ulong Z) GetComputeThreadGroupSize(this EntryPointLayout entryPoint)
    {
        var sizes = stackalloc ulong[3];
        Slang.PInvoke.spReflectionEntryPoint_getComputeThreadGroupSize(entryPoint, 3, sizes);
        return (sizes[0], sizes[1], sizes[2]);
    }
}
