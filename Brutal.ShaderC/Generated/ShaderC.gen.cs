using System.Runtime.InteropServices;

namespace Brutal.ShaderCApi
{
    public static partial class ShaderC
    {
        public static unsafe partial class PInvoke
        {
            [LibraryImport("shaderc")]
            public static partial Compiler shaderc_compiler_initialize();

            [LibraryImport("shaderc")]
            public static partial void shaderc_compiler_release(Compiler arg);

            [LibraryImport("shaderc")]
            public static partial CompileOptions shaderc_compile_options_initialize();

            [LibraryImport("shaderc")]
            public static partial CompileOptions shaderc_compile_options_clone(CompileOptions options);

            [LibraryImport("shaderc")]
            public static partial void shaderc_compile_options_release(CompileOptions options);

            [LibraryImport("shaderc")]
            public static partial void shaderc_compile_options_add_macro_definition(
                CompileOptions options,
                byte* name,
                ByteSize64 nameLength,
                byte* value,
                ByteSize64 valueLength
            );

            [LibraryImport("shaderc")]
            public static partial void shaderc_compile_options_set_source_language(
                CompileOptions options,
                SourceLanguage lang
            );

            [LibraryImport("shaderc")]
            public static partial void shaderc_compile_options_set_generate_debug_info(CompileOptions options);

            [LibraryImport("shaderc")]
            public static partial void shaderc_compile_options_set_emit_nonsemantic_shader_debug_info(
                CompileOptions options
            );

            [LibraryImport("shaderc")]
            public static partial void shaderc_compile_options_set_optimization_level(
                CompileOptions options,
                OptimizationLevel level
            );

            [LibraryImport("shaderc")]
            public static partial void shaderc_compile_options_set_forced_version_profile(
                CompileOptions options,
                int version,
                Profile profile
            );

            [LibraryImport("shaderc")]
            public static partial void shaderc_compile_options_set_include_callbacks(
                CompileOptions options,
                IncludeResolveFn resolver,
                IncludeResultReleaseFn resultReleaser,
                void* userData
            );

            [LibraryImport("shaderc")]
            public static partial void shaderc_compile_options_set_suppress_warnings(CompileOptions options);

            [LibraryImport("shaderc")]
            public static partial void shaderc_compile_options_set_target_env(
                CompileOptions options,
                TargetEnv target,
                uint version
            );

            [LibraryImport("shaderc")]
            public static partial void shaderc_compile_options_set_target_spirv(
                CompileOptions options,
                SpirvVersion version
            );

            [LibraryImport("shaderc")]
            public static partial void shaderc_compile_options_set_warnings_as_errors(CompileOptions options);

            [LibraryImport("shaderc")]
            public static partial void shaderc_compile_options_set_limit(
                CompileOptions options,
                Limit limit,
                int value
            );

            [LibraryImport("shaderc")]
            public static partial void shaderc_compile_options_set_auto_bind_uniforms(
                CompileOptions options,
                Bool8 autoBind
            );

            [LibraryImport("shaderc")]
            public static partial void shaderc_compile_options_set_auto_combined_image_sampler(
                CompileOptions options,
                Bool8 upgrade
            );

            [LibraryImport("shaderc")]
            public static partial void shaderc_compile_options_set_hlsl_io_mapping(
                CompileOptions options,
                Bool8 hlslIomap
            );

            [LibraryImport("shaderc")]
            public static partial void shaderc_compile_options_set_hlsl_offsets(
                CompileOptions options,
                Bool8 hlslOffsets
            );

            [LibraryImport("shaderc")]
            public static partial void shaderc_compile_options_set_binding_base(
                CompileOptions options,
                UniformKind kind,
                uint @base
            );

            [LibraryImport("shaderc")]
            public static partial void shaderc_compile_options_set_binding_base_for_stage(
                CompileOptions options,
                ShaderKind shaderKind,
                UniformKind kind,
                uint @base
            );

            [LibraryImport("shaderc")]
            public static partial void shaderc_compile_options_set_preserve_bindings(
                CompileOptions options,
                Bool8 preserveBindings
            );

