namespace Brutal.ShaderCApi
{
    public enum TargetEnv : uint
    {
        Vulkan, // SPIR-V under Vulkan semantics
        Opengl, // SPIR-V under OpenGL semantics
        OpenglCompat, // SPIR-V under OpenGL semantics, including compatibility profile functions

        [Obsolete("Deprecated, SPIR-V under WebGPU semantics")]
        Webgpu, // Deprecated, SPIR-V under WebGPU semantics
        Default = Vulkan,
    }

    public enum EnvVersion : uint
    {
        /// For Vulkan, use Vulkan's mapping of version numbers to integers.
        /// See vulkan.h
        Vulkan_1_0 = ((1u << 22)),

        /// For Vulkan, use Vulkan's mapping of version numbers to integers.
        /// See vulkan.h
        Vulkan_1_1 = ((1u << 22) | (1 << 12)),

        /// For Vulkan, use Vulkan's mapping of version numbers to integers.
        /// See vulkan.h
        Vulkan_1_2 = ((1u << 22) | (2 << 12)),

        /// For Vulkan, use Vulkan's mapping of version numbers to integers.
        /// See vulkan.h
        Vulkan_1_3 = ((1u << 22) | (3 << 12)),

        /// For Vulkan, use Vulkan's mapping of version numbers to integers.
        /// See vulkan.h
        Vulkan_1_4 = ((1u << 22) | (4 << 12)),

        /// For OpenGL, use the number from #version in shaders.
        /// TODO(dneto): Currently no difference between OpenGL 4.5 and 4.6.
        /// See glslang/Standalone/Standalone.cpp
        /// TODO(dneto): Glslang doesn't accept a OpenGL client version of 460.
        Opengl_4_5 = 450,

        [Obsolete("Deprecated, WebGPU env never defined versions")]
        Webgpu, // Deprecated, WebGPU env never defined versions
    }

    /// The known versions of SPIR-V.
    public enum SpirvVersion : uint
    {
        /// Use the values used for word 1 of a SPIR-V binary:
        /// - bits 24 to 31: zero
        /// - bits 16 to 23: major version number
        /// - bits 8 to 15: minor version number
        /// - bits 0 to 7: zero
        _1_0 = 0x010000u,

        /// Use the values used for word 1 of a SPIR-V binary:
        /// - bits 24 to 31: zero
        /// - bits 16 to 23: major version number
        /// - bits 8 to 15: minor version number
        /// - bits 0 to 7: zero
        _1_1 = 0x010100u,

        /// Use the values used for word 1 of a SPIR-V binary:
        /// - bits 24 to 31: zero
        /// - bits 16 to 23: major version number
        /// - bits 8 to 15: minor version number
        /// - bits 0 to 7: zero
        _1_2 = 0x010200u,

        /// Use the values used for word 1 of a SPIR-V binary:
        /// - bits 24 to 31: zero
        /// - bits 16 to 23: major version number
        /// - bits 8 to 15: minor version number
        /// - bits 0 to 7: zero
        _1_3 = 0x010300u,

        /// Use the values used for word 1 of a SPIR-V binary:
        /// - bits 24 to 31: zero
        /// - bits 16 to 23: major version number
        /// - bits 8 to 15: minor version number
        /// - bits 0 to 7: zero
        _1_4 = 0x010400u,

        /// Use the values used for word 1 of a SPIR-V binary:
        /// - bits 24 to 31: zero
        /// - bits 16 to 23: major version number
        /// - bits 8 to 15: minor version number
        /// - bits 0 to 7: zero
        _1_5 = 0x010500u,

        /// Use the values used for word 1 of a SPIR-V binary:
        /// - bits 24 to 31: zero
        /// - bits 16 to 23: major version number
        /// - bits 8 to 15: minor version number
        /// - bits 0 to 7: zero
        _1_6 = 0x010600u,
    }

    /// Indicate the status of a compilation.
    public enum CompilationStatus : uint
    {
        Success = 0,
        InvalidStage = 1, // error stage deduction
        CompilationError = 2,
        InternalError = 3, // unexpected failure
        NullResultObject = 4,
        InvalidAssembly = 5,
        ValidationError = 6,
        TransformationError = 7,
        ConfigurationError = 8,
    }

    /// Source language kind.
    public enum SourceLanguage : uint
    {
        Glsl,
        Hlsl,
    }

