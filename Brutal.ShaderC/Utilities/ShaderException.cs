namespace Brutal.ShaderCApi;

/// <summary>
/// Base class of all shader exceptions. Captures the compiler diagnostic message from a
/// <see cref="CompilationResult"/> at the point of construction.
/// </summary>
/// <remarks>
/// <para>
/// The exception does <b>not</b> take ownership of <paramref name="result"/>; the caller remains
/// responsible for disposing it. <see cref="CompilationResult.GetErrorMessage()"/> is evaluated
/// eagerly during construction, so <paramref name="result"/> must still be valid at the point of
/// <c>throw</c>. The captured message is the raw shaderc diagnostic text.
/// </para>
/// </remarks>
/// <param name="result">
/// The compilation result whose error message is captured. The exception does not take ownership;
/// the caller remains responsible for disposing <paramref name="result"/>. The message is read
/// eagerly at construction time, so the result must still be valid at the point of <c>throw</c>.
/// </param>
public class ShaderException(in CompilationResult result) : Exception(result.GetErrorMessage());

/// <summary>
/// Triggers when the shader stage could not be inferred from the source code.
/// </summary>
public class ShaderInvalidStageException(in CompilationResult result) : ShaderException(result);

/// <summary>
/// Triggers when the shader compilation failed.
/// </summary>
public class ShaderCompilationException(in CompilationResult result) : ShaderException(result);

/// <summary>
/// Triggers when the shader compiler encountered an internal error.
/// </summary>
public class ShaderInternalException(in CompilationResult result) : ShaderException(result);

/// <summary>
/// Triggers when the shader compiler returned a null result object.
/// </summary>
public class ShaderNullResultObjectException(in CompilationResult result) : ShaderException(result);

/// <summary>
/// Triggers when the SPIR-V assembly is invalid.
/// </summary>
public class ShaderInvalidAssemblyException(in CompilationResult result) : ShaderException(result);

/// <summary>
/// Triggers when SPIR-V validation failed (when validation is enabled).
/// </summary>
public class ShaderValidationException(in CompilationResult result) : ShaderException(result);

/// <summary>
/// Triggered when transformation to SPIR-V failed.
/// </summary>
public class ShaderTransformationException(in CompilationResult result) : ShaderException(result);

/// <summary>
/// Triggered when configuration of the shader compiler is invalid.
/// </summary>
public class ShaderConfigurationException(in CompilationResult result) : ShaderException(result);
