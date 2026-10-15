using Brutal.Interop;
using Brutal.Strings;

namespace Brutal.SlangApi
{
    public unsafe partial struct CompilerOptionValue
    {
        public Brutal.SlangApi.CompilerOptionValueKind Kind;
        public int IntValue0;
        public int IntValue1;
        public byte* StringValue0;
        public byte* StringValue1;
    };

    public unsafe partial struct CompilerOptionEntry
    {
        public Brutal.SlangApi.CompilerOptionName Name;
        public Brutal.SlangApi.CompilerOptionValue Value;
    };

    /// @brief A request for one or more compilation actions to be performed.
    [InteropHandle]
    public readonly unsafe partial struct ICompileRequest : IDisposable
    {
        public void Dispose()
        {
            Slang.PInvoke.spDestroyCompileRequest(this);
        }

        internal void SetFileSystem(nint fileSystem)
        {
            Slang.PInvoke.spSetFileSystem(this, fileSystem);
        }

        public void SetCompileFlags(CompileFlags flags)
        {
            Slang.PInvoke.spSetCompileFlags(this, flags);
        }

        public CompileFlags GetCompileFlags()
        {
            return Slang.PInvoke.spGetCompileFlags(this);
        }

        public void SetDumpIntermediates(int enable)
        {
            Slang.PInvoke.spSetDumpIntermediates(this, enable);
        }

        public void SetDumpIntermediatePrefix(RefString8 prefix)
        {
            Slang.PInvoke.spSetDumpIntermediatePrefix(this, prefix);
        }

        public void SetLineDirectiveMode(Brutal.SlangApi.LineDirectiveMode mode)
        {
            Slang.PInvoke.spSetLineDirectiveMode(this, mode);
        }

        public void SetTargetLineDirectiveMode(int targetIndex, Brutal.SlangApi.LineDirectiveMode mode)
        {
            Slang.PInvoke.spSetTargetLineDirectiveMode(this, targetIndex, mode);
        }

        public void SetTargetForceGlslScalarBufferLayout(int targetIndex, Brutal.Bool8 forceScalarLayout)
        {
            Slang.PInvoke.spSetTargetForceGLSLScalarBufferLayout(this, targetIndex, forceScalarLayout);
        }

        public void SetTargetUseMinimumSlangOptimization(int targetIndex, Brutal.Bool8 val)
        {
            Slang.PInvoke.spSetTargetUseMinimumSlangOptimization(this, targetIndex, val);
        }

        public void SetIgnoreCapabilityCheck(Brutal.Bool8 val)
        {
            Slang.PInvoke.spSetIgnoreCapabilityCheck(this, val);
        }

        public void SetCodeGenTarget(Brutal.SlangApi.CompileTarget target)
        {
            Slang.PInvoke.spSetCodeGenTarget(this, target);
        }

        public int AddCodeGenTarget(Brutal.SlangApi.CompileTarget target)
        {
            return Slang.PInvoke.spAddCodeGenTarget(this, target);
        }

        public void SetTargetProfile(int targetIndex, Brutal.SlangApi.ProfileID profile)
        {
            Slang.PInvoke.spSetTargetProfile(this, targetIndex, profile);
        }

        public void SetTargetFlags(int targetIndex, TargetFlags flags)
        {
            Slang.PInvoke.spSetTargetFlags(this, targetIndex, flags);
        }

        public void SetTargetFloatingPointMode(int targetIndex, Brutal.SlangApi.FloatingPointMode mode)
        {
            Slang.PInvoke.spSetTargetFloatingPointMode(this, targetIndex, mode);
        }

        public void AddTargetCapability(int targetIndex, Brutal.SlangApi.CapabilityID capability)
        {
            Slang.PInvoke.spAddTargetCapability(this, targetIndex, capability);
        }

        public void SetTargetMatrixLayoutMode(int targetIndex, Brutal.SlangApi.MatrixLayoutMode mode)
        {
            Slang.PInvoke.spSetTargetMatrixLayoutMode(this, targetIndex, mode);
        }

        public void SetMatrixLayoutMode(Brutal.SlangApi.MatrixLayoutMode mode)
        {
            Slang.PInvoke.spSetMatrixLayoutMode(this, mode);
        }

        public void SetDebugInfoLevel(Brutal.SlangApi.DebugInfoLevel level)
        {
            Slang.PInvoke.spSetDebugInfoLevel(this, level);
        }

        public void SetDebugInfoFormat(Brutal.SlangApi.DebugInfoFormat format)
        {
            Slang.PInvoke.spSetDebugInfoFormat(this, format);
        }

        public void SetOptimizationLevel(Brutal.SlangApi.OptimizationLevel level)
        {
            Slang.PInvoke.spSetOptimizationLevel(this, level);
        }

        public void SetOutputContainerFormat(Brutal.SlangApi.ContainerFormat format)
        {
            Slang.PInvoke.spSetOutputContainerFormat(this, format);
        }

        public void SetPassThrough(Brutal.SlangApi.PassThrough passThrough)
        {
            Slang.PInvoke.spSetPassThrough(this, passThrough);
        }

        public unsafe void SetDiagnosticCallback(DiagnosticCallback callback, void* userData)
        {
            Slang.PInvoke.spSetDiagnosticCallback(this, callback, userData);
        }

        internal void SetWriter(Brutal.SlangApi.WriterChannel channel, nint writer)
        {
            Slang.PInvoke.spSetWriter(this, channel, writer);
        }

        internal nint GetWriter(Brutal.SlangApi.WriterChannel channel)
        {
            return Slang.PInvoke.spGetWriter(this, channel);
        }

        public void AddSearchPath(RefString8 searchDir)
        {
            Slang.PInvoke.spAddSearchPath(this, searchDir);
        }

        public void AddPreprocessorDefine(RefString8 key, RefString8 value)
        {
            Slang.PInvoke.spAddPreprocessorDefine(this, key, value);
        }

        public unsafe Result ProcessCommandLineArguments(byte** args, int argCount)
        {
            return Slang.PInvoke.spProcessCommandLineArguments(this, args, argCount);
        }

        public int AddTranslationUnit(Brutal.SlangApi.SourceLanguage language, RefString8 name)
        {
            return Slang.PInvoke.spAddTranslationUnit(this, language, name);
        }

        public void SetDefaultModuleName(RefString8 defaultModuleName)
        {
            Slang.PInvoke.spSetDefaultModuleName(this, defaultModuleName);
        }

        public void TranslationUnitAddPreprocessorDefine(int translationUnitIndex, RefString8 key, RefString8 value)
        {
            Slang.PInvoke.spTranslationUnit_addPreprocessorDefine(this, translationUnitIndex, key, value);
        }

        public void AddTranslationUnitSourceFile(int translationUnitIndex, RefString8 path)
        {
            Slang.PInvoke.spAddTranslationUnitSourceFile(this, translationUnitIndex, path);
        }

        public void AddTranslationUnitSourceString(int translationUnitIndex, RefString8 path, RefString8 source)
        {
            Slang.PInvoke.spAddTranslationUnitSourceString(this, translationUnitIndex, path, source);
        }

        public unsafe Result AddLibraryReference(RefString8 basePath, void* libData, ByteSize64 libDataSize)
        {
            return Slang.PInvoke.spAddLibraryReference(this, basePath, libData, libDataSize);
        }

        public void AddTranslationUnitSourceStringSpan(
            int translationUnitIndex,
            RefString8 path,
            RefString8 sourceBegin,
            RefString8 sourceEnd
        )
        {
            Slang.PInvoke.spAddTranslationUnitSourceStringSpan(
                this,
                translationUnitIndex,
                path,
                sourceBegin,
                sourceEnd
            );
        }

        internal void AddTranslationUnitSourceBlob(int translationUnitIndex, RefString8 path, nint sourceBlob)
        {
            Slang.PInvoke.spAddTranslationUnitSourceBlob(this, translationUnitIndex, path, sourceBlob);
        }

        public int AddEntryPoint(int translationUnitIndex, RefString8 name, Brutal.SlangApi.Stage stage)
        {
            return Slang.PInvoke.spAddEntryPoint(this, translationUnitIndex, name, stage);
        }

        public unsafe int AddEntryPointEx(
            int translationUnitIndex,
            RefString8 name,
            Brutal.SlangApi.Stage stage,
            int genericArgCount,
            byte** genericArgs
        )
        {
            return Slang.PInvoke.spAddEntryPointEx(
                this,
                translationUnitIndex,
                name,
                stage,
                genericArgCount,
                genericArgs
            );
        }

        public unsafe Result SetGlobalGenericArgs(int genericArgCount, byte** genericArgs)
        {
            return Slang.PInvoke.spSetGlobalGenericArgs(this, genericArgCount, genericArgs);
        }

        public Result SetTypeNameForGlobalExistentialTypeParam(int slotIndex, RefString8 typeName)
        {
            return Slang.PInvoke.spSetTypeNameForGlobalExistentialTypeParam(this, slotIndex, typeName);
        }

        public Result SetTypeNameForEntryPointExistentialTypeParam(
            int entryPointIndex,
            int slotIndex,
            RefString8 typeName
        )
        {
            return Slang.PInvoke.spSetTypeNameForEntryPointExistentialTypeParam(
                this,
                entryPointIndex,
                slotIndex,
                typeName
            );
        }

        public Result Compile()
        {
            return Slang.PInvoke.spCompile(this);
        }

        public string GetDiagnosticOutput()
        {
            return Slang.PInvoke.spGetDiagnosticOutput(this);
        }

        internal Result GetDiagnosticOutputBlob(out nint outBlob)
        {
            return Slang.PInvoke.spGetDiagnosticOutputBlob(this, out outBlob);
        }

        public int GetDependencyFileCount()
        {
            return Slang.PInvoke.spGetDependencyFileCount(this);
        }

        public string GetDependencyFilePath(int index)
        {
            return Slang.PInvoke.spGetDependencyFilePath(this, index);
        }

        public int GetTranslationUnitCount()
        {
            return Slang.PInvoke.spGetTranslationUnitCount(this);
        }

        public string GetEntryPointSource(int entryPointIndex)
        {
            return Slang.PInvoke.spGetEntryPointSource(this, entryPointIndex);
        }

        public unsafe void* GetEntryPointCode(int entryPointIndex, out ByteSize64 outSize)
        {
            return Slang.PInvoke.spGetEntryPointCode(this, entryPointIndex, out outSize);
        }

        internal Result GetEntryPointCodeBlob(int entryPointIndex, int targetIndex, out nint outBlob)
        {
            return Slang.PInvoke.spGetEntryPointCodeBlob(this, entryPointIndex, targetIndex, out outBlob);
        }

        internal Result GetEntryPointHostCallable(int entryPointIndex, int targetIndex, out nint outSharedLibrary)
        {
            return Slang.PInvoke.spGetEntryPointHostCallable(this, entryPointIndex, targetIndex, out outSharedLibrary);
        }

        internal Result GetTargetCodeBlob(int targetIndex, out nint outBlob)
        {
            return Slang.PInvoke.spGetTargetCodeBlob(this, targetIndex, out outBlob);
        }

        internal Result GetTargetHostCallable(int targetIndex, out nint outSharedLibrary)
        {
            return Slang.PInvoke.spGetTargetHostCallable(this, targetIndex, out outSharedLibrary);
        }

        public unsafe void* GetCompileRequestCode(out ByteSize64 outSize)
        {
            return Slang.PInvoke.spGetCompileRequestCode(this, out outSize);
        }

        internal Result GetContainerCode(out nint outBlob)
        {
            return Slang.PInvoke.spGetContainerCode(this, out outBlob);
        }

        internal unsafe Result LoadRepro(nint fileSystem, void* data, ByteSize64 size)
        {
            return Slang.PInvoke.spLoadRepro(this, fileSystem, data, size);
        }

        internal Result SaveRepro(out nint outBlob)
        {
            return Slang.PInvoke.spSaveRepro(this, out outBlob);
        }

        public Result EnableReproCapture()
        {
            return Slang.PInvoke.spEnableReproCapture(this);
        }

        internal unsafe Result GetCompileTimeProfile(nint* compileTimeProfile, Brutal.Bool8 shouldClear)
        {
            return Slang.PInvoke.spGetCompileTimeProfile(this, compileTimeProfile, shouldClear);
        }

        public void OverrideDiagnosticSeverity(Int messageID, Brutal.SlangApi.Severity overrideSeverity)
        {
            Slang.PInvoke.spOverrideDiagnosticSeverity(this, messageID, overrideSeverity);
        }

        public DiagnosticFlags GetDiagnosticFlags()
        {
            return Slang.PInvoke.spGetDiagnosticFlags(this);
        }

        public void SetDiagnosticFlags(DiagnosticFlags flags)
        {
            Slang.PInvoke.spSetDiagnosticFlags(this, flags);
        }

        public Reflection GetReflection()
        {
            return Slang.PInvoke.spGetReflection(this);
        }

        public Result IsParameterLocationUsed(
            Int entryPointIndex,
            Int targetIndex,
            Brutal.SlangApi.ParameterCategory category,
            UInt spaceIndex,
            UInt registerIndex,
            ref Brutal.Bool8 outUsed
        )
        {
            return Slang.PInvoke.spIsParameterLocationUsed(
                this,
                entryPointIndex,
                targetIndex,
                category,
                spaceIndex,
                registerIndex,
                ref outUsed
            );
        }

        public string GetTranslationUnitSource(int translationUnitIndex)
        {
            return Slang.PInvoke.spGetTranslationUnitSource(this, translationUnitIndex);
        }

        internal Result GetProgram(out nint outProgram)
        {
            return Slang.PInvoke.spCompileRequest_getProgram(this, out outProgram);
        }

        internal Result GetProgramWithEntryPoints(out nint outProgram)
        {
            return Slang.PInvoke.spCompileRequest_getProgramWithEntryPoints(this, out outProgram);
        }

        internal Result GetEntryPoint(Int entryPointIndex, out nint outEntryPoint)
        {
            return Slang.PInvoke.spCompileRequest_getEntryPoint(this, entryPointIndex, out outEntryPoint);
        }

        internal Result GetModule(Int translationUnitIndex, out nint outModule)
        {
            return Slang.PInvoke.spCompileRequest_getModule(this, translationUnitIndex, out outModule);
        }

        internal Result GetSession(out nint outSession)
        {
            return Slang.PInvoke.spCompileRequest_getSession(this, out outSession);
        }
    };

