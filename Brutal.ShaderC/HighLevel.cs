namespace Brutal.ShaderCApi;

/// <summary>
/// Extension methods providing a high-level throwing API over the generated <see cref="Compiler"/> surface.
/// </summary>
public static class CompilerExtensions
{
    /// <summary>
    /// Returns <paramref name="result"/> unchanged when compilation succeeded; otherwise throws the
    /// typed <see cref="ShaderException"/> that corresponds to the <see cref="CompilationStatus"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method is fluent and does <b>not</b> dispose <paramref name="result"/>; ownership remains
    /// with the caller regardless of whether it succeeds or throws.
    /// </para>
    /// </remarks>
    /// <param name="result">The compilation result to inspect.</param>
    /// <returns><paramref name="result"/> when <see cref="CompilationStatus.Success"/>.</returns>
    /// <exception cref="ShaderInvalidStageException">
    /// Thrown when the status is <see cref="CompilationStatus.InvalidStage"/>.
    /// </exception>
    /// <exception cref="ShaderCompilationException">
    /// Thrown when the status is <see cref="CompilationStatus.CompilationError"/>.
    /// </exception>
    /// <exception cref="ShaderInternalException">
    /// Thrown when the status is <see cref="CompilationStatus.InternalError"/>.
    /// </exception>
    /// <exception cref="ShaderNullResultObjectException">
    /// Thrown when the status is <see cref="CompilationStatus.NullResultObject"/>.
    /// </exception>
    /// <exception cref="ShaderInvalidAssemblyException">
    /// Thrown when the status is <see cref="CompilationStatus.InvalidAssembly"/>.
    /// </exception>
    /// <exception cref="ShaderValidationException">
    /// Thrown when the status is <see cref="CompilationStatus.ValidationError"/>.
    /// </exception>
    /// <exception cref="ShaderTransformationException">
    /// Thrown when the status is <see cref="CompilationStatus.TransformationError"/>.
    /// </exception>
    /// <exception cref="ShaderConfigurationException">
    /// Thrown when the status is <see cref="CompilationStatus.ConfigurationError"/>.
    /// </exception>
    /// <exception cref="ShaderException">Thrown when the status is unrecognised.</exception>
    public static CompilationResult ThrowIfFailed(this in CompilationResult result) =>
        result.GetCompilationStatus() switch
        {
            CompilationStatus.Success => result,
            CompilationStatus.InvalidStage => throw new ShaderInvalidStageException(result),
            CompilationStatus.CompilationError => throw new ShaderCompilationException(result),
            CompilationStatus.InternalError => throw new ShaderInternalException(result),
            CompilationStatus.NullResultObject => throw new ShaderNullResultObjectException(result),
            CompilationStatus.InvalidAssembly => throw new ShaderInvalidAssemblyException(result),
            CompilationStatus.ValidationError => throw new ShaderValidationException(result),
            CompilationStatus.TransformationError => throw new ShaderTransformationException(result),
            CompilationStatus.ConfigurationError => throw new ShaderConfigurationException(result),
            _ => throw new ShaderException(result),
        };

    /// <summary>
    /// Returns <paramref name="result"/> when compilation succeeded; otherwise disposes the intermediate
    /// result and rethrows the typed <see cref="ShaderException"/>, so a failed compile never leaks the
    /// native handle while a successful one stays owned by the caller.
    /// </summary>
    private static CompilationResult ThrowIfFailedOrDispose(this in CompilationResult result)
    {
        try
        {
            return result.ThrowIfFailed();
        }
        catch
        {
            result.Dispose();
            throw;
        }
    }

    /// <summary>
    /// Compiles GLSL/HLSL <paramref name="source"/> to binary SPIR-V, throwing a typed
    /// <see cref="ShaderException"/> on failure.
    /// </summary>
    /// <remarks>
    /// <para>
    /// On success the returned <see cref="CompilationResult"/> is owned by the caller and must be
    /// disposed. While the result is undisposed the raw SPIR-V bytes are accessible via
    /// <see cref="CompilationResult.GetBytes()"/>.
    /// </para>
    /// <para>
    /// On failure the intermediate result is disposed before the exception propagates, so no
    /// native handle is leaked.
    /// </para>
    /// </remarks>
    /// <param name="c">The compiler to use.</param>
    /// <param name="source">The GLSL/HLSL source code encoded as UTF-8.</param>
    /// <param name="kind">The shader stage.</param>
    /// <param name="inputFileName">Source file name used in diagnostic messages.</param>
    /// <param name="entryPoint">Name of the entry-point function.</param>
    /// <param name="options">Compilation options.</param>
    /// <returns>
    /// A <see cref="CompilationResult"/> with <see cref="CompilationStatus.Success"/> containing
    /// the compiled SPIR-V bytes.
    /// </returns>
    /// <exception cref="ShaderException">Thrown on any compilation failure.</exception>
    public static CompilationResult Compile(
        this in Compiler c,
        ReadOnlySpan<byte> source,
        ShaderKind kind,
        RefString8 inputFileName,
        RefString8 entryPoint,
        CompileOptions options
    )
    {
        return c.CompileIntoSpv(source, kind, inputFileName, entryPoint, options).ThrowIfFailedOrDispose();
    }