            [LibraryImport("shaderc")]
            public static partial void shaderc_compile_options_set_max_id_bound(
                CompileOptions options,
                uint maxIdBound
            );

            [LibraryImport("shaderc")]
            public static partial void shaderc_compile_options_set_auto_map_locations(
                CompileOptions options,
                Bool8 autoMap
            );

            [LibraryImport("shaderc")]
            public static partial void shaderc_compile_options_set_hlsl_register_set_and_binding_for_stage(
                CompileOptions options,
                ShaderKind shaderKind,
                byte* reg,
                byte* set,
                byte* binding
            );

            [LibraryImport("shaderc")]
            public static partial void shaderc_compile_options_set_hlsl_register_set_and_binding(
                CompileOptions options,
                byte* reg,
                byte* set,
                byte* binding
            );

            [LibraryImport("shaderc")]
            public static partial void shaderc_compile_options_set_hlsl_functionality1(
                CompileOptions options,
                Bool8 enable
            );

            [LibraryImport("shaderc")]
            public static partial void shaderc_compile_options_set_hlsl_16bit_types(
                CompileOptions options,
                Bool8 enable
            );

            [LibraryImport("shaderc")]
            public static partial void shaderc_compile_options_set_vulkan_rules_relaxed(
                CompileOptions options,
                Bool8 enable
            );

            [LibraryImport("shaderc")]
            public static partial void shaderc_compile_options_set_invert_y(CompileOptions options, Bool8 enable);

            [LibraryImport("shaderc")]
            public static partial void shaderc_compile_options_set_nan_clamp(CompileOptions options, Bool8 enable);

            [LibraryImport("shaderc")]
            public static partial CompilationResult shaderc_compile_into_spv(
                Compiler compiler,
                byte* sourceText,
                ByteSize64 sourceTextSize,
                ShaderKind shaderKind,
                byte* inputFileName,
                byte* entryPointName,
                CompileOptions additionalOptions
            );

            [LibraryImport("shaderc")]
            public static partial CompilationResult shaderc_compile_into_spv_assembly(
                Compiler compiler,
                byte* sourceText,
                ByteSize64 sourceTextSize,
                ShaderKind shaderKind,
                byte* inputFileName,
                byte* entryPointName,
                CompileOptions additionalOptions
            );

            [LibraryImport("shaderc")]
            public static partial CompilationResult shaderc_compile_into_preprocessed_text(
                Compiler compiler,
                byte* sourceText,
                ByteSize64 sourceTextSize,
                ShaderKind shaderKind,
                byte* inputFileName,
                byte* entryPointName,
                CompileOptions additionalOptions
            );

            [LibraryImport("shaderc")]
            public static partial CompilationResult shaderc_assemble_into_spv(
                Compiler compiler,
                byte* sourceAssembly,
                ByteSize64 sourceAssemblySize,
                CompileOptions additionalOptions
            );

            [LibraryImport("shaderc")]
            public static partial void shaderc_result_release(CompilationResult result);

            [LibraryImport("shaderc")]
            public static partial ByteSize64 shaderc_result_get_length(CompilationResult result);

            [LibraryImport("shaderc")]
            public static partial ByteSize64 shaderc_result_get_num_warnings(CompilationResult result);

            [LibraryImport("shaderc")]
            public static partial ByteSize64 shaderc_result_get_num_errors(CompilationResult result);

            [LibraryImport("shaderc")]
            public static partial CompilationStatus shaderc_result_get_compilation_status(CompilationResult arg);

            [LibraryImport("shaderc")]
            public static partial byte* shaderc_result_get_bytes(CompilationResult result);

            [LibraryImport(
                "shaderc",
                StringMarshalling = StringMarshalling.Custom,
                StringMarshallingCustomType = typeof(Brutal.Interop.InteropStringMarshaller)
            )]
            public static partial string shaderc_result_get_error_message(CompilationResult result);

            [LibraryImport("shaderc")]
            public static partial void shaderc_get_spv_version(uint* version, uint* revision);

            [LibraryImport("shaderc")]
            public static partial Bool8 shaderc_parse_version_profile(byte* str, int* version, Profile* profile);
        }
    }
}
