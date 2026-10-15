namespace Brutal.ShaderCApi;

/// <summary>
///     An object that contains options for compiling shaders.
/// </summary>
public partial struct CompileOptions
{
    /// <summary>Installs the built-in disk-based include resolver on this options instance.</summary>
    /// <remarks>
    ///     <para>
    ///         Overwrites any include callbacks previously set on this instance (destructive replacement).
    ///         The resolver reads the requested file from disk relative to the directory of the requesting
    ///         source, so the source name passed to the compiler must be a real, accessible file path.
    ///     </para>
    ///     <para>
    ///         This options instance must outlive any compilation that uses it; the native shaderc API
    ///         holds a raw pointer to the callbacks for the duration of the compile.
    ///     </para>
    /// </remarks>
    public unsafe void SetDefaultIncludeCallbacks()
    {
        ShaderC.PInvoke.shaderc_compile_options_set_include_callbacks(
            this,
            new IncludeResolveFn(&ShaderC.Callbacks.Resolve),
            new IncludeResultReleaseFn(&ShaderC.Callbacks.Release),
            null
        );
    }
}