    [InteropHandle]
    public readonly unsafe partial struct BufferReflection { };

    [InteropHandle]
    public readonly unsafe partial struct DeclReflection
    {
        public enum Kind
        {
            Unsupported = (int)DeclKind.UnsupportedForReflection,
            Struct = (int)DeclKind.Struct,
            Func = (int)DeclKind.Func,
            Module = (int)DeclKind.Module,
            Generic = (int)DeclKind.Generic,
            Variable = (int)DeclKind.Variable,
            Namespace = (int)DeclKind.Namespace,
            Enum = (int)DeclKind.Enum,
        }

        public uint GetChildrenCount()
        {
            return Slang.PInvoke.spReflectionDecl_getChildrenCount(this);
        }

        public ReflectionDecl GetChild(uint index)
        {
            return Slang.PInvoke.spReflectionDecl_getChild(this, index);
        }

        public string GetName()
        {
            return Slang.PInvoke.spReflectionDecl_getName(this);
        }

        public Brutal.SlangApi.DeclKind GetKind()
        {
            return Slang.PInvoke.spReflectionDecl_getKind(this);
        }

        public ReflectionFunction CastToFunction()
        {
            return Slang.PInvoke.spReflectionDecl_castToFunction(this);
        }

        public ReflectionVariable CastToVariable()
        {
            return Slang.PInvoke.spReflectionDecl_castToVariable(this);
        }

        public ReflectionGeneric CastToGeneric()
        {
            return Slang.PInvoke.spReflectionDecl_castToGeneric(this);
        }

        public ReflectionType ReflectionGetTypeFromDecl()
        {
            return Slang.PInvoke.spReflection_getTypeFromDecl(this);
        }

        public ReflectionDecl GetParent()
        {
            return Slang.PInvoke.spReflectionDecl_getParent(this);
        }

        public Brutal.SlangApi.ReflectionModifier FindModifier(Brutal.SlangApi.ModifierID modifierID)
        {
            return Slang.PInvoke.spReflectionDecl_findModifier(this, modifierID);
        }
    };

