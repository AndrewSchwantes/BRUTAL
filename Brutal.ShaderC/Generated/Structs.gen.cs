using System.Runtime.InteropServices.Marshalling;
using Brutal.Interop;

namespace Brutal.ShaderCApi
{
    [InteropHandle]
    public readonly unsafe partial struct Compiler : IDisposable
    {
        public void Dispose() => ShaderC.PInvoke.shaderc_compiler_release(this);

        public CompilationResult CompileIntoSpv(
            ReadOnlySpan<byte> sourceText,
            ShaderKind shaderKind,
            RefString8 inputFileName,
            RefString8 entryPointName,
            CompileOptions additionalOptions
        )
        {
            fixed (byte* sourceTextFixed = sourceText)
            {
                fixed (byte* inputFileNameFixed = inputFileName)
                {
                    fixed (byte* entryPointNameFixed = entryPointName)
                    {
                        return ShaderC.PInvoke.shaderc_compile_into_spv(
                            this,
                            sourceTextFixed,
                            ByteSize.Of(sourceText),
                            shaderKind,
                            inputFileNameFixed,
                            entryPointNameFixed,
                            additionalOptions
                        );
                    }
                }
            }
        }

        public CompilationResult CompileIntoSpvAssembly(
            ReadOnlySpan<byte> sourceText,
            ShaderKind shaderKind,
            RefString8 inputFileName,
            RefString8 entryPointName,
            CompileOptions additionalOptions
        )
        {
            fixed (byte* sourceTextFixed = sourceText)
            {
                fixed (byte* inputFileNameFixed = inputFileName)
                {
                    fixed (byte* entryPointNameFixed = entryPointName)
                    {
                        return ShaderC.PInvoke.shaderc_compile_into_spv_assembly(
                            this,
                            sourceTextFixed,
                            ByteSize.Of(sourceText),
                            shaderKind,
                            inputFileNameFixed,
                            entryPointNameFixed,
                            additionalOptions
                        );
                    }
                }
            }
        }

        public CompilationResult CompileIntoPreprocessedText(
            ReadOnlySpan<byte> sourceText,
            ShaderKind shaderKind,
            RefString8 inputFileName,
            RefString8 entryPointName,
            CompileOptions additionalOptions
        )
        {
            fixed (byte* sourceTextFixed = sourceText)
            {
                fixed (byte* inputFileNameFixed = inputFileName)
                {
                    fixed (byte* entryPointNameFixed = entryPointName)
                    {
                        return ShaderC.PInvoke.shaderc_compile_into_preprocessed_text(
                            this,
                            sourceTextFixed,
                            ByteSize.Of(sourceText),
                            shaderKind,
                            inputFileNameFixed,
                            entryPointNameFixed,
                            additionalOptions
                        );
                    }
                }
            }
        }

        public CompilationResult AssembleIntoSpv(ReadOnlySpan<byte> sourceAssembly, CompileOptions additionalOptions)
        {
            fixed (byte* sourceAssemblyFixed = sourceAssembly)
            {
                return ShaderC.PInvoke.shaderc_assemble_into_spv(
                    this,
                    sourceAssemblyFixed,
                    ByteSize.Of(sourceAssembly),
                    additionalOptions
                );
            }
        }
    }