    public enum ShaderKind : uint
    {
        /// Forced shader kinds. These shader kinds force the compiler to compile the
        /// source code as the specified kind of shader.
        VertexShader,

        /// Forced shader kinds. These shader kinds force the compiler to compile the
        /// source code as the specified kind of shader.
        FragmentShader,

        /// Forced shader kinds. These shader kinds force the compiler to compile the
        /// source code as the specified kind of shader.
        ComputeShader,

        /// Forced shader kinds. These shader kinds force the compiler to compile the
        /// source code as the specified kind of shader.
        GeometryShader,

        /// Forced shader kinds. These shader kinds force the compiler to compile the
        /// source code as the specified kind of shader.
        TessControlShader,

        /// Forced shader kinds. These shader kinds force the compiler to compile the
        /// source code as the specified kind of shader.
        TessEvaluationShader,

        /// Forced shader kinds. These shader kinds force the compiler to compile the
        /// source code as the specified kind of shader.
        GlslVertexShader = VertexShader,

        /// Forced shader kinds. These shader kinds force the compiler to compile the
        /// source code as the specified kind of shader.
        GlslFragmentShader = FragmentShader,

        /// Forced shader kinds. These shader kinds force the compiler to compile the
        /// source code as the specified kind of shader.
        GlslComputeShader = ComputeShader,

        /// Forced shader kinds. These shader kinds force the compiler to compile the
        /// source code as the specified kind of shader.
        GlslGeometryShader = GeometryShader,

        /// Forced shader kinds. These shader kinds force the compiler to compile the
        /// source code as the specified kind of shader.
        GlslTessControlShader = TessControlShader,

        /// Forced shader kinds. These shader kinds force the compiler to compile the
        /// source code as the specified kind of shader.
        GlslTessEvaluationShader = TessEvaluationShader,

        /// Deduce the shader kind from #pragma annotation in the source code. Compiler
        /// will emit error if #pragma annotation is not found.
        GlslInferFromSource,

        /// Default shader kinds. Compiler will fall back to compile the source code as
        /// the specified kind of shader when #pragma annotation is not found in the
        /// source code.
        GlslDefaultVertexShader,

        /// Default shader kinds. Compiler will fall back to compile the source code as
        /// the specified kind of shader when #pragma annotation is not found in the
        /// source code.
        GlslDefaultFragmentShader,

        /// Default shader kinds. Compiler will fall back to compile the source code as
        /// the specified kind of shader when #pragma annotation is not found in the
        /// source code.
        GlslDefaultComputeShader,

        /// Default shader kinds. Compiler will fall back to compile the source code as
        /// the specified kind of shader when #pragma annotation is not found in the
        /// source code.
        GlslDefaultGeometryShader,

        /// Default shader kinds. Compiler will fall back to compile the source code as
        /// the specified kind of shader when #pragma annotation is not found in the
        /// source code.
        GlslDefaultTessControlShader,

        /// Default shader kinds. Compiler will fall back to compile the source code as
        /// the specified kind of shader when #pragma annotation is not found in the
        /// source code.
        GlslDefaultTessEvaluationShader,

        /// Default shader kinds. Compiler will fall back to compile the source code as
        /// the specified kind of shader when #pragma annotation is not found in the
        /// source code.
        SpirvAssembly,

        /// Default shader kinds. Compiler will fall back to compile the source code as
        /// the specified kind of shader when #pragma annotation is not found in the
        /// source code.
        RayGenShader,

        /// Default shader kinds. Compiler will fall back to compile the source code as
        /// the specified kind of shader when #pragma annotation is not found in the
        /// source code.
        AnyHitShader,

        /// Default shader kinds. Compiler will fall back to compile the source code as
        /// the specified kind of shader when #pragma annotation is not found in the
        /// source code.
        ClosestHitShader,

        /// Default shader kinds. Compiler will fall back to compile the source code as
        /// the specified kind of shader when #pragma annotation is not found in the
        /// source code.
        MissShader,

        /// Default shader kinds. Compiler will fall back to compile the source code as
        /// the specified kind of shader when #pragma annotation is not found in the
        /// source code.
        IntersectionShader,

        /// Default shader kinds. Compiler will fall back to compile the source code as
        /// the specified kind of shader when #pragma annotation is not found in the
        /// source code.
        CallableShader,

        /// Default shader kinds. Compiler will fall back to compile the source code as
        /// the specified kind of shader when #pragma annotation is not found in the
        /// source code.
        GlslRayGenShader = RayGenShader,

