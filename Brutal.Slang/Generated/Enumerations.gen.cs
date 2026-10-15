namespace Brutal.SlangApi
{
    public enum CompilerOptionName
    {
        MacroDefine = 0, // stringValue0: macro name;  stringValue1: macro value
        DepFile = 1,
        EntryPointName = 2,
        Specialize = 3,
        Help = 4,
        HelpStyle = 5,
        Include = 6, // stringValue: additional include path.
        Language = 7,
        MatrixLayoutColumn = 8, // bool
        MatrixLayoutRow = 9, // bool
        ZeroInitialize = 10, // bool
        IgnoreCapabilities = 11, // bool
        RestrictiveCapabilityCheck = 12, // bool
        ModuleName = 13, // stringValue0: module name.
        Output = 14,
        Profile = 15, // intValue0: profile
        Stage = 16, // intValue0: stage
        Target = 17, // intValue0: CodeGenTarget
        Version = 18,
        WarningsAsErrors = 19, // stringValue0: "all" or comma-separated list ofwarning codes or names.
        DisableWarnings = 20, // stringValue0: comma separated list of warning codes or names.
        EnableWarning = 21, // stringValue0: warning code or name.
        DisableWarning = 22, // stringValue0: warning code or name.
        DumpWarningDiagnostics = 23,
        InputFilesRemain = 24,
        EmitIR = 25, // bool
        ReportDownstreamTime = 26, // bool
        ReportPerfBenchmark = 27, // bool
        ReportCheckpointIntermediates = 28, // bool
        SkipSpirvValidation = 29, // bool
        SourceEmbedStyle = 30,
        SourceEmbedName = 31,
        SourceEmbedLanguage = 32,
        DisableShortCircuit = 33, // bool
        MinimumSlangOptimization = 34, // bool
        DisableNonEssentialValidations = 35, // bool
        DisableSourceMap = 36, // bool
        UnscopedEnum = 37, // bool
        PreserveParameters = 38, // bool: preserve all resource parameters in the output code.
        Capability = 39, // intValue0: CapabilityName
        DefaultImageFormatUnknown = 40, // bool
        DisableDynamicDispatch = 41, // bool
        DisableSpecialization = 42, // bool
        FloatingPointMode = 43, // intValue0: FloatingPointMode
        DebugInformation = 44, // intValue0: DebugInfoLevel
        LineDirectiveMode = 45,
        Optimization = 46, // intValue0: OptimizationLevel
        Obfuscate = 47, // bool
        VulkanBindShift = 48, // intValue0 (higher 8 bits): kind; intValue0(lower bits): set;intValue1: shift
        VulkanBindGlobals = 49, // intValue0: index; intValue1: set
        VulkanInvertY = 50, // bool
        VulkanUseDxPositionW = 51, // bool
        VulkanUseEntryPointName = 52, // bool
        VulkanUseGlLayout = 53, // bool
        VulkanEmitReflection = 54, // bool
        GlslForceScalarLayout = 55, // bool
        EnableEffectAnnotations = 56, // bool
        EmitSpirvViaGlsl = 57, // bool (will be deprecated)
        EmitSpirvDirectly = 58, // bool (will be deprecated)
        SpirvCoreGrammarJson = 59, // stringValue0: json path
        IncompleteLibrary = 60, // bool, when set, will not issue an error when the linked programhas unresolved extern function symbols.

        /// Downstream
        Path = 61,

        /// Downstream
        DefaultDownstreamCompiler = 62,
        DownstreamArgs = 63, // stringValue0: downstream compiler name. stringValue1: argument list,one per line.
        PassThrough = 64,

        /// Repro
        DumpRepro = 65,

        /// Repro
        DumpReproOnError = 66,

        /// Repro
        ExtractRepro = 67,

        /// Repro
        LoadRepro = 68,

        /// Repro
        LoadReproDirectory = 69,

        /// Repro
        ReproFallbackDirectory = 70,

        /// Debugging
        DumpAst = 71,

        /// Debugging
        DumpIntermediatePrefix = 72,
        DumpIntermediates = 73, // bool
        DumpIR = 74, // bool
        DumpIRIds = 75,
        PreprocessorOutput = 76,
        OutputIncludes = 77,
        ReproFileSystem = 78,
        REMOVEDSerialIR = 79, // deprecated and removed; value must never be reused
        SkipCodeGen = 80, // bool
        ValidateIR = 81, // bool
        VerbosePaths = 82,
        VerifyDebugSerialIR = 83,
        NoCodeGen = 84, // Not used.

        /// Experimental
        FileSystem = 85,

        /// Experimental
        Heterogeneous = 86,

        /// Experimental
        NoMangle = 87,

        /// Experimental
        NoHlslBinding = 88,

        /// Experimental
        NoHlslPackConstantBufferElements = 89,

        /// Experimental
        ValidateUniformity = 90,

        /// Experimental
        AllowGlsl = 91,

        /// Experimental
        EnableExperimentalPasses = 92,
        BindlessSpaceIndex = 93, // int
        SpirvResourceHeapStride = 94, // int: byte stride for SPIRV resource descriptor heap
        SpirvSamplerHeapStride = 95, // int: byte stride for SPIRV sampler descriptor heap

        /// Internal
        ArchiveType = 96,

        /// Internal
        CompileCoreModule = 97,

        /// Internal
        Doc = 98,
        IRCompression = 99, // deprecated; value must never be reused
        LoadCoreModule = 100,
        ReferenceModule = 101,
        SaveCoreModule = 102,
        SaveCoreModuleBinSource = 103,
        TrackLiveness = 104,
        LoopInversion = 105, // bool, enable loop inversion optimization
        ParameterBlocksUseRegisterSpaces = 106, // Deprecated; value must never be reused
        LanguageVersion = 107, // intValue0: SlangLanguageVersion
        TypeConformance = 108, // stringValue0: type conformance to link; format:"<TypeName>:<IInterfaceName>[=<sequentialId>]",e.g. "Impl:IFoo=3" or "Impl:IFoo".
        EnableExperimentalDynamicDispatch = 109, // bool, experimental
        EmitReflectionJson = 110, // bool
        CountOfParsableOptions = 111, // historical sentinel; value must not be reused
        DebugInformationFormat = 112, // intValue0: DebugInfoFormat (derived from -g; no direct CLIflag)
        VulkanBindShiftAll = 113, // intValue0: kind; intValue1: shift (derived from-fvk-x-shift; no direct CLI flag)
        GenerateWholeProgram = 114, // bool
        UseUpToDateBinaryModule = 115, // bool, when set, will only load precompiled modulesif up-to-date with source. (API-only; no direct CLI flag)
        EmbedDownstreamIR = 116, // bool
        ForceDxLayout = 117, // bool
        EmitSpirvMethod = 118, // enum SlangEmitSpirvMethod (derived; no direct CLI flag)
        SaveGlslModuleBinSource = 119,
        SkipDownstreamLinking = 120, // bool, experimental (API-only; no direct CLI flag)
        DumpModule = 121,
        GetModuleInfo = 122, // Print serialized module version and name
        GetSupportedModuleVersions = 123, // Print the min and max module versions this compilersupports
        EmitSeparateDebug = 124, // bool

        /// Floating point denormal handling modes
        DenormalModeFp16 = 125,

        /// Floating point denormal handling modes
        DenormalModeFp32 = 126,

        /// Floating point denormal handling modes
        DenormalModeFp64 = 127,
        UseMsvcStyleBitfieldPacking = 128, // bool
        ForceCLayout = 129, // bool
        ExperimentalFeature = 130, // bool, enable experimental features
        ReportDetailedPerfBenchmark = 131, // bool, reports detailed compiler performance benchmarkresults
        ValidateIRDetailed = 132, // bool, enable detailed IR validation
        DumpIRBefore = 133, // string, pass name to dump IR before
        DumpIRAfter = 134, // string, pass name to dump IR after
        EmitCpuMethod = 135, // enum SlangEmitCPUMethod (derived; no direct CLI flag)
        EmitCpuViaCpp = 136, // bool
        EmitCpuViaLlvm = 137, // bool
        LlvmTargetTriple = 138, // string
        LlvmCpu = 139, // string
        LlvmFeatures = 140, // string
        EnableRichDiagnostics = 141, // bool, enable the experimental rich diagnostics
        ReportDynamicDispatchSites = 142, // bool
        EnableMachineReadableDiagnostics = 143, // bool, enable machine-readable diagnostic output(implies EnableRichDiagnostics)
        DiagnosticColor = 144, // intValue0: SlangDiagnosticColor (always, never, auto)

        /// Add new options HERE, immediately before CountOf.
        CountOf,
    }

    public enum CompilerOptionValueKind
    {
        Int = 0,
        String = 1,
    }

    public enum BuiltinModuleName
    {
        Core = 0,
        Glsl = 1,
    }

    public enum ContainerType
    {
        None = 0,
        UnsizedArray = 1,
        StructuredBuffer = 2,
        ConstantBuffer = 3,
        ParameterBlock = 4,
    }

    public enum OperandDataType
    {
        General = 0, // General data type, can be any type.
        Int32 = 1, // 32-bit integer.
        Int64 = 2, // 64-bit integer.
        Float32 = 3, // 32-bit floating-point number.
        Float64 = 4, // 64-bit floating-point number.
        String = 5, // String data type, represented as a pointer to a null-terminated string.
    }

    public enum Severity : SeverityIntegral
    {
        Disabled = 0, // < A message that is disabled, filtered out.
        Note = 1, // < An informative message.
        Warning = 2, // < A warning, which indicates a possible problem.
        Error = 3, // < An error, indicating that compilation failed.
        Fatal = 4, // < An unrecoverable error, which forced compilation to abort.
        Internal = 5,
    }

    public enum BindableResourceType : BindableResourceIntegral
    {
        NonBindable = 0,
        Texture = 1,
        Sampler = 2,
        UniformBuffer = 3,
        StorageBuffer = 4,
    }

    public enum CompileTarget : CompileTargetIntegral
    {
        TargetUnknown = 0,
        TargetNone = 1,
        Glsl = 2,
        GlslVulkanDeprecated = 3, // < deprecated and removed: just use `SLANG_GLSL`.
        GlslVulkanOneDescDeprecated = 4, // < deprecated and removed.
        Hlsl = 5,
        Spirv = 6,
        SpirvAsm = 7,
        Dxbc = 8,
        DxbcAsm = 9,
        Dxil = 10,
        DxilAsm = 11,
        CSource = 12, // < The C language
        CppSource = 13, // < C++ code for shader kernels.
        HostExecutable = 14, // < Standalone binary executable (for hosting CPU/OS)
        ShaderSharedLibrary = 15, // < A shared library/Dll for shader kernels (for hosting< CPU/OS)
        ShaderHostCallable = 16, // < A CPU target that makes the compiled shader code< available to be run immediately
        CudaSource = 17, // < Cuda source
        Ptx = 18, // < PTX
        CudaObjectCode = 19, // < Object code that contains CUDA functions.
        ObjectCode = 20, // < Object code that can be used for later linking (kernel/shader)
        HostCppSource = 21, // < C++ code for host library or executable.
        HostHostCallable = 22, // < Host callable host code (ie non kernel/shader)
        CppPytorchBinding = 23, // < C++ PyTorch binding code.
        Metal = 24, // < Metal shading language
        MetalLib = 25, // < Metal library
        MetalLibAsm = 26, // < Metal library assembly
        HostSharedLibrary = 27, // < A shared library/Dll for host code (for hosting CPU/OS)
        Wgsl = 28, // < WebGPU shading language
        WgslSpirvAsm = 29, // < SPIR-V assembly via WebGPU shading language
        WgslSpirv = 30, // < SPIR-V via WebGPU shading language
        HostVm = 31, // < Bytecode that can be interpreted by the Slang VM
        CppHeader = 32, // < C++ header for shader kernels.
        CudaHeader = 33, // < Cuda header
        HostObjectCode = 34, // < Host object code
        HostLlvmIR = 35, // < Host LLVM IR assembly
        ShaderLlvmIR = 36, // < Host LLVM IR assembly (kernel/shader)
        TargetCountOf,
    }

    public enum ContainerFormat : ContainerFormatIntegral
    {
        ///  Don't generate a container.
        None = 0,

        ///  Generate a container in the `.slang-module` format,
        /// which includes reflection information, compiled kernels, etc.
        SlangModule = 1,
    }

    public enum PassThrough : PassThroughIntegral
    {
        None = 0,
        Fxc = 1,
        Dxc = 2,
        Glslang = 3,
        SpirvDis = 4,
        Clang = 5, // < Clang C/C++ compiler
        VisualStudio = 6, // < Visual studio C/C++ compiler
        GCC = 7, // < GCC C/C++ compiler
        GenericCCpp = 8, // < Generic C or C++ compiler, which is decided by< the source type
        Nvrtc = 9, // < NVRTC Cuda compiler
        Llvm = 10, // < LLVM 'compiler' - includes LLVM and Clang
        SpirvOpt = 11, // < SPIRV-opt
        Metal = 12, // < Metal compiler
        Tint = 13, // < Tint WGSL compiler
        SpirvLink = 14, // < SPIRV-link
        CountOf,
    }

    public enum ArchiveType : ArchiveTypeIntegral
    {
        Undefined = 0,
        Zip = 1,
        Riff = 2, // < Riff container with no compression
        RiffDeflate = 3,
        RiffLz4 = 4,
        CountOf,
    }

    public enum FloatingPointMode : FloatingPointModeIntegral
    {
        Default = 0,
        Fast = 1,
        Precise = 2,
    }

    public enum FpDenormalMode : FpDenormalModeIntegral
    {
        DenormModeAny = 0,
        DenormModePreserve = 1,
        DenormModeFtz = 2,
    }

    public enum LineDirectiveMode : LineDirectiveModeIntegral
    {
        Default = 0,
        None = 1, // < Don't emit line directives at all.
        Standard = 2, // < Emit standard C-style `#line` directives.
        Glsl = 3, // < Emit GLSL-style directives with file *number*instead of name
        SourceMap = 4, // < Use a source map to track line mappings (ieno #line will appear in emitting source)
    }

    public enum SourceLanguage : SourceLanguageIntegral
    {
        Unknown = 0,
        Slang = 1,
        Hlsl = 2,
        Glsl = 3,
        C = 4,
        Cpp = 5,
        Cuda = 6,
        Spirv = 7,
        Metal = 8,
        Wgsl = 9,
        Llvm = 10,
        CountOf,
    }

    public enum ProfileID : ProfileIDIntegral
    {
        Unknown = 0,
    }

    public enum CapabilityID : CapabilityIDIntegral
    {
        Unknown = 0,
    }

    public enum MatrixLayoutMode : MatrixLayoutModeIntegral
    {
        Unknown = 0,
        RowMajor = 1,
        ColumnMajor = 2,
    }

    public enum Stage : StageIntegral
    {
        None = 0,
        Vertex = 1,
        Hull = 2,
        Domain = 3,
        Geometry = 4,
        Fragment = 5,
        Compute = 6,
        RayGeneration = 7,
        Intersection = 8,
        AnyHit = 9,
        ClosestHit = 10,
        Miss = 11,
        Callable = 12,
        Mesh = 13,
        Amplification = 14,
        Dispatch = 15,

        ///
        Count,

        /// alias:
        Pixel = Fragment,
    }

    public enum CooperativeMatrixUse : CooperativeMatrixUseIntegral
    {
        A = 0,
        B = 1,
        Accumulator = 2,
    }

    public enum CooperativeVectorMatrixLayout : CooperativeVectorMatrixLayoutIntegral
    {
        RowMajor = 0,
        ColumnMajor = 1,
        InferencingOptimal = 2,
        TrainingOptimal = 3,
    }

    public enum DebugInfoLevel : DebugInfoLevelIntegral
    {
        None = 0, // < Don't emit debug information at all.
        Minimal = 1, // < Emit as little debug information as possible,while still supporting stack trackers.
        Standard = 2, // < Emit whatever is the standard level of debuginformation for each target.
        Maximal = 3, // < Emit as much debug information as possible foreach target.
    }

    public enum DebugInfoFormat : DebugInfoFormatIntegral
    {
        Default = 0, // < Use the default debugging format for the target
        C7 = 1, // < CodeView C7 format (typically means debugging< information is embedded in the binary)
        Pdb = 2, // < Program database
        Stabs = 3, // < Stabs
        Coff = 4, // < COFF debug info
        Dwarf = 5, // < DWARF debug info (we may want to support specifying< the version)
        CountOf,
    }

    public enum OptimizationLevel : OptimizationLevelIntegral
    {
        None = 0, // < Don't optimize at all.
        Default = 1, // < Default optimization level: balance code qualityand compilation time.
        High = 2, // < Optimize aggressively.
        Maximal = 3, // < Include optimizations that may take a very longtime, or may involve severe space-vs-speedtradeoffs
    }

    public enum EmitSpirvMethod : uint
    {
        Default = 0,
        ViaGlsl = 1,
        Directly = 2,
    }

    public enum EmitCpuMethod : uint
    {
        Default = 0,
        ViaCpp = 1,
        ViaLlvm = 2,
    }

    public enum DiagnosticColor : uint
    {
        Auto = 0, // Use color if output sink is a tty
        Always = 1, // Always use color
        Never = 2, // Never use color
    }

    public enum PathType : PathTypeIntegral
    {
        Directory = 0, // < Path specified specifies a directory.
        File = 1, // < Path specified is to a file.
    }

    ///  Determines how paths map to files on the OS file system
    public enum OSPathKind : byte
    {
        None = 0, // < Paths do not map to the file system
        Direct = 1, // < Paths map directly to the file system
        OperatingSystem = 2, // < Only paths gained via PathKind::OperatingSystem map to the< operating system file system
    }

    ///  Used to determine what kind of path is required from an input path
    public enum PathKind
    {
        /// Given a path, returns a simplified version of that path.
        /// This typically means removing '..' and/or '.' from the path.
        /// A simplified path must point to the same object as the original.
        Simplified = 0,

        /// Given a path, returns a 'canonical path' to the item.
        /// This may be the operating system 'canonical path' that is the unique path to the item.
        ///
        /// If the item exists the returned canonical path should always be usable to access the
        /// item.
        ///
        /// If the item the path specifies doesn't exist, the canonical path may not be returnable
        /// or be a path simplification.
        /// Not all file systems support canonical paths.
        Canonical = 1,

        /// Given a path returns a path such that it is suitable to be displayed to the user.
        ///
        /// For example if the file system is a zip file - it might include the path to the zip
        /// container as well as the path to the specific file.
        ///
        /// NOTE! The display path won't necessarily work on the file system to access the item
        Display = 2,

        /// Get the path to the item on the *operating system* file system, if available.
        OperatingSystem = 3,

        /// Get the path to the item on the *operating system* file system, if available.
        CountOf,
    }

    public enum WriterChannel : WriterChannelIntegral
    {
        Diagnostic = 0,
        StdOutput = 1,
        StdError = 2,
        CountOf,
    }

    public enum WriterMode : WriterModeIntegral
    {
        Text = 0,
        Binary = 1,
    }

    public enum ReflectionGenericArgType : uint
    {
        GenericArgType = 0,
        GenericArgInt = 1,
        GenericArgBool = 2,
    }

    public enum TypeKind : TypeKindIntegral
    {
        None = 0,
        Struct = 1,
        Array = 2,
        Matrix = 3,
        Vector = 4,
        Scalar = 5,
        ConstantBuffer = 6,
        Resource = 7,
        SamplerState = 8,
        TextureBuffer = 9,
        ShaderStorageBuffer = 10,
        ParameterBlock = 11,
        GenericTypeParameter = 12,
        Interface = 13,
        OutputStream = 14,
        MeshOutput = 15,
        Specialized = 16,
        Feedback = 17,
        Pointer = 18,
        DynamicResource = 19,
        Enum = 20,
        Count,
    }

    public enum ScalarType : ScalarTypeIntegral
    {
        None = 0,
        Void = 1,
        Bool = 2,
        Int32 = 3,
        UInt32 = 4,
        Int64 = 5,
        UInt64 = 6,
        Float16 = 7,
        Float32 = 8,
        Float64 = 9,
        Int8 = 10,
        UInt8 = 11,
        Int16 = 12,
        UInt16 = 13,
        IntPtr = 14,
        UIntPtr = 15,
        BFloat16 = 16,
        FloatE4M3 = 17,
        FloatE5M2 = 18,
    }

    public enum DeclKind : DeclKindIntegral
    {
        UnsupportedForReflection = 0,
        Struct = 1,
        Func = 2,
        Module = 3,
        Generic = 4,
        Variable = 5,
        Namespace = 6,
        Enum = 7,
    }

    public enum ResourceShape : ResourceShapeIntegral
    {
        BaseShapeMask = 0X0F,
        None = 0X00,
        Texture1D = 0X01,
        Texture2D = 0X02,
        Texture3D = 0X03,
        TextureCube = 0X04,
        TextureBuffer = 0X05,
        StructuredBuffer = 0X06,
        ByteAddressBuffer = 0X07,
        Unknown = 0X08,
        AccelerationStructure = 0X09,
        TextureSubpass = 0X0A,
        ExtShapeMask = 0X1F0,
        TextureFeedbackFlag = 0X10,
        TextureShadowFlag = 0X20,
        TextureArrayFlag = 0X40,
        TextureMultisampleFlag = 0X80,
        TextureCombinedFlag = 0X100,
        Texture1DArray = Texture1D | TextureArrayFlag,
        Texture2DArray = Texture2D | TextureArrayFlag,
        TextureCubeArray = TextureCube | TextureArrayFlag,
        Texture2DMultisample = Texture2D | TextureMultisampleFlag,
        Texture2DMultisampleArray = Texture2D | TextureMultisampleFlag | TextureArrayFlag,
        TextureSubpassMultisample = TextureSubpass | TextureMultisampleFlag,
    }

    public enum ResourceAccess : ResourceAccessIntegral
    {
        None = 0,
        Read = 1,
        ReadWrite = 2,
        RasterOrdered = 3,
        Append = 4,
        Consume = 5,
        Write = 6,
        Feedback = 7,
        Unknown = 0X7FFFFFFF,
    }

    public enum ParameterCategory : ParameterCategoryIntegral
    {
        None = 0,
        Mixed = 1,
        ConstantBuffer = 2,
        ShaderResource = 3,
        UnorderedAccess = 4,
        VaryingInput = 5,
        VaryingOutput = 6,
        SamplerState = 7,
        Uniform = 8,
        DescriptorTableSlot = 9,
        SpecializationConstant = 10,
        PushConstantBuffer = 11,

        /// HLSL register `space`, Vulkan GLSL `set`
        RegisterSpace = 12,

        /// A parameter whose type is to be specialized by a global generic type argument
        Generic = 13,

        /// A parameter whose type is to be specialized by a global generic type argument
        RayPayload = 14,

        /// A parameter whose type is to be specialized by a global generic type argument
        HitAttributes = 15,

        /// A parameter whose type is to be specialized by a global generic type argument
        CallablePayload = 16,

        /// A parameter whose type is to be specialized by a global generic type argument
        ShaderRecord = 17,

        /// An existential type parameter represents a "hole" that
        /// needs to be filled with a concrete type to enable
        /// generation of specialized code.
        ///
        /// Consider this example:
        ///
        ///      struct MyParams
        ///      {
        ///          IMaterial material;
        ///          ILight lights[3];
        ///      };
        ///
        /// This `MyParams` type introduces two existential type parameters:
        /// one for `material` and one for `lights`. Even though `lights`
        /// is an array, it only introduces one type parameter, because
        /// we need to have a *single* concrete type for all the array
        /// elements to be able to generate specialized code.
        ExistentialTypeParam = 18,

        /// An existential object parameter represents a value
        /// that needs to be passed in to provide data for some
        /// interface-type shader parameter.
        ///
        /// Consider this example:
        ///
        ///      struct MyParams
        ///      {
        ///          IMaterial material;
        ///          ILight lights[3];
        ///      };
        ///
        /// This `MyParams` type introduces four existential object parameters:
        /// one for `material` and three for `lights` (one for each array
        /// element). This is consistent with the number of interface-type
        /// "objects" that are being passed through to the shader.
        ExistentialObjectParam = 19,

        /// The register space offset for the sub-elements that occupies register spaces.
        SubElementRegisterSpace = 20,

        /// The input_attachment_index subpass occupancy tracker
        Subpass = 21,

        /// Metal tier-1 argument buffer element [[id]].
        MetalArgumentBufferElement = 22,

        /// Metal [[attribute]] inputs.
        MetalAttribute = 23,

        /// Metal [[payload]] inputs
        MetalPayload = 24,

        ///
        Count,

        /// Aliases for Metal-specific categories.
        MetalBuffer = ConstantBuffer,

        /// Aliases for Metal-specific categories.
        MetalTexture = ShaderResource,

        /// Aliases for Metal-specific categories.
        MetalSampler = SamplerState,

        /// DEPRECATED:
        VertexInput = VaryingInput,

        /// DEPRECATED:
        FragmentOutput = VaryingOutput,

        /// DEPRECATED:
        CountV1 = Subpass,
    }

    public enum BindingType : BindingTypeIntegral
    {
        Unknown = 0,
        Sampler = 1,
        Texture = 2,
        ConstantBuffer = 3,
        ParameterBlock = 4,
        TypedBuffer = 5,
        RawBuffer = 6,
        CombinedTextureSampler = 7,
        InputRenderTarget = 8,
        InlineUniformData = 9,
        RayTracingAccelerationStructure = 10,
        VaryingInput = 11,
        VaryingOutput = 12,
        ExistentialValue = 13,
        PushConstant = 14,
        MutableFlag = 0X100,
        MutableTexture = Texture | MutableFlag,
        MutableTypedBuffer = TypedBuffer | MutableFlag,
        MutableRawBuffer = RawBuffer | MutableFlag,
        BaseMask = 0X00FF,
        ExtMask = 0XFF00,
    }

    public enum LayoutRules : LayoutRulesIntegral
    {
        Default = 0,
        MetalArgumentBufferTier2 = 1,
        DefaultStructuredBuffer = 2,
        DefaultConstantBuffer = 3,
    }

    public enum ModifierID : ModifierIDIntegral
    {
        Shared = 0,
        NoDiff = 1,
        Static = 2,
        Const = 3,
        Export = 4,
        Extern = 5,
        Differentiable = 6,
        Mutating = 7,
        In = 8,
        Out = 9,
        InOut = 10,
    }

    public enum ImageFormat : ImageFormatIntegral
    {
        Unknown,
        RGBA32F,
        RGBA16F,
        RG32F,
        RG16F,
        R11FG11FB10F,
        R32F,
        R16F,
        RGBA16,
        RGB10A2,
        RGBA8,
        RG16,
        RG8,
        R16,
        R8,
        RGBA16SNorm,
        RGBA8SNorm,
        RG16SNorm,
        RG8SNorm,
        R16SNorm,
        R8SNorm,
        RGBA32I,
        RGBA16I,
        RGBA8I,
        RG32I,
        RG16I,
        RG8I,
        R32I,
        R16I,
        R8I,
        RGBA32UI,
        RGBA16UI,
        RGB10A2UI,
        RGBA8UI,
        RG32UI,
        RG16UI,
        RG8UI,
        R32UI,
        R16UI,
        R8UI,
        R64UI,
        R64I,
        BGRA8,
    }

    public enum LanguageVersion : uint
    {
        Unknown = 0,
        Legacy = 2018,
        _2025 = 2025,
        _2026 = 2026,
        Default = Legacy,
        Latest = _2026,
    }
}