    [InteropHandle]
    public readonly unsafe partial struct CompileOptions : IDisposable
    {
        public CompileOptions Clone() => ShaderC.PInvoke.shaderc_compile_options_clone(this);

        public void Dispose() => ShaderC.PInvoke.shaderc_compile_options_release(this);

        public void AddMacroDefinition(ReadOnlySpan<byte> name, ReadOnlySpan<byte> value)
        {
            fixed (byte* nameFixed = name)
            {
                fixed (byte* valueFixed = value)
                {
                    ShaderC.PInvoke.shaderc_compile_options_add_macro_definition(
                        this,
                        nameFixed,
                        ByteSize.Of(name),
                        valueFixed,
                        ByteSize.Of(value)
                    );
                }
            }
        }

        public void SetSourceLanguage(SourceLanguage lang) =>
            ShaderC.PInvoke.shaderc_compile_options_set_source_language(this, lang);

        public void SetGenerateDebugInfo() => ShaderC.PInvoke.shaderc_compile_options_set_generate_debug_info(this);

        public void SetEmitNonsemanticShaderDebugInfo() =>
            ShaderC.PInvoke.shaderc_compile_options_set_emit_nonsemantic_shader_debug_info(this);

        public void SetOptimizationLevel(OptimizationLevel level) =>
            ShaderC.PInvoke.shaderc_compile_options_set_optimization_level(this, level);

        public void SetForcedVersionProfile(int version, Profile profile) =>
            ShaderC.PInvoke.shaderc_compile_options_set_forced_version_profile(this, version, profile);

        public void SetIncludeCallbacks(
            IncludeResolveFn resolver,
            IncludeResultReleaseFn resultReleaser,
            void* userData
        ) => ShaderC.PInvoke.shaderc_compile_options_set_include_callbacks(this, resolver, resultReleaser, userData);

        public void SetSuppressWarnings() => ShaderC.PInvoke.shaderc_compile_options_set_suppress_warnings(this);

        public void SetTargetEnv(TargetEnv target, uint version) =>
            ShaderC.PInvoke.shaderc_compile_options_set_target_env(this, target, version);

        public void SetTargetSpirv(SpirvVersion version) =>
            ShaderC.PInvoke.shaderc_compile_options_set_target_spirv(this, version);

        public void SetWarningsAsErrors() => ShaderC.PInvoke.shaderc_compile_options_set_warnings_as_errors(this);

        public void SetLimit(Limit limit, int value) =>
            ShaderC.PInvoke.shaderc_compile_options_set_limit(this, limit, value);

        public void SetAutoBindUniforms(Bool8 autoBind) =>
            ShaderC.PInvoke.shaderc_compile_options_set_auto_bind_uniforms(this, autoBind);

        public void SetAutoCombinedImageSampler(Bool8 upgrade) =>
            ShaderC.PInvoke.shaderc_compile_options_set_auto_combined_image_sampler(this, upgrade);

        public void SetHlslIoMapping(Bool8 hlslIomap) =>
            ShaderC.PInvoke.shaderc_compile_options_set_hlsl_io_mapping(this, hlslIomap);

        public void SetHlslOffsets(Bool8 hlslOffsets) =>
            ShaderC.PInvoke.shaderc_compile_options_set_hlsl_offsets(this, hlslOffsets);

        public void SetBindingBase(UniformKind kind, uint @base) =>
            ShaderC.PInvoke.shaderc_compile_options_set_binding_base(this, kind, @base);

        public void SetBindingBaseForStage(ShaderKind shaderKind, UniformKind kind, uint @base) =>
            ShaderC.PInvoke.shaderc_compile_options_set_binding_base_for_stage(this, shaderKind, kind, @base);

        public void SetPreserveBindings(Bool8 preserveBindings) =>
            ShaderC.PInvoke.shaderc_compile_options_set_preserve_bindings(this, preserveBindings);

        public void SetMaxIdBound(uint maxIdBound) =>
            ShaderC.PInvoke.shaderc_compile_options_set_max_id_bound(this, maxIdBound);

        public void SetAutoMapLocations(Bool8 autoMap) =>
            ShaderC.PInvoke.shaderc_compile_options_set_auto_map_locations(this, autoMap);

        public void SetHlslRegisterSetAndBindingForStage(
            ShaderKind shaderKind,
            RefString8 reg,
            RefString8 set,
            RefString8 binding
        )
        {
            fixed (byte* regFixed = reg)
            {
                fixed (byte* setFixed = set)
                {
                    fixed (byte* bindingFixed = binding)
                    {
                        ShaderC.PInvoke.shaderc_compile_options_set_hlsl_register_set_and_binding_for_stage(
                            this,
                            shaderKind,
                            regFixed,
                            setFixed,
                            bindingFixed
                        );
                    }
                }
            }
        }

        public void SetHlslRegisterSetAndBinding(RefString8 reg, RefString8 set, RefString8 binding)
        {
            fixed (byte* regFixed = reg)
            {
                fixed (byte* setFixed = set)
                {
                    fixed (byte* bindingFixed = binding)
                    {
                        ShaderC.PInvoke.shaderc_compile_options_set_hlsl_register_set_and_binding(
                            this,
                            regFixed,
                            setFixed,
                            bindingFixed
                        );
                    }
                }
            }
        }

        public void SetHlslFunctionality1(Bool8 enable) =>
            ShaderC.PInvoke.shaderc_compile_options_set_hlsl_functionality1(this, enable);

        public void SetHlsl16BitTypes(Bool8 enable) =>
            ShaderC.PInvoke.shaderc_compile_options_set_hlsl_16bit_types(this, enable);

        public void SetVulkanRulesRelaxed(Bool8 enable) =>
            ShaderC.PInvoke.shaderc_compile_options_set_vulkan_rules_relaxed(this, enable);

        public void SetInvertY(Bool8 enable) => ShaderC.PInvoke.shaderc_compile_options_set_invert_y(this, enable);

        public void SetNanClamp(Bool8 enable) => ShaderC.PInvoke.shaderc_compile_options_set_nan_clamp(this, enable);
    }

    /// An include result.
    public unsafe partial struct IncludeResult
    {
        /// The name of the source file.  The name should be fully resolved
        /// in the sense that it should be a unique name in the context of the
        /// includer.  For example, if the includer maps source names to files in
        /// a filesystem, then this name should be the absolute path of the file.
        /// For a failed inclusion, this string is empty.
        public byte* SourceName;
        public ByteSize64 SourceNameLength;

        /// The text contents of the source file in the normal case.
        /// For a failed inclusion, this contains the error message.
        public byte* Content;
        public ByteSize64 ContentLength;

        /// User data to be passed along with this request.
        public void* UserData;
    }

    [InteropHandle]
    public readonly unsafe partial struct CompilationResult : IDisposable
    {
        public void Dispose() => ShaderC.PInvoke.shaderc_result_release(this);

        public ByteSize64 GetLength() => ShaderC.PInvoke.shaderc_result_get_length(this);

        public ByteSize64 GetNumWarnings() => ShaderC.PInvoke.shaderc_result_get_num_warnings(this);

        public ByteSize64 GetNumErrors() => ShaderC.PInvoke.shaderc_result_get_num_errors(this);

        public CompilationStatus GetCompilationStatus() => ShaderC.PInvoke.shaderc_result_get_compilation_status(this);

        public byte* GetBytes() => ShaderC.PInvoke.shaderc_result_get_bytes(this);

        public string GetErrorMessage() => ShaderC.PInvoke.shaderc_result_get_error_message(this);
    }
}