        /// Default shader kinds. Compiler will fall back to compile the source code as
        /// the specified kind of shader when #pragma annotation is not found in the
        /// source code.
        GlslAnyHitShader = AnyHitShader,

        /// Default shader kinds. Compiler will fall back to compile the source code as
        /// the specified kind of shader when #pragma annotation is not found in the
        /// source code.
        GlslClosestHitShader = ClosestHitShader,

        /// Default shader kinds. Compiler will fall back to compile the source code as
        /// the specified kind of shader when #pragma annotation is not found in the
        /// source code.
        GlslMissShader = MissShader,

        /// Default shader kinds. Compiler will fall back to compile the source code as
        /// the specified kind of shader when #pragma annotation is not found in the
        /// source code.
        GlslIntersectionShader = IntersectionShader,

        /// Default shader kinds. Compiler will fall back to compile the source code as
        /// the specified kind of shader when #pragma annotation is not found in the
        /// source code.
        GlslCallableShader = CallableShader,

        /// Default shader kinds. Compiler will fall back to compile the source code as
        /// the specified kind of shader when #pragma annotation is not found in the
        /// source code.
        GlslDefaultRayGenShader,

        /// Default shader kinds. Compiler will fall back to compile the source code as
        /// the specified kind of shader when #pragma annotation is not found in the
        /// source code.
        GlslDefaultAnyHitShader,

        /// Default shader kinds. Compiler will fall back to compile the source code as
        /// the specified kind of shader when #pragma annotation is not found in the
        /// source code.
        GlslDefaultClosestHitShader,

        /// Default shader kinds. Compiler will fall back to compile the source code as
        /// the specified kind of shader when #pragma annotation is not found in the
        /// source code.
        GlslDefaultMissShader,

        /// Default shader kinds. Compiler will fall back to compile the source code as
        /// the specified kind of shader when #pragma annotation is not found in the
        /// source code.
        GlslDefaultIntersectionShader,

        /// Default shader kinds. Compiler will fall back to compile the source code as
        /// the specified kind of shader when #pragma annotation is not found in the
        /// source code.
        GlslDefaultCallableShader,

        /// Default shader kinds. Compiler will fall back to compile the source code as
        /// the specified kind of shader when #pragma annotation is not found in the
        /// source code.
        TaskShader,

        /// Default shader kinds. Compiler will fall back to compile the source code as
        /// the specified kind of shader when #pragma annotation is not found in the
        /// source code.
        MeshShader,

        /// Default shader kinds. Compiler will fall back to compile the source code as
        /// the specified kind of shader when #pragma annotation is not found in the
        /// source code.
        GlslTaskShader = TaskShader,

        /// Default shader kinds. Compiler will fall back to compile the source code as
        /// the specified kind of shader when #pragma annotation is not found in the
        /// source code.
        GlslMeshShader = MeshShader,

        /// Default shader kinds. Compiler will fall back to compile the source code as
        /// the specified kind of shader when #pragma annotation is not found in the
        /// source code.
        GlslDefaultTaskShader,

        /// Default shader kinds. Compiler will fall back to compile the source code as
        /// the specified kind of shader when #pragma annotation is not found in the
        /// source code.
        GlslDefaultMeshShader,
    }

    public enum Profile : uint
    {
        None, // Used if and only if GLSL version did not specify profiles.
        Core,
        Compatibility, // Disabled. This generates an error
        Es,
    }

    /// Optimization level.
    public enum OptimizationLevel : uint
    {
        Zero, // no optimization
        Size, // optimize towards reducing code size
        Performance, // optimize towards performance
    }