    [InteropHandle]
    public readonly unsafe partial struct TypeLayoutReflection
    {
        public new ReflectionType GetType()
        {
            return Slang.PInvoke.spReflectionTypeLayout_GetType(this);
        }

        public Brutal.SlangApi.TypeKind GetKind()
        {
            return Slang.PInvoke.spReflectionTypeLayout_getKind(this);
        }

        public ByteSize64 GetSize(Brutal.SlangApi.ParameterCategory category)
        {
            return Slang.PInvoke.spReflectionTypeLayout_GetSize(this, category);
        }

        public ByteSize64 GetStride(Brutal.SlangApi.ParameterCategory category)
        {
            return Slang.PInvoke.spReflectionTypeLayout_GetStride(this, category);
        }

        public int GetAlignment(Brutal.SlangApi.ParameterCategory category)
        {
            return Slang.PInvoke.spReflectionTypeLayout_getAlignment(this, category);
        }

        public uint GetFieldCount()
        {
            return Slang.PInvoke.spReflectionTypeLayout_GetFieldCount(this);
        }

        public ReflectionVariableLayout GetFieldByIndex(uint index)
        {
            return Slang.PInvoke.spReflectionTypeLayout_GetFieldByIndex(this, index);
        }

        public Int FindFieldIndexByName(RefString8 nameBegin, RefString8 nameEnd)
        {
            return Slang.PInvoke.spReflectionTypeLayout_findFieldIndexByName(this, nameBegin, nameEnd);
        }

        public ReflectionVariableLayout GetExplicitCounter()
        {
            return Slang.PInvoke.spReflectionTypeLayout_GetExplicitCounter(this);
        }

        public ByteSize64 GetElementStride(Brutal.SlangApi.ParameterCategory category)
        {
            return Slang.PInvoke.spReflectionTypeLayout_GetElementStride(this, category);
        }

        public ReflectionTypeLayout GetElementTypeLayout()
        {
            return Slang.PInvoke.spReflectionTypeLayout_GetElementTypeLayout(this);
        }

        public ReflectionVariableLayout GetElementVarLayout()
        {
            return Slang.PInvoke.spReflectionTypeLayout_GetElementVarLayout(this);
        }

        public ReflectionVariableLayout GetContainerVarLayout()
        {
            return Slang.PInvoke.spReflectionTypeLayout_getContainerVarLayout(this);
        }

        public Brutal.SlangApi.ParameterCategory GetParameterCategory()
        {
            return Slang.PInvoke.spReflectionTypeLayout_GetParameterCategory(this);
        }

        public uint GetCategoryCount()
        {
            return Slang.PInvoke.spReflectionTypeLayout_GetCategoryCount(this);
        }

        public Brutal.SlangApi.ParameterCategory GetCategoryByIndex(uint index)
        {
            return Slang.PInvoke.spReflectionTypeLayout_GetCategoryByIndex(this, index);
        }

        public Brutal.SlangApi.MatrixLayoutMode GetMatrixLayoutMode()
        {
            return Slang.PInvoke.spReflectionTypeLayout_GetMatrixLayoutMode(this);
        }

        public int GetGenericParamIndex()
        {
            return Slang.PInvoke.spReflectionTypeLayout_getGenericParamIndex(this);
        }

        public ReflectionTypeLayout GetPendingDataTypeLayout()
        {
            return Slang.PInvoke.spReflectionTypeLayout_getPendingDataTypeLayout(this);
        }

        public ReflectionVariableLayout GetSpecializedTypePendingDataVarLayout()
        {
            return Slang.PInvoke.spReflectionTypeLayout_getSpecializedTypePendingDataVarLayout(this);
        }

        public Int GetBindingRangeCount()
        {
            return Slang.PInvoke.spReflectionTypeLayout_getBindingRangeCount(this);
        }

        public Brutal.SlangApi.BindingType GetBindingRangeType(Int index)
        {
            return Slang.PInvoke.spReflectionTypeLayout_getBindingRangeType(this, index);
        }

        public Int IsBindingRangeSpecializable(Int index)
        {
            return Slang.PInvoke.spReflectionTypeLayout_isBindingRangeSpecializable(this, index);
        }

        public Int GetBindingRangeBindingCount(Int index)
        {
            return Slang.PInvoke.spReflectionTypeLayout_getBindingRangeBindingCount(this, index);
        }

        public ReflectionTypeLayout GetBindingRangeLeafTypeLayout(Int index)
        {
            return Slang.PInvoke.spReflectionTypeLayout_getBindingRangeLeafTypeLayout(this, index);
        }

        public ReflectionVariable GetBindingRangeLeafVariable(Int index)
        {
            return Slang.PInvoke.spReflectionTypeLayout_getBindingRangeLeafVariable(this, index);
        }

        public Brutal.SlangApi.ImageFormat GetBindingRangeImageFormat(Int index)
        {
            return Slang.PInvoke.spReflectionTypeLayout_getBindingRangeImageFormat(this, index);
        }

        public Int GetFieldBindingRangeOffset(Int fieldIndex)
        {
            return Slang.PInvoke.spReflectionTypeLayout_getFieldBindingRangeOffset(this, fieldIndex);
        }

        public Int GetExplicitCounterBindingRangeOffset()
        {
            return Slang.PInvoke.spReflectionTypeLayout_getExplicitCounterBindingRangeOffset(this);
        }

        public Int GetBindingRangeDescriptorSetIndex(Int index)
        {
            return Slang.PInvoke.spReflectionTypeLayout_getBindingRangeDescriptorSetIndex(this, index);
        }

        public Int GetBindingRangeFirstDescriptorRangeIndex(Int index)
        {
            return Slang.PInvoke.spReflectionTypeLayout_getBindingRangeFirstDescriptorRangeIndex(this, index);
        }

        public Int GetBindingRangeDescriptorRangeCount(Int index)
        {
            return Slang.PInvoke.spReflectionTypeLayout_getBindingRangeDescriptorRangeCount(this, index);
        }

        public Int GetDescriptorSetCount()
        {
            return Slang.PInvoke.spReflectionTypeLayout_getDescriptorSetCount(this);
        }

        public Int GetDescriptorSetSpaceOffset(Int setIndex)
        {
            return Slang.PInvoke.spReflectionTypeLayout_getDescriptorSetSpaceOffset(this, setIndex);
        }

        public Int GetDescriptorSetDescriptorRangeCount(Int setIndex)
        {
            return Slang.PInvoke.spReflectionTypeLayout_getDescriptorSetDescriptorRangeCount(this, setIndex);
        }

        public Int GetDescriptorSetDescriptorRangeIndexOffset(Int setIndex, Int rangeIndex)
        {
            return Slang.PInvoke.spReflectionTypeLayout_getDescriptorSetDescriptorRangeIndexOffset(
                this,
                setIndex,
                rangeIndex
            );
        }

        public Int GetDescriptorSetDescriptorRangeDescriptorCount(Int setIndex, Int rangeIndex)
        {
            return Slang.PInvoke.spReflectionTypeLayout_getDescriptorSetDescriptorRangeDescriptorCount(
                this,
                setIndex,
                rangeIndex
            );
        }

        public Brutal.SlangApi.BindingType GetDescriptorSetDescriptorRangeType(Int setIndex, Int rangeIndex)
        {
            return Slang.PInvoke.spReflectionTypeLayout_getDescriptorSetDescriptorRangeType(this, setIndex, rangeIndex);
        }

        public Brutal.SlangApi.ParameterCategory GetDescriptorSetDescriptorRangeCategory(Int setIndex, Int rangeIndex)
        {
            return Slang.PInvoke.spReflectionTypeLayout_getDescriptorSetDescriptorRangeCategory(
                this,
                setIndex,
                rangeIndex
            );
        }

        public Int GetSubObjectRangeCount()
        {
            return Slang.PInvoke.spReflectionTypeLayout_getSubObjectRangeCount(this);
        }

        public Int GetSubObjectRangeBindingRangeIndex(Int subObjectRangeIndex)
        {
            return Slang.PInvoke.spReflectionTypeLayout_getSubObjectRangeBindingRangeIndex(this, subObjectRangeIndex);
        }

        public Int GetSubObjectRangeSpaceOffset(Int subObjectRangeIndex)
        {
            return Slang.PInvoke.spReflectionTypeLayout_getSubObjectRangeSpaceOffset(this, subObjectRangeIndex);
        }

        public ReflectionVariableLayout GetSubObjectRangeOffset(Int subObjectRangeIndex)
        {
            return Slang.PInvoke.spReflectionTypeLayout_getSubObjectRangeOffset(this, subObjectRangeIndex);
        }
    };

