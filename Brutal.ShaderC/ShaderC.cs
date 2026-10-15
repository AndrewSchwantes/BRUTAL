using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Brutal.ShaderCApi;

/// <summary>
///     Provides access to the shaderc library for compiling GLSL/HLSL shaders to SPIR-V.
/// </summary>
/// <remarks>
///     <para>
///         The native <c>shaderc_shared</c> library is loaded in the type initializer. If the library is not
///         present in the assembly directory at the time the type is first accessed, the runtime will throw a
///         <see cref="TypeInitializationException"/>, a common deployment pitfall.
///     </para>
///     <para>
///         On macOS the library is loaded as <c>shaderc_shared.1</c> (versioned dylib); all other platforms
///         use <c>shaderc_shared</c>. This platform difference is managed internally.
///     </para>
///     <para>
///         All handles returned by the factory methods (<see cref="CreateCompiler"/>,
///         <see cref="CreateCompilerOptions"/>) wrap native memory and must be disposed when no longer needed.
///         Each handle must be disposed <em>exactly once</em>: disposing twice or copying a handle struct and
///         disposing both copies passes an invalid handle to native code and results in undefined behavior
///         (heap corruption or crash). A structural dispose-guard is tracked upstream against the
///         <c>[InteropHandle]</c> generator.
///     </para>
/// </remarks>
public static partial class ShaderC
{
    private const string DLL = "shaderc";

    static ShaderC()
    {
        var assembly = typeof(ShaderC).Assembly;

        // EXCEPTION: The dll name is different on macOS, so we need to handle that case
        var nativeDll = NativeLibrary.Load(
            OperatingSystem.IsMacOS() ? "shaderc_shared.1" : "shaderc_shared",
            assembly,
            DllImportSearchPath.AssemblyDirectory
        );

        // Set the DllImportResolver for the assembly to resolve the shaderc library
        NativeLibrary.SetDllImportResolver(
            assembly,
            (libraryName, _, _) =>
            {
                // We should only ever be resolving the shaderc library, so we assert that the library name is correct.
                Debug.Assert(libraryName == "shaderc", "Unexpected library name requested for resolution.");

                return nativeDll;
            }
        );
    }

    /// <summary>Creates a new shaderc compiler handle.</summary>
    /// <returns>
    ///     A <see cref="Compiler"/> handle wrapping native memory. The caller is responsible for disposing
    ///     the handle to release native resources.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         The returned handle wraps a native <c>shaderc_compiler_t</c> object. It must be disposed
    ///         when no longer needed; failing to do so leaks native memory. Dispose the handle exactly
    ///         once; a second <c>Dispose</c> call (or disposing two copies of the same struct) passes an
    ///         invalid handle to native code and results in undefined behavior.
    ///     </para>
    ///     <para>
    ///         A <see cref="Compiler"/> handle is <strong>not thread-safe</strong>: do not use the same
    ///         handle concurrently across threads. Create a separate handle per thread if concurrent
    ///         compilation is required.
    ///     </para>
    ///     <para>
    ///         If the underlying native initialization fails, shaderc returns a null handle, which this
    ///         method propagates as-is (an uninitialized <see cref="Compiler"/> struct).
    ///     </para>
    /// </remarks>
    public static Compiler CreateCompiler()
    {
        return PInvoke.shaderc_compiler_initialize();
    }

    /// <summary>Creates a new shaderc compile-options handle.</summary>
    /// <returns>
    ///     A <see cref="CompileOptions"/> handle wrapping native memory. The caller is responsible for
    ///     disposing the handle to release native resources.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         The returned handle wraps a native <c>shaderc_compile_options_t</c> object. It must be
    ///         disposed when no longer needed; failing to do so leaks native memory. Dispose the handle
    ///         exactly once; a second <c>Dispose</c> call (or disposing two copies of the same struct)
    ///         passes an invalid handle to native code and results in undefined behavior.
    ///     </para>
    ///     <para>
    ///         A <see cref="CompileOptions"/> handle is <strong>not thread-safe</strong>: do not mutate
    ///         options from multiple threads simultaneously. The options instance must also outlive any
    ///         compilation that uses it.
    ///     </para>
    ///     <para>
    ///         If the underlying native initialization fails, shaderc returns a null handle, which this
    ///         method propagates as-is (an uninitialized <see cref="CompileOptions"/> struct).
    ///     </para>
    /// </remarks>
    public static CompileOptions CreateCompilerOptions()
    {
        return PInvoke.shaderc_compile_options_initialize();
    }

    /// <summary>Gets the SPIR-V version and revision that shaderc was built against.</summary>
    /// <param name="version">
    ///     Packed SPIR-V version: major version in the upper 16 bits, minor version in the lower 16 bits.
    /// </param>
    /// <param name="revision">SPIR-V revision number.</param>
    public static unsafe void GetSpvVersion(out uint version, out uint revision)
    {
        fixed (uint* v = &version)
        fixed (uint* r = &revision)
        {
            PInvoke.shaderc_get_spv_version(v, r);
        }
    }

    /// <summary>Tries to parse a GLSL version/profile string such as "450core".</summary>
    /// <param name="str">UTF-8 encoded, null-terminated version/profile string.</param>
    /// <param name="version">
    ///     When this method returns <c>true</c>, the parsed GLSL version number (e.g. 450).
    /// </param>
    /// <param name="profile">
    ///     When this method returns <c>true</c>, the parsed profile. Undefined when this method returns
    ///     <c>false</c>.
    /// </param>
    /// <returns><c>true</c> if parsing succeeded; otherwise <c>false</c>.</returns>
    /// <remarks>
    ///     <c>str</c> must be null-terminated: the underlying shaderc API treats the pointer as a C string
    ///     and will overread if no null terminator is present. A <see cref="ReadOnlySpan{T}"/> obtained
    ///     from a string literal or a stack-allocated buffer with an explicit <c>\0</c> satisfies this
    ///     requirement.
    /// </remarks>
    public static unsafe bool TryParseVersionProfile(ReadOnlySpan<byte> str, out int version, out Profile profile)
    {
        fixed (byte* s = str)
        fixed (int* v = &version)
        fixed (Profile* p = &profile)
        {
            return PInvoke.shaderc_parse_version_profile(s, v, p);
        }
    }
}