    /// <summary>
    /// Pre-processes GLSL/HLSL <paramref name="source"/>, throwing a typed
    /// <see cref="ShaderException"/> on failure.
    /// </summary>
    /// <remarks>
    /// <para>
    /// On success the returned <see cref="CompilationResult"/> is owned by the caller and must be
    /// disposed. While the result is undisposed the pre-processed text bytes are accessible via
    /// <see cref="CompilationResult.GetBytes()"/>.
    /// </para>
    /// <para>
    /// On failure the intermediate result is disposed before the exception propagates, so no
    /// native handle is leaked.
    /// </para>
    /// </remarks>
    /// <param name="c">The compiler to use.</param>
    /// <param name="source">The GLSL/HLSL source code encoded as UTF-8.</param>
    /// <param name="kind">The shader stage.</param>
    /// <param name="inputFileName">Source file name used in diagnostic messages.</param>
    /// <param name="entryPoint">Name of the entry-point function.</param>
    /// <param name="options">Compilation options.</param>
    /// <returns>
    /// A <see cref="CompilationResult"/> with <see cref="CompilationStatus.Success"/> containing
    /// the pre-processed text.
    /// </returns>
    /// <exception cref="ShaderException">Thrown on any pre-processing failure.</exception>
    public static CompilationResult Preprocess(
        this in Compiler c,
        ReadOnlySpan<byte> source,
        ShaderKind kind,
        RefString8 inputFileName,
        RefString8 entryPoint,
        CompileOptions options
    )
    {
        return c.CompileIntoPreprocessedText(source, kind, inputFileName, entryPoint, options).ThrowIfFailedOrDispose();
    }

    /// <summary>
    /// Compiles GLSL/HLSL <paramref name="source"/> (supplied as a managed <see cref="string"/>) to binary
    /// SPIR-V, throwing a typed <see cref="ShaderException"/> on failure.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This overload UTF-8 encodes <paramref name="source"/>, <paramref name="inputFileName"/>, and
    /// <paramref name="entryPoint"/> then delegates to the span-based <see cref="Compile(in Compiler, ReadOnlySpan{byte}, ShaderKind, RefString8, RefString8, CompileOptions)"/> overload.
    /// </para>
    /// <para>
    /// On success the returned <see cref="CompilationResult"/> is owned by the caller and must be
    /// disposed. While the result is undisposed the raw SPIR-V bytes are accessible via
    /// <see cref="CompilationResult.GetBytes()"/>.
    /// </para>
    /// <para>
    /// On failure the intermediate result is disposed before the exception propagates, so no
    /// native handle is leaked.
    /// </para>
    /// <para>
    /// When <paramref name="options"/> is omitted (default), no custom compilation options or include
    /// callbacks are applied; shaderc compiles with its built-in defaults.
    /// </para>
    /// </remarks>
    /// <param name="c">The compiler to use.</param>
    /// <param name="source">The GLSL/HLSL source code as a managed string.</param>
    /// <param name="kind">The shader stage.</param>
    /// <param name="inputFileName">Source file name used in diagnostic messages.</param>
    /// <param name="entryPoint">Name of the entry-point function. Defaults to <c>"main"</c>.</param>
    /// <param name="options">
    /// Compilation options, or <see langword="default"/> to use shaderc built-in defaults with no
    /// include callbacks.
    /// </param>
    /// <returns>
    /// A <see cref="CompilationResult"/> with <see cref="CompilationStatus.Success"/> containing
    /// the compiled SPIR-V bytes.
    /// </returns>
    /// <exception cref="ShaderException">Thrown on any compilation failure.</exception>
    public static CompilationResult Compile(
        this in Compiler c,
        string source,
        ShaderKind kind,
        string inputFileName,
        string entryPoint = "main",
        CompileOptions options = default
    )
    {
        using var writer = new BespokeUtf8Encoder(
            source,
            inputFileName,
            entryPoint,
            out var source8,
            out var inputFileName8,
            out var entryPoint8
        );
        return c.Compile(source8, kind, inputFileName8, entryPoint8, options);
    }