    [InteropHandle]
    public readonly unsafe partial struct TypeReflection
    {
        public enum Kind
        {
            None = (int)TypeKind.None,
            Struct = (int)TypeKind.Struct,
            Array = (int)TypeKind.Array,
            Matrix = (int)TypeKind.Matrix,
            Vector = (int)TypeKind.Vector,
            Scalar = (int)TypeKind.Scalar,
            ConstantBuffer = (int)TypeKind.ConstantBuffer,
            Resource = (int)TypeKind.Resource,
            SamplerState = (int)TypeKind.SamplerState,
            TextureBuffer = (int)TypeKind.TextureBuffer,
            ShaderStorageBuffer = (int)TypeKind.ShaderStorageBuffer,
            ParameterBlock = (int)TypeKind.ParameterBlock,
            GenericTypeParameter = (int)TypeKind.GenericTypeParameter,
            Interface = (int)TypeKind.Interface,
            OutputStream = (int)TypeKind.OutputStream,
            Specialized = (int)TypeKind.Specialized,
            Feedback = (int)TypeKind.Feedback,
            Pointer = (int)TypeKind.Pointer,
            DynamicResource = (int)TypeKind.DynamicResource,
            MeshOutput = (int)TypeKind.MeshOutput,
            Enum = (int)TypeKind.Enum,
        }

        public enum ScalarType : ScalarTypeIntegral
        {
            None,
            Void,
            Bool,
            Int32,
            UInt32,
            Int64,
            UInt64,
            Float16,
            Float32,
            Float64,
            Int8,
            UInt8,
            Int16,
            UInt16,
            IntPtr,
            UIntPtr,
            BFloat16,
            FloatE4M3,
            FloatE5M2,
        }

        public Brutal.SlangApi.TypeKind GetKind()
        {
            return Slang.PInvoke.spReflectionType_GetKind(this);
        }

        public uint GetUserAttributeCount()
        {
            return Slang.PInvoke.spReflectionType_GetUserAttributeCount(this);
        }

        public Brutal.SlangApi.ReflectionUserAttribute GetUserAttribute(uint index)
        {
            return Slang.PInvoke.spReflectionType_GetUserAttribute(this, index);
        }

        public Brutal.SlangApi.ReflectionUserAttribute FindUserAttributeByName(RefString8 name)
        {
            return Slang.PInvoke.spReflectionType_FindUserAttributeByName(this, name);
        }

        public ReflectionType ApplySpecializations(ReflectionGeneric generic)
        {
            return Slang.PInvoke.spReflectionType_applySpecializations(this, generic);
        }

        public uint GetFieldCount()
        {
            return Slang.PInvoke.spReflectionType_GetFieldCount(this);
        }

        public ReflectionVariable GetFieldByIndex(uint index)
        {
            return Slang.PInvoke.spReflectionType_GetFieldByIndex(this, index);
        }

        public ByteSize64 GetElementCount()
        {
            return Slang.PInvoke.spReflectionType_GetElementCount(this);
        }

        public ByteSize64 GetSpecializedElementCount(Reflection reflection)
        {
            return Slang.PInvoke.spReflectionType_GetSpecializedElementCount(this, reflection);
        }

        public ReflectionType GetElementType()
        {
            return Slang.PInvoke.spReflectionType_GetElementType(this);
        }

        public uint GetRowCount()
        {
            return Slang.PInvoke.spReflectionType_GetRowCount(this);
        }

        public uint GetColumnCount()
        {
            return Slang.PInvoke.spReflectionType_GetColumnCount(this);
        }

        public Brutal.SlangApi.ScalarType GetScalarType()
        {
            return Slang.PInvoke.spReflectionType_GetScalarType(this);
        }

        public Brutal.SlangApi.ResourceShape GetResourceShape()
        {
            return Slang.PInvoke.spReflectionType_GetResourceShape(this);
        }

        public Brutal.SlangApi.ResourceAccess GetResourceAccess()
        {
            return Slang.PInvoke.spReflectionType_GetResourceAccess(this);
        }

        public ReflectionType GetResourceResultType()
        {
            return Slang.PInvoke.spReflectionType_GetResourceResultType(this);
        }

        public string GetName()
        {
            return Slang.PInvoke.spReflectionType_GetName(this);
        }

        internal Result GetFullName(out nint outNameBlob)
        {
            return Slang.PInvoke.spReflectionType_GetFullName(this, out outNameBlob);
        }

        public ReflectionGeneric GetGenericContainer()
        {
            return Slang.PInvoke.spReflectionType_GetGenericContainer(this);
        }

        public Int GetSpecializedTypeArgCount()
        {
            return Slang.PInvoke.spReflectionType_getSpecializedTypeArgCount(this);
        }

        public ReflectionType GetSpecializedTypeArgType(Int index)
        {
            return Slang.PInvoke.spReflectionType_getSpecializedTypeArgType(this, index);
        }
    };