    /// Resource limits.
    public enum Limit : uint
    {
        MaxLights,
        MaxClipPlanes,
        MaxTextureUnits,
        MaxTextureCoords,
        MaxVertexAttribs,
        MaxVertexUniformComponents,
        MaxVaryingFloats,
        MaxVertexTextureImageUnits,
        MaxCombinedTextureImageUnits,
        MaxTextureImageUnits,
        MaxFragmentUniformComponents,
        MaxDrawBuffers,
        MaxVertexUniformVectors,
        MaxVaryingVectors,
        MaxFragmentUniformVectors,
        MaxVertexOutputVectors,
        MaxFragmentInputVectors,
        MinProgramTexelOffset,
        MaxProgramTexelOffset,
        MaxClipDistances,
        MaxComputeWorkGroupCountX,
        MaxComputeWorkGroupCountY,
        MaxComputeWorkGroupCountZ,
        MaxComputeWorkGroupSizeX,
        MaxComputeWorkGroupSizeY,
        MaxComputeWorkGroupSizeZ,
        MaxComputeUniformComponents,
        MaxComputeTextureImageUnits,
        MaxComputeImageUniforms,
        MaxComputeAtomicCounters,
        MaxComputeAtomicCounterBuffers,
        MaxVaryingComponents,
        MaxVertexOutputComponents,
        MaxGeometryInputComponents,
        MaxGeometryOutputComponents,
        MaxFragmentInputComponents,
        MaxImageUnits,
        MaxCombinedImageUnitsAndFragmentOutputs,
        MaxCombinedShaderOutputResources,
        MaxImageSamples,
        MaxVertexImageUniforms,
        MaxTessControlImageUniforms,
        MaxTessEvaluationImageUniforms,
        MaxGeometryImageUniforms,
        MaxFragmentImageUniforms,
        MaxCombinedImageUniforms,
        MaxGeometryTextureImageUnits,
        MaxGeometryOutputVertices,
        MaxGeometryTotalOutputComponents,
        MaxGeometryUniformComponents,
        MaxGeometryVaryingComponents,
        MaxTessControlInputComponents,
        MaxTessControlOutputComponents,
        MaxTessControlTextureImageUnits,
        MaxTessControlUniformComponents,
        MaxTessControlTotalOutputComponents,
        MaxTessEvaluationInputComponents,
        MaxTessEvaluationOutputComponents,
        MaxTessEvaluationTextureImageUnits,
        MaxTessEvaluationUniformComponents,
        MaxTessPatchComponents,
        MaxPatchVertices,
        MaxTessGenLevel,
        MaxViewports,
        MaxVertexAtomicCounters,
        MaxTessControlAtomicCounters,
        MaxTessEvaluationAtomicCounters,
        MaxGeometryAtomicCounters,
        MaxFragmentAtomicCounters,
        MaxCombinedAtomicCounters,
        MaxAtomicCounterBindings,
        MaxVertexAtomicCounterBuffers,
        MaxTessControlAtomicCounterBuffers,
        MaxTessEvaluationAtomicCounterBuffers,
        MaxGeometryAtomicCounterBuffers,
        MaxFragmentAtomicCounterBuffers,
        MaxCombinedAtomicCounterBuffers,
        MaxAtomicCounterBufferSize,
        MaxTransformFeedbackBuffers,
        MaxTransformFeedbackInterleavedComponents,
        MaxCullDistances,
        MaxCombinedClipAndCullDistances,
        MaxSamples,
        MaxMeshOutputVerticesNv,
        MaxMeshOutputPrimitivesNv,
        MaxMeshWorkGroupSizeXNv,
        MaxMeshWorkGroupSizeYNv,
        MaxMeshWorkGroupSizeZNv,
        MaxTaskWorkGroupSizeXNv,
        MaxTaskWorkGroupSizeYNv,
        MaxTaskWorkGroupSizeZNv,
        MaxMeshViewCountNv,
        MaxMeshOutputVerticesExt,
        MaxMeshOutputPrimitivesExt,
        MaxMeshWorkGroupSizeXExt,
        MaxMeshWorkGroupSizeYExt,
        MaxMeshWorkGroupSizeZExt,
        MaxTaskWorkGroupSizeXExt,
        MaxTaskWorkGroupSizeYExt,
        MaxTaskWorkGroupSizeZExt,
        MaxMeshViewCountExt,
        MaxDualSourceDrawBuffersExt,
    }

    /// Uniform resource kinds.
    /// In Vulkan, uniform resources are bound to the pipeline via descriptors
    /// with numbered bindings and sets.
    public enum UniformKind : uint
    {
        /// Image and image buffer.
        Image,

        /// Pure sampler.
        Sampler,

        /// Sampled texture in GLSL, and Shader Resource View in HLSL.
        Texture,

        /// Uniform Buffer Object (UBO) in GLSL.  Cbuffer in HLSL.
        Buffer,

        /// Shader Storage Buffer Object (SSBO) in GLSL.
        StorageBuffer,

        /// Unordered Access View, in HLSL.  (Writable storage image or storage
        /// buffer.)
        UnorderedAccessView,
    }

    /// The kinds of include requests.
    public enum IncludeType : uint
    {
        Relative, // E.g. #include "source"
        Standard, // E.g. #include <source>
    }
}