    /// <summary>
    /// Reads the file at <paramref name="path"/> and compiles its GLSL/HLSL content to binary SPIR-V,
    /// throwing a typed <see cref="ShaderException"/> on failure.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The file is read with <see cref="File.ReadAllBytes(string)"/> and passed as raw bytes to the
    /// span-based <see cref="Compile(in Compiler, ReadOnlySpan{byte}, ShaderKind, RefString8, RefString8, CompileOptions)"/> overload. <paramref name="path"/> is passed as the
    /// <c>inputFileName</c>, so when include support is enabled by passing <paramref name="options"/>
    /// configured with <see cref="CompileOptions.SetDefaultIncludeCallbacks"/>, relative <c>#include</c>
    /// paths anchor against it. With the default <paramref name="options"/> shaderc has no include
    /// support, so a source containing <c>#include</c> fails to compile.
    /// </para>
    /// <para>
    /// On success the returned <see cref="CompilationResult"/> is owned by the caller and must be
    /// disposed. While the result is undisposed the raw SPIR-V bytes are accessible via
    /// <see cref="CompilationResult.GetBytes()"/>.
    /// </para>
    /// <para>
    /// On failure the intermediate result is disposed before the exception propagates, so no
    /// native handle is leaked.
    /// </para>
    /// <para>
    /// When <paramref name="options"/> is omitted (default), no custom compilation options or include
    /// callbacks are applied; shaderc compiles with its built-in defaults.
    /// </para>
    /// </remarks>
    /// <param name="c">The compiler to use.</param>
    /// <param name="path">Path to the shader source file.</param>
    /// <param name="kind">The shader stage.</param>
    /// <param name="entryPoint">Name of the entry-point function. Defaults to <c>"main"</c>.</param>
    /// <param name="options">
    /// Compilation options, or <see langword="default"/> to use shaderc built-in defaults with no
    /// include callbacks.
    /// </param>
    /// <returns>
    /// A <see cref="CompilationResult"/> with <see cref="CompilationStatus.Success"/> containing
    /// the compiled SPIR-V bytes.
    /// </returns>
    /// <exception cref="ShaderException">Thrown on any compilation failure.</exception>
    /// <exception cref="System.IO.FileNotFoundException">Thrown if no file exists at <paramref name="path"/>.</exception>
    /// <exception cref="System.IO.IOException">Thrown if the file at <paramref name="path"/> cannot be read.</exception>
    public static CompilationResult CompileFile(
        this in Compiler c,
        string path,
        ShaderKind kind,
        string entryPoint = "main",
        CompileOptions options = default
    )
    {
        using var writer = new BespokeUtf8Encoder(path, entryPoint, out var path8, out var entryPoint8);
        var sourceUtf8 = File.ReadAllBytes(path).AsSpan(); // wip: improve allocation strategy
        return c.Compile(sourceUtf8, kind, path8, entryPoint8, options);
    }

    /// <summary>
    /// Assembles SPIR-V text assembly in <paramref name="sourceAssembly"/> into binary SPIR-V,
    /// throwing a typed <see cref="ShaderException"/> on failure.
    /// </summary>
    /// <remarks>
    /// <para>
    /// On success the returned <see cref="CompilationResult"/> is owned by the caller and must be
    /// disposed. While the result is undisposed the raw SPIR-V bytes are accessible via
    /// <see cref="CompilationResult.GetBytes()"/>.
    /// </para>
    /// <para>
    /// On failure the intermediate result is disposed before the exception propagates, so no
    /// native handle is leaked.
    /// </para>
    /// </remarks>
    /// <param name="c">The compiler to use.</param>
    /// <param name="sourceAssembly">The SPIR-V text assembly encoded as UTF-8.</param>
    /// <param name="options">Compilation options.</param>
    /// <returns>
    /// A <see cref="CompilationResult"/> with <see cref="CompilationStatus.Success"/> containing
    /// the assembled SPIR-V bytes.
    /// </returns>
    /// <exception cref="ShaderException">Thrown on any assembly failure.</exception>
    public static CompilationResult Assemble(
        this in Compiler c,
        ReadOnlySpan<byte> sourceAssembly,
        CompileOptions options
    )
    {
        return c.AssembleIntoSpv(sourceAssembly, options).ThrowIfFailedOrDispose();
    }
}