    [InteropHandle]
    public readonly unsafe partial struct VariableLayoutReflection
    {
        public ReflectionVariable GetVariable()
        {
            return Slang.PInvoke.spReflectionVariableLayout_GetVariable(this);
        }

        public ReflectionTypeLayout GetTypeLayout()
        {
            return Slang.PInvoke.spReflectionVariableLayout_GetTypeLayout(this);
        }

        public ByteSize64 GetOffset(Brutal.SlangApi.ParameterCategory category)
        {
            return Slang.PInvoke.spReflectionVariableLayout_GetOffset(this, category);
        }

        public ByteSize64 GetSpace(Brutal.SlangApi.ParameterCategory category)
        {
            return Slang.PInvoke.spReflectionVariableLayout_GetSpace(this, category);
        }

        public Brutal.SlangApi.ImageFormat GetImageFormat()
        {
            return Slang.PInvoke.spReflectionVariableLayout_GetImageFormat(this);
        }

        public string GetSemanticName()
        {
            return Slang.PInvoke.spReflectionVariableLayout_GetSemanticName(this);
        }

        public ByteSize64 GetSemanticIndex()
        {
            return Slang.PInvoke.spReflectionVariableLayout_GetSemanticIndex(this);
        }

        public Brutal.SlangApi.Stage GetStage()
        {
            return Slang.PInvoke.spReflectionVariableLayout_getStage(this);
        }

        public ReflectionVariableLayout GetPendingDataLayout()
        {
            return Slang.PInvoke.spReflectionVariableLayout_getPendingDataLayout(this);
        }
    };

