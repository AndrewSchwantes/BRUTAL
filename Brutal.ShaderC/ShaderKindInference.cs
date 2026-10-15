namespace Brutal.ShaderCApi;

/// <summary>
///     Contains methods for inferring shader kind from file paths.
/// </summary>
public static partial class ShaderC
{
    /// <summary>
    ///     Attempts to infer the shader kind from a file path's extension.
    /// </summary>
    /// <param name="path">The file path to infer the shader kind from.</param>
    /// <param name="kind">
    ///     When this method returns <see langword="true"/>, the inferred shader kind. When it returns
    ///     <see langword="false"/> it is set to <see langword="default"/>, which is a real stage
    ///     (<see cref="ShaderKind.VertexShader"/>), so callers must branch on the return value, not on
    ///     <paramref name="kind"/>.
    /// </param>
    /// <returns>True if the extension was recognized, false otherwise.</returns>
    public static bool TryInferShaderKind(ReadOnlySpan<char> path, out ShaderKind kind)
    {
        // Path.GetExtension returns the extension including the leading '.', and ignores dots that
        // appear in directory names. It is empty when there is no extension (including a trailing '.').
        ReadOnlySpan<char> extension = Path.GetExtension(path);
        if (extension.Length <= 1)
        {
            kind = default;
            return false;
        }

        extension = extension[1..]; // drop the leading '.'

        // Lowercase into a small stack buffer; an extension longer than this is not one we recognise.
        Span<char> lowercased = stackalloc char[16];
        if (extension.Length > lowercased.Length)
        {
            kind = default;
            return false;
        }

        int written = extension.ToLowerInvariant(lowercased);
        ReadOnlySpan<char> lower = lowercased[..written];

        // Single match table: a recognised extension yields its kind, anything else yields null so the
        // caller can distinguish "no match" from the (zero-valued) VertexShader default.
        ShaderKind? match = lower switch
        {
            "vert" => ShaderKind.VertexShader,
            "frag" => ShaderKind.FragmentShader,
            "comp" => ShaderKind.ComputeShader,
            "geom" => ShaderKind.GeometryShader,
            "tesc" => ShaderKind.TessControlShader,
            "tese" => ShaderKind.TessEvaluationShader,
            "mesh" => ShaderKind.MeshShader,
            "task" => ShaderKind.TaskShader,
            "rgen" => ShaderKind.RayGenShader,
            "rmiss" => ShaderKind.MissShader,
            "rchit" => ShaderKind.ClosestHitShader,
            "rahit" => ShaderKind.AnyHitShader,
            "rint" => ShaderKind.IntersectionShader,
            "rcall" => ShaderKind.CallableShader,
            _ => null,
        };

        kind = match ?? default;
        return match.HasValue;
    }

    /// <summary>
    ///     Infers the shader kind from a file path's extension.
    /// </summary>
    /// <param name="path">The file path to infer the shader kind from.</param>
    /// <returns>The inferred shader kind.</returns>
    /// <exception cref="ArgumentException">Thrown if the extension is not recognized.</exception>
    public static ShaderKind InferShaderKind(ReadOnlySpan<char> path)
    {
        if (!TryInferShaderKind(path, out var kind))
        {
            throw new ArgumentException($"Could not infer ShaderKind from path '{path}'.", nameof(path));
        }

        return kind;
    }
}