    [InteropHandle]
    public readonly unsafe partial struct VariableReflection
    {
        public string GetName()
        {
            return Slang.PInvoke.spReflectionVariable_GetName(this);
        }

        public new ReflectionType GetType()
        {
            return Slang.PInvoke.spReflectionVariable_GetType(this);
        }

        public Brutal.SlangApi.ReflectionModifier FindModifier(Brutal.SlangApi.ModifierID modifierID)
        {
            return Slang.PInvoke.spReflectionVariable_FindModifier(this, modifierID);
        }

        public uint GetUserAttributeCount()
        {
            return Slang.PInvoke.spReflectionVariable_GetUserAttributeCount(this);
        }

        public Brutal.SlangApi.ReflectionUserAttribute GetUserAttribute(uint index)
        {
            return Slang.PInvoke.spReflectionVariable_GetUserAttribute(this, index);
        }

        internal Brutal.SlangApi.ReflectionUserAttribute FindUserAttributeByName(nint globalSession, RefString8 name)
        {
            return Slang.PInvoke.spReflectionVariable_FindUserAttributeByName(this, globalSession, name);
        }

        public Brutal.Bool8 HasDefaultValue()
        {
            return Slang.PInvoke.spReflectionVariable_HasDefaultValue(this);
        }

        public unsafe Result GetDefaultValueInt(long* rs)
        {
            return Slang.PInvoke.spReflectionVariable_GetDefaultValueInt(this, rs);
        }

        public unsafe Result GetDefaultValueFloat(float* rs)
        {
            return Slang.PInvoke.spReflectionVariable_GetDefaultValueFloat(this, rs);
        }

        public ReflectionGeneric GetGenericContainer()
        {
            return Slang.PInvoke.spReflectionVariable_GetGenericContainer(this);
        }

        public ReflectionVariable ApplySpecializations(ReflectionGeneric generic)
        {
            return Slang.PInvoke.spReflectionVariable_applySpecializations(this, generic);
        }
    };

    [InteropHandle]
    public readonly unsafe partial struct FunctionReflection
    {
        public ReflectionDecl AsDecl()
        {
            return Slang.PInvoke.spReflectionFunction_asDecl(this);
        }

        public string GetName()
        {
            return Slang.PInvoke.spReflectionFunction_GetName(this);
        }

        public Brutal.SlangApi.ReflectionModifier FindModifier(Brutal.SlangApi.ModifierID modifierID)
        {
            return Slang.PInvoke.spReflectionFunction_FindModifier(this, modifierID);
        }

        public uint GetUserAttributeCount()
        {
            return Slang.PInvoke.spReflectionFunction_GetUserAttributeCount(this);
        }

        public Brutal.SlangApi.ReflectionUserAttribute GetUserAttribute(uint index)
        {
            return Slang.PInvoke.spReflectionFunction_GetUserAttribute(this, index);
        }

        internal Brutal.SlangApi.ReflectionUserAttribute FindUserAttributeByName(nint globalSession, RefString8 name)
        {
            return Slang.PInvoke.spReflectionFunction_FindUserAttributeByName(this, globalSession, name);
        }

        public uint GetParameterCount()
        {
            return Slang.PInvoke.spReflectionFunction_GetParameterCount(this);
        }

        public ReflectionVariable GetParameter(uint index)
        {
            return Slang.PInvoke.spReflectionFunction_GetParameter(this, index);
        }

        public ReflectionType GetResultType()
        {
            return Slang.PInvoke.spReflectionFunction_GetResultType(this);
        }

        public ReflectionGeneric GetGenericContainer()
        {
            return Slang.PInvoke.spReflectionFunction_GetGenericContainer(this);
        }

        public ReflectionFunction ApplySpecializations(ReflectionGeneric generic)
        {
            return Slang.PInvoke.spReflectionFunction_applySpecializations(this, generic);
        }

        public ReflectionFunction SpecializeWithArgTypes(Int argTypeCount, in ReflectionType argTypes)
        {
            return Slang.PInvoke.spReflectionFunction_specializeWithArgTypes(this, argTypeCount, in argTypes);
        }

        public Brutal.Bool8 IsOverloaded()
        {
            return Slang.PInvoke.spReflectionFunction_isOverloaded(this);
        }

        public uint GetOverloadCount()
        {
            return Slang.PInvoke.spReflectionFunction_getOverloadCount(this);
        }

        public ReflectionFunction GetOverload(uint index)
        {
            return Slang.PInvoke.spReflectionFunction_getOverload(this, index);
        }
    };

    [InteropHandle]
    public readonly unsafe partial struct GenericReflection
    {
        public ReflectionDecl AsDecl()
        {
            return Slang.PInvoke.spReflectionGeneric_asDecl(this);
        }

        public string GetName()
        {
            return Slang.PInvoke.spReflectionGeneric_GetName(this);
        }

        public uint GetTypeParameterCount()
        {
            return Slang.PInvoke.spReflectionGeneric_GetTypeParameterCount(this);
        }

        public ReflectionVariable GetTypeParameter(uint index)
        {
            return Slang.PInvoke.spReflectionGeneric_GetTypeParameter(this, index);
        }

        public uint GetValueParameterCount()
        {
            return Slang.PInvoke.spReflectionGeneric_GetValueParameterCount(this);
        }

        public ReflectionVariable GetValueParameter(uint index)
        {
            return Slang.PInvoke.spReflectionGeneric_GetValueParameter(this, index);
        }

        public uint GetTypeParameterConstraintCount(ReflectionVariable typeParam)
        {
            return Slang.PInvoke.spReflectionGeneric_GetTypeParameterConstraintCount(this, typeParam);
        }

        public ReflectionType GetTypeParameterConstraintType(ReflectionVariable typeParam, uint index)
        {
            return Slang.PInvoke.spReflectionGeneric_GetTypeParameterConstraintType(this, typeParam, index);
        }

        public Brutal.SlangApi.DeclKind GetInnerKind()
        {
            return Slang.PInvoke.spReflectionGeneric_GetInnerKind(this);
        }

        public ReflectionDecl GetInnerDecl()
        {
            return Slang.PInvoke.spReflectionGeneric_GetInnerDecl(this);
        }

        public ReflectionGeneric GetOuterGenericContainer()
        {
            return Slang.PInvoke.spReflectionGeneric_GetOuterGenericContainer(this);
        }

        public ReflectionType GetConcreteType(ReflectionVariable typeParam)
        {
            return Slang.PInvoke.spReflectionGeneric_GetConcreteType(this, typeParam);
        }

        public long GetConcreteIntVal(ReflectionVariable valueParam)
        {
            return Slang.PInvoke.spReflectionGeneric_GetConcreteIntVal(this, valueParam);
        }

        public ReflectionGeneric ApplySpecializations(ReflectionGeneric generic)
        {
            return Slang.PInvoke.spReflectionGeneric_applySpecializations(this, generic);
        }
    };

    [InteropHandle]
    public readonly unsafe partial struct Attribute { };

    [InteropHandle]
    public readonly unsafe partial struct ShaderReflection { };

    [InteropHandle]
    public readonly unsafe partial struct Modifier
    {
        public enum Id : ModifierIDIntegral
        {
            Shared = (ModifierIDIntegral)ModifierID.Shared,
            NoDiff = (ModifierIDIntegral)ModifierID.NoDiff,
            Static = (ModifierIDIntegral)ModifierID.Static,
            Const = (ModifierIDIntegral)ModifierID.Const,
            Export = (ModifierIDIntegral)ModifierID.Export,
            Extern = (ModifierIDIntegral)ModifierID.Extern,
            Differentiable = (ModifierIDIntegral)ModifierID.Differentiable,
            Mutating = (ModifierIDIntegral)ModifierID.Mutating,
            In = (ModifierIDIntegral)ModifierID.In,
            Out = (ModifierIDIntegral)ModifierID.Out,
            InOut = (ModifierIDIntegral)ModifierID.InOut,
        }
    };

    [InteropHandle]
    public readonly unsafe partial struct EntryPointReflection { };

    [InteropHandle]
    public readonly unsafe partial struct TypeParameterReflection
    {
        public string GetName()
        {
            return Slang.PInvoke.spReflectionTypeParameter_GetName(this);
        }

        public uint GetIndex()
        {
            return Slang.PInvoke.spReflectionTypeParameter_GetIndex(this);
        }

        public uint GetConstraintCount()
        {
            return Slang.PInvoke.spReflectionTypeParameter_GetConstraintCount(this);
        }

        public ReflectionType GetConstraintByIndex(uint index)
        {
            return Slang.PInvoke.spReflectionTypeParameter_GetConstraintByIndex(this, index);
        }
    };

    [InteropHandle]
    public readonly unsafe partial struct CompileCoreModuleFlag
    {
        public enum Enum : CompileCoreModuleFlags
        {
            WriteDocumentation = 0X1,
        }
    };

    public unsafe partial struct SessionDesc
    {
        ///  The size of this structure, in bytes.
        public ByteSize64 StructureSize;

        ///  Code generation targets to include in the session.
        public Brutal.SlangApi.TargetDesc* Targets;
        public Int TargetCount;

        ///  Flags to configure the session.
        public SessionFlags Flags;

        ///  Default layout to assume for variables with matrix types.
        public Brutal.SlangApi.MatrixLayoutMode DefaultMatrixLayoutMode;

        ///  Paths to use when searching for `#include`d or `import`ed files.
        public byte** SearchPaths;
        public Int SearchPathCount;
        public Brutal.SlangApi.PreprocessorMacroDesc* PreprocessorMacros;
        public Int PreprocessorMacroCount;
        public nint FileSystem;
        public Brutal.Bool8 EnableEffectAnnotations;
        public Brutal.Bool8 AllowGlslSyntax;

        ///  Pointer to an array of compiler option entries, whose size is compilerOptionEntryCount.
        public Brutal.SlangApi.CompilerOptionEntry* CompilerOptionEntries;

        ///  Number of additional compiler option entries.
        public uint CompilerOptionEntryCount;

        ///  Whether to skip SPIRV validation.
        public Brutal.Bool8 SkipSpirvValidation;
    };

    ///  Argument used for specialization to types/values.
    [StructLayout(LayoutKind.Explicit)]
    public unsafe partial struct SpecializationArg
    {
        public enum Kind
        {
            Unknown = 0, // < An invalid specialization argument.
            Type = 1, // < Specialize to a type.
            Expr = 2, // < An expression representing a type or value
        }

        ///  The kind of specialization argument.
        [FieldOffset(0)]
        public Brutal.SlangApi.SpecializationArg.Kind _Kind;

        ///  A type specialization argument, used for `Kind::Type`.
        [FieldOffset(8)]
        public Brutal.SlangApi.TypeReflection Type;

        ///  An expression in Slang syntax, used for `Kind::Expr`.
        [FieldOffset(8)]
        public byte* Expr;
    };

    ///  Description of a code generation target.
    public unsafe partial struct TargetDesc
    {
        ///  The size of this structure, in bytes.
        public ByteSize64 StructureSize;

        ///  The target format to generate code for (e.g., SPIR-V, DXIL, etc.)
        public Brutal.SlangApi.CompileTarget Format;

        ///  The compilation profile supported by the target (e.g., "Shader Model 5.1")
        public Brutal.SlangApi.ProfileID Profile;

        ///  Flags for the code generation target. Currently unused.
        public TargetFlags Flags;

        ///  Default mode to use for floating-point operations on the target.
        public Brutal.SlangApi.FloatingPointMode FloatingPointMode;

        ///  The line directive mode for output source code.
        public Brutal.SlangApi.LineDirectiveMode LineDirectiveMode;

        ///  Whether to force `scalar` layout for glsl shader storage buffers.
        public Brutal.Bool8 ForceGlslScalarBufferLayout;

        ///  Pointer to an array of compiler option entries, whose size is compilerOptionEntryCount.
        public Brutal.SlangApi.CompilerOptionEntry* CompilerOptionEntries;

        ///  Number of additional compiler option entries.
        public uint CompilerOptionEntryCount;
    };

    public unsafe partial struct PreprocessorMacroDesc
    {
        public byte* Name;
        public byte* Value;
    };

    public unsafe partial struct SourceLocation
    {
        public byte* FilePath;
        public Int Line;
        public Int Column;
    };

    public unsafe partial struct VMExecOperand
    {
        public byte** Section; // Pointer to the section start pointer.
        public uint Type; // type of the operand data.
        public uint Size;
        public uint Offset;
    };

    public unsafe partial struct VMExecInstHeader
    {
        public VMExtFunction FunctionPtr; // Pointer to the function that executes this instruction.
        public uint OpcodeExtension;
        public uint OperandCount;
    };

    public unsafe partial struct ByteCodeFuncInfo
    {
        public uint ParameterCount;
        public uint ReturnValueSize;
    };

    public unsafe partial struct ByteCodeRunnerDesc
    {
        ///  The size of this structure, in bytes.
        public ByteSize64 StructSize;
    };

    ///  A "Universally Unique Identifier" (UUID)
    ///
    /// The Slang API uses UUIDs to identify interfaces when
    /// using `queryInterface`.
    ///
    /// This type is compatible with the `GUID` type defined
    /// by the Component Object Model (COM), but Slang is
    /// not dependent on COM.
    public unsafe partial struct Uuid
    {
        public uint Data1;
        public ushort Data2;
        public ushort Data3;
        public fixed byte Data4[8];
    };

    public unsafe partial struct Guid { };

    ///  Can be requested from ISlangCastable cast to indicate the contained chars are null
    /// terminated.
    public unsafe partial struct TerminatedChars
    {
        public fixed byte Chars[1];
    };

    [InteropHandle]
    public readonly unsafe partial struct ProgramLayout { };

    public unsafe partial struct EntryPoint { };

    [InteropHandle]
    public readonly unsafe partial struct EntryPointLayout { };

    public unsafe partial struct ReflectionModifier { };

    [InteropHandle]
    public readonly unsafe partial struct ReflectionUserAttribute
    {
        public string GetName()
        {
            return Slang.PInvoke.spReflectionUserAttribute_GetName(this);
        }

        public uint GetArgumentCount()
        {
            return Slang.PInvoke.spReflectionUserAttribute_GetArgumentCount(this);
        }

        public ReflectionType GetArgumentType(uint index)
        {
            return Slang.PInvoke.spReflectionUserAttribute_GetArgumentType(this, index);
        }

        public unsafe Result GetArgumentValueInt(uint index, int* rs)
        {
            return Slang.PInvoke.spReflectionUserAttribute_GetArgumentValueInt(this, index, rs);
        }

        public unsafe Result GetArgumentValueFloat(uint index, float* rs)
        {
            return Slang.PInvoke.spReflectionUserAttribute_GetArgumentValueFloat(this, index, rs);
        }

        public string GetArgumentValueString(uint index, out ByteSize64 outSize)
        {
            return Slang.PInvoke.spReflectionUserAttribute_GetArgumentValueString(this, index, out outSize);
        }
    };

    ///  Description of a Slang global session.
    public unsafe partial struct GlobalSessionDesc
    {
        /// Size of this struct.
        public uint StructureSize;

        /// Slang API version.
        public uint ApiVersion;

        /// Specify the oldest Slang language version that any sessions will use.
        public uint MinLanguageVersion;

        /// Whether to enable GLSL support.
        public Brutal.Bool8 EnableGlsl;

        /// Reserved for future use.
        public fixed uint Reserved[16];
    };

    [StructLayout(LayoutKind.Explicit)]
    public unsafe partial struct GenericArgReflection
    {
        [FieldOffset(0)]
        public Brutal.SlangApi.TypeReflection TypeVal;

        [FieldOffset(0)]
        public long IntVal;

        [FieldOffset(0)]
        public Brutal.Bool8 BoolVal;
    };

    [StructLayout(LayoutKind.Explicit)]
    public unsafe partial struct ReflectionGenericArg
    {
        [FieldOffset(0)]
        public ReflectionType TypeVal;

        [FieldOffset(0)]
        public long IntVal;

        [FieldOffset(0)]
        public Brutal.Bool8 BoolVal;
    };
}
