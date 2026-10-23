using Brutal.Strings;

namespace Brutal.SlangApi
{
    public static partial class Slang
    {
        public static unsafe partial class PInvoke
        {
            [LibraryImport(
                "slang",
                StringMarshalling = StringMarshalling.Custom,
                StringMarshallingCustomType = typeof(Brutal.Interop.InteropStringMarshaller)
            )]
            public static partial string spGetBuildTagString();

            [LibraryImport("slang")]
            public static partial nint spCreateSession(RefString8 deprecated);

            [LibraryImport("slang")]
            public static partial void spDestroySession(nint session);

            [LibraryImport("slang")]
            public static partial void spSessionSetSharedLibraryLoader(nint session, nint loader);

            [LibraryImport("slang")]
            public static partial nint spSessionGetSharedLibraryLoader(nint session);

            [LibraryImport("slang")]
            public static partial Result spSessionCheckCompileTargetSupport(
                nint session,
                Brutal.SlangApi.CompileTarget target
            );

            [LibraryImport("slang")]
            public static partial Result spSessionCheckPassThroughSupport(
                nint session,
                Brutal.SlangApi.PassThrough passThrough
            );

            [LibraryImport("slang")]
            public static partial void spAddBuiltins(nint session, RefString8 sourcePath, RefString8 sourceString);

            [LibraryImport("slang")]
            public static partial CompileRequest spCreateCompileRequest(nint session);

            [LibraryImport("slang")]
            public static partial void spDestroyCompileRequest(CompileRequest request);

            [LibraryImport("slang")]
            public static partial void spSetFileSystem(CompileRequest request, nint fileSystem);

            [LibraryImport("slang")]
            public static partial void spSetCompileFlags(CompileRequest request, CompileFlags flags);

            [LibraryImport("slang")]
            public static partial CompileFlags spGetCompileFlags(CompileRequest request);

            [LibraryImport("slang")]
            public static partial void spSetDumpIntermediates(CompileRequest request, int enable);

            [LibraryImport("slang")]
            public static partial void spSetDumpIntermediatePrefix(CompileRequest request, RefString8 prefix);

            [LibraryImport("slang")]
            public static partial void spSetLineDirectiveMode(
                CompileRequest request,
                Brutal.SlangApi.LineDirectiveMode mode
            );

            [LibraryImport("slang")]
            public static partial void spSetTargetLineDirectiveMode(
                CompileRequest request,
                int targetIndex,
                Brutal.SlangApi.LineDirectiveMode mode
            );

            [LibraryImport("slang")]
            public static partial void spSetTargetForceGLSLScalarBufferLayout(
                CompileRequest request,
                int targetIndex,
                Brutal.Bool8 forceScalarLayout
            );

            [LibraryImport("slang")]
            public static partial void spSetTargetUseMinimumSlangOptimization(
                Brutal.SlangApi.ICompileRequest request,
                int targetIndex,
                Brutal.Bool8 val
            );

            [LibraryImport("slang")]
            public static partial void spSetIgnoreCapabilityCheck(
                Brutal.SlangApi.ICompileRequest request,
                Brutal.Bool8 val
            );

            [LibraryImport("slang")]
            public static partial void spSetCodeGenTarget(CompileRequest request, Brutal.SlangApi.CompileTarget target);

            [LibraryImport("slang")]
            public static partial int spAddCodeGenTarget(CompileRequest request, Brutal.SlangApi.CompileTarget target);

            [LibraryImport("slang")]
            public static partial void spSetTargetProfile(
                CompileRequest request,
                int targetIndex,
                Brutal.SlangApi.ProfileID profile
            );

            [LibraryImport("slang")]
            public static partial void spSetTargetFlags(CompileRequest request, int targetIndex, TargetFlags flags);

            [LibraryImport("slang")]
            public static partial void spSetTargetFloatingPointMode(
                CompileRequest request,
                int targetIndex,
                Brutal.SlangApi.FloatingPointMode mode
            );

            [LibraryImport("slang")]
            public static partial void spAddTargetCapability(
                Brutal.SlangApi.ICompileRequest request,
                int targetIndex,
                Brutal.SlangApi.CapabilityID capability
            );

            [LibraryImport("slang")]
            public static partial void spSetTargetMatrixLayoutMode(
                CompileRequest request,
                int targetIndex,
                Brutal.SlangApi.MatrixLayoutMode mode
            );

            [LibraryImport("slang")]
            public static partial void spSetMatrixLayoutMode(
                CompileRequest request,
                Brutal.SlangApi.MatrixLayoutMode mode
            );

            [LibraryImport("slang")]
            public static partial void spSetDebugInfoLevel(
                CompileRequest request,
                Brutal.SlangApi.DebugInfoLevel level
            );

            [LibraryImport("slang")]
            public static partial void spSetDebugInfoFormat(
                CompileRequest request,
                Brutal.SlangApi.DebugInfoFormat format
            );

            [LibraryImport("slang")]
            public static partial void spSetOptimizationLevel(
                CompileRequest request,
                Brutal.SlangApi.OptimizationLevel level
            );

            [LibraryImport("slang")]
            public static partial void spSetOutputContainerFormat(
                CompileRequest request,
                Brutal.SlangApi.ContainerFormat format
            );

            [LibraryImport("slang")]
            public static partial void spSetPassThrough(
                CompileRequest request,
                Brutal.SlangApi.PassThrough passThrough
            );

            [LibraryImport("slang")]
            public static partial void spSetDiagnosticCallback(
                CompileRequest request,
                DiagnosticCallback callback,
                void* userData
            );

            [LibraryImport("slang")]
            public static partial void spSetWriter(
                CompileRequest request,
                Brutal.SlangApi.WriterChannel channel,
                nint writer
            );

            [LibraryImport("slang")]
            public static partial nint spGetWriter(CompileRequest request, Brutal.SlangApi.WriterChannel channel);

            [LibraryImport("slang")]
            public static partial void spAddSearchPath(CompileRequest request, RefString8 searchDir);

            [LibraryImport("slang")]
            public static partial void spAddPreprocessorDefine(
                CompileRequest request,
                RefString8 key,
                RefString8 value
            );

            [LibraryImport("slang")]
            public static partial Result spProcessCommandLineArguments(
                CompileRequest request,
                byte** args,
                int argCount
            );

            [LibraryImport("slang")]
            public static partial int spAddTranslationUnit(
                CompileRequest request,
                Brutal.SlangApi.SourceLanguage language,
                RefString8 name
            );

            [LibraryImport("slang")]
            public static partial void spSetDefaultModuleName(CompileRequest request, RefString8 defaultModuleName);

            [LibraryImport("slang")]
            public static partial void spTranslationUnit_addPreprocessorDefine(
                CompileRequest request,
                int translationUnitIndex,
                RefString8 key,
                RefString8 value
            );

            [LibraryImport("slang")]
            public static partial void spAddTranslationUnitSourceFile(
                CompileRequest request,
                int translationUnitIndex,
                RefString8 path
            );

            [LibraryImport("slang")]
            public static partial void spAddTranslationUnitSourceString(
                CompileRequest request,
                int translationUnitIndex,
                RefString8 path,
                RefString8 source
            );

            [LibraryImport("slang")]
            public static partial Result spAddLibraryReference(
                CompileRequest request,
                RefString8 basePath,
                void* libData,
                ByteSize libDataSize
            );

            [LibraryImport("slang")]
            public static partial void spAddTranslationUnitSourceStringSpan(
                CompileRequest request,
                int translationUnitIndex,
                RefString8 path,
                RefString8 sourceBegin,
                RefString8 sourceEnd
            );

            [LibraryImport("slang")]
            public static partial void spAddTranslationUnitSourceBlob(
                CompileRequest request,
                int translationUnitIndex,
                RefString8 path,
                nint sourceBlob
            );

            [LibraryImport("slang")]
            public static partial Brutal.SlangApi.ProfileID spFindProfile(nint session, RefString8 name);

            [LibraryImport("slang")]
            public static partial Brutal.SlangApi.CapabilityID spFindCapability(nint session, RefString8 name);

            [LibraryImport("slang")]
            public static partial int spAddEntryPoint(
                CompileRequest request,
                int translationUnitIndex,
                RefString8 name,
                Brutal.SlangApi.Stage stage
            );

            [LibraryImport("slang")]
            public static partial int spAddEntryPointEx(
                CompileRequest request,
                int translationUnitIndex,
                RefString8 name,
                Brutal.SlangApi.Stage stage,
                int genericArgCount,
                byte** genericArgs
            );

            [LibraryImport("slang")]
            public static partial Result spSetGlobalGenericArgs(
                CompileRequest request,
                int genericArgCount,
                byte** genericArgs
            );

            [LibraryImport("slang")]
            public static partial Result spSetTypeNameForGlobalExistentialTypeParam(
                CompileRequest request,
                int slotIndex,
                RefString8 typeName
            );

            [LibraryImport("slang")]
            public static partial Result spSetTypeNameForEntryPointExistentialTypeParam(
                CompileRequest request,
                int entryPointIndex,
                int slotIndex,
                RefString8 typeName
            );

            [LibraryImport("slang")]
            public static partial Result spCompile(CompileRequest request);

            [LibraryImport(
                "slang",
                StringMarshalling = StringMarshalling.Custom,
                StringMarshallingCustomType = typeof(Brutal.Interop.InteropStringMarshaller)
            )]
            public static partial string spGetDiagnosticOutput(CompileRequest request);

            [LibraryImport("slang")]
            public static partial Result spGetDiagnosticOutputBlob(CompileRequest request, out nint outBlob);

            [LibraryImport("slang")]
            public static partial int spGetDependencyFileCount(CompileRequest request);

            [LibraryImport(
                "slang",
                StringMarshalling = StringMarshalling.Custom,
                StringMarshallingCustomType = typeof(Brutal.Interop.InteropStringMarshaller)
            )]
            public static partial string spGetDependencyFilePath(CompileRequest request, int index);

            [LibraryImport("slang")]
            public static partial int spGetTranslationUnitCount(CompileRequest request);

            [LibraryImport(
                "slang",
                StringMarshalling = StringMarshalling.Custom,
                StringMarshallingCustomType = typeof(Brutal.Interop.InteropStringMarshaller)
            )]
            public static partial string spGetEntryPointSource(CompileRequest request, int entryPointIndex);

            [LibraryImport("slang")]
            public static partial void* spGetEntryPointCode(
                CompileRequest request,
                int entryPointIndex,
                out ByteSize outSize
            );

            [LibraryImport("slang")]
            public static partial Result spGetEntryPointCodeBlob(
                CompileRequest request,
                int entryPointIndex,
                int targetIndex,
                out nint outBlob
            );

            [LibraryImport("slang")]
            public static partial Result spGetEntryPointHostCallable(
                CompileRequest request,
                int entryPointIndex,
                int targetIndex,
                out nint outSharedLibrary
            );

            [LibraryImport("slang")]
            public static partial Result spGetTargetCodeBlob(CompileRequest request, int targetIndex, out nint outBlob);

            [LibraryImport("slang")]
            public static partial Result spGetTargetHostCallable(
                CompileRequest request,
                int targetIndex,
                out nint outSharedLibrary
            );

            [LibraryImport("slang")]
            public static partial void* spGetCompileRequestCode(CompileRequest request, out ByteSize outSize);

            [LibraryImport("slang")]
            public static partial Result spGetContainerCode(CompileRequest request, out nint outBlob);

            [LibraryImport("slang")]
            public static partial Result spLoadRepro(
                CompileRequest request,
                nint fileSystem,
                void* data,
                ByteSize size
            );

            [LibraryImport("slang")]
            public static partial Result spSaveRepro(CompileRequest request, out nint outBlob);

            [LibraryImport("slang")]
            public static partial Result spEnableReproCapture(CompileRequest request);

            [LibraryImport("slang")]
            public static partial Result spGetCompileTimeProfile(
                CompileRequest request,
                nint* compileTimeProfile,
                Brutal.Bool8 shouldClear
            );

            [LibraryImport("slang")]
            public static partial Result spExtractRepro(
                nint session,
                void* reproData,
                ByteSize reproDataSize,
                nint fileSystem
            );

            [LibraryImport("slang")]
            public static partial Result spLoadReproAsFileSystem(
                nint session,
                void* reproData,
                ByteSize reproDataSize,
                nint replaceFileSystem,
                out nint outFileSystem
            );

            [LibraryImport("slang")]
            public static partial void spOverrideDiagnosticSeverity(
                CompileRequest request,
                Int messageID,
                Brutal.SlangApi.Severity overrideSeverity
            );

            [LibraryImport("slang")]
            public static partial DiagnosticFlags spGetDiagnosticFlags(CompileRequest request);

            [LibraryImport("slang")]
            public static partial void spSetDiagnosticFlags(CompileRequest request, DiagnosticFlags flags);

            [LibraryImport("slang")]
            public static partial Reflection spGetReflection(CompileRequest request);

            [LibraryImport(
                "slang",
                StringMarshalling = StringMarshalling.Custom,
                StringMarshallingCustomType = typeof(Brutal.Interop.InteropStringMarshaller)
            )]
            public static partial string spReflectionUserAttribute_GetName(
                Brutal.SlangApi.ReflectionUserAttribute attrib
            );

            [LibraryImport("slang")]
            public static partial uint spReflectionUserAttribute_GetArgumentCount(
                Brutal.SlangApi.ReflectionUserAttribute attrib
            );

            [LibraryImport("slang")]
            public static partial ReflectionType spReflectionUserAttribute_GetArgumentType(
                Brutal.SlangApi.ReflectionUserAttribute attrib,
                uint index
            );

            [LibraryImport("slang")]
            public static partial Result spReflectionUserAttribute_GetArgumentValueInt(
                Brutal.SlangApi.ReflectionUserAttribute attrib,
                uint index,
                int* rs
            );

            [LibraryImport("slang")]
            public static partial Result spReflectionUserAttribute_GetArgumentValueFloat(
                Brutal.SlangApi.ReflectionUserAttribute attrib,
                uint index,
                float* rs
            );

            [LibraryImport(
                "slang",
                StringMarshalling = StringMarshalling.Custom,
                StringMarshallingCustomType = typeof(Brutal.Interop.InteropStringMarshaller)
            )]
            public static partial string spReflectionUserAttribute_GetArgumentValueString(
                Brutal.SlangApi.ReflectionUserAttribute attrib,
                uint index,
                out ByteSize outSize
            );

            [LibraryImport("slang")]
            public static partial Brutal.SlangApi.TypeKind spReflectionType_GetKind(ReflectionType type);

            [LibraryImport("slang")]
            public static partial uint spReflectionType_GetUserAttributeCount(ReflectionType type);

            [LibraryImport("slang")]
            public static partial Brutal.SlangApi.ReflectionUserAttribute spReflectionType_GetUserAttribute(
                ReflectionType type,
                uint index
            );

            [LibraryImport("slang")]
            public static partial Brutal.SlangApi.ReflectionUserAttribute spReflectionType_FindUserAttributeByName(
                ReflectionType type,
                RefString8 name
            );

            [LibraryImport("slang")]
            public static partial ReflectionType spReflectionType_applySpecializations(
                ReflectionType type,
                ReflectionGeneric generic
            );

            [LibraryImport("slang")]
            public static partial uint spReflectionType_GetFieldCount(ReflectionType type);

            [LibraryImport("slang")]
            public static partial ReflectionVariable spReflectionType_GetFieldByIndex(ReflectionType type, uint index);

            [LibraryImport("slang")]
            public static partial ByteSize spReflectionType_GetElementCount(ReflectionType type);

            [LibraryImport("slang")]
            public static partial ByteSize spReflectionType_GetSpecializedElementCount(
                ReflectionType type,
                Reflection reflection
            );

            [LibraryImport("slang")]
            public static partial ReflectionType spReflectionType_GetElementType(ReflectionType type);

            [LibraryImport("slang")]
            public static partial uint spReflectionType_GetRowCount(ReflectionType type);

            [LibraryImport("slang")]
            public static partial uint spReflectionType_GetColumnCount(ReflectionType type);

            [LibraryImport("slang")]
            public static partial Brutal.SlangApi.ScalarType spReflectionType_GetScalarType(ReflectionType type);

            [LibraryImport("slang")]
            public static partial Brutal.SlangApi.ResourceShape spReflectionType_GetResourceShape(ReflectionType type);

            [LibraryImport("slang")]
            public static partial Brutal.SlangApi.ResourceAccess spReflectionType_GetResourceAccess(
                ReflectionType type
            );

            [LibraryImport("slang")]
            public static partial ReflectionType spReflectionType_GetResourceResultType(ReflectionType type);

            [LibraryImport(
                "slang",
                StringMarshalling = StringMarshalling.Custom,
                StringMarshallingCustomType = typeof(Brutal.Interop.InteropStringMarshaller)
            )]
            public static partial string spReflectionType_GetName(ReflectionType type);

            [LibraryImport("slang")]
            public static partial Result spReflectionType_GetFullName(ReflectionType type, out nint outNameBlob);

            [LibraryImport("slang")]
            public static partial ReflectionGeneric spReflectionType_GetGenericContainer(ReflectionType type);

            [LibraryImport("slang")]
            public static partial ReflectionType spReflectionTypeLayout_GetType(ReflectionTypeLayout type);

            [LibraryImport("slang")]
            public static partial Brutal.SlangApi.TypeKind spReflectionTypeLayout_getKind(ReflectionTypeLayout type);

            [LibraryImport("slang")]
            public static partial ByteSize spReflectionTypeLayout_GetSize(
                ReflectionTypeLayout type,
                Brutal.SlangApi.ParameterCategory category
            );

            [LibraryImport("slang")]
            public static partial ByteSize spReflectionTypeLayout_GetStride(
                ReflectionTypeLayout type,
                Brutal.SlangApi.ParameterCategory category
            );

            [LibraryImport("slang")]
            public static partial int spReflectionTypeLayout_getAlignment(
                ReflectionTypeLayout type,
                Brutal.SlangApi.ParameterCategory category
            );

            [LibraryImport("slang")]
            public static partial uint spReflectionTypeLayout_GetFieldCount(ReflectionTypeLayout type);

            [LibraryImport("slang")]
            public static partial ReflectionVariableLayout spReflectionTypeLayout_GetFieldByIndex(
                ReflectionTypeLayout type,
                uint index
            );

            [LibraryImport("slang")]
            public static partial Int spReflectionTypeLayout_findFieldIndexByName(
                ReflectionTypeLayout typeLayout,
                RefString8 nameBegin,
                RefString8 nameEnd
            );

            [LibraryImport("slang")]
            public static partial ReflectionVariableLayout spReflectionTypeLayout_GetExplicitCounter(
                ReflectionTypeLayout typeLayout
            );

            [LibraryImport("slang")]
            public static partial ByteSize spReflectionTypeLayout_GetElementStride(
                ReflectionTypeLayout type,
                Brutal.SlangApi.ParameterCategory category
            );

            [LibraryImport("slang")]
            public static partial ReflectionTypeLayout spReflectionTypeLayout_GetElementTypeLayout(
                ReflectionTypeLayout type
            );

            [LibraryImport("slang")]
            public static partial ReflectionVariableLayout spReflectionTypeLayout_GetElementVarLayout(
                ReflectionTypeLayout type
            );

            [LibraryImport("slang")]
            public static partial ReflectionVariableLayout spReflectionTypeLayout_getContainerVarLayout(
                ReflectionTypeLayout type
            );

            [LibraryImport("slang")]
            public static partial Brutal.SlangApi.ParameterCategory spReflectionTypeLayout_GetParameterCategory(
                ReflectionTypeLayout type
            );

            [LibraryImport("slang")]
            public static partial uint spReflectionTypeLayout_GetCategoryCount(ReflectionTypeLayout type);

            [LibraryImport("slang")]
            public static partial Brutal.SlangApi.ParameterCategory spReflectionTypeLayout_GetCategoryByIndex(
                ReflectionTypeLayout type,
                uint index
            );

            [LibraryImport("slang")]
            public static partial Brutal.SlangApi.MatrixLayoutMode spReflectionTypeLayout_GetMatrixLayoutMode(
                ReflectionTypeLayout type
            );

            [LibraryImport("slang")]
            public static partial int spReflectionTypeLayout_getGenericParamIndex(ReflectionTypeLayout type);

            [LibraryImport("slang")]
            public static partial ReflectionTypeLayout spReflectionTypeLayout_getPendingDataTypeLayout(
                ReflectionTypeLayout type
            );

            [LibraryImport("slang")]
            public static partial ReflectionVariableLayout spReflectionTypeLayout_getSpecializedTypePendingDataVarLayout(
                ReflectionTypeLayout type
            );

            [LibraryImport("slang")]
            public static partial Int spReflectionType_getSpecializedTypeArgCount(ReflectionType type);

            [LibraryImport("slang")]
            public static partial ReflectionType spReflectionType_getSpecializedTypeArgType(
                ReflectionType type,
                Int index
            );

            [LibraryImport("slang")]
            public static partial Int spReflectionTypeLayout_getBindingRangeCount(ReflectionTypeLayout typeLayout);

            [LibraryImport("slang")]
            public static partial Brutal.SlangApi.BindingType spReflectionTypeLayout_getBindingRangeType(
                ReflectionTypeLayout typeLayout,
                Int index
            );

            [LibraryImport("slang")]
            public static partial Int spReflectionTypeLayout_isBindingRangeSpecializable(
                ReflectionTypeLayout typeLayout,
                Int index
            );

            [LibraryImport("slang")]
            public static partial Int spReflectionTypeLayout_getBindingRangeBindingCount(
                ReflectionTypeLayout typeLayout,
                Int index
            );

            [LibraryImport("slang")]
            public static partial ReflectionTypeLayout spReflectionTypeLayout_getBindingRangeLeafTypeLayout(
                ReflectionTypeLayout typeLayout,
                Int index
            );

            [LibraryImport("slang")]
            public static partial ReflectionVariable spReflectionTypeLayout_getBindingRangeLeafVariable(
                ReflectionTypeLayout typeLayout,
                Int index
            );

            [LibraryImport("slang")]
            public static partial Brutal.SlangApi.ImageFormat spReflectionTypeLayout_getBindingRangeImageFormat(
                ReflectionTypeLayout typeLayout,
                Int index
            );

            [LibraryImport("slang")]
            public static partial Int spReflectionTypeLayout_getFieldBindingRangeOffset(
                ReflectionTypeLayout typeLayout,
                Int fieldIndex
            );

            [LibraryImport("slang")]
            public static partial Int spReflectionTypeLayout_getExplicitCounterBindingRangeOffset(
                ReflectionTypeLayout inTypeLayout
            );

            [LibraryImport("slang")]
            public static partial Int spReflectionTypeLayout_getBindingRangeDescriptorSetIndex(
                ReflectionTypeLayout typeLayout,
                Int index
            );

            [LibraryImport("slang")]
            public static partial Int spReflectionTypeLayout_getBindingRangeFirstDescriptorRangeIndex(
                ReflectionTypeLayout typeLayout,
                Int index
            );

            [LibraryImport("slang")]
            public static partial Int spReflectionTypeLayout_getBindingRangeDescriptorRangeCount(
                ReflectionTypeLayout typeLayout,
                Int index
            );

            [LibraryImport("slang")]
            public static partial Int spReflectionTypeLayout_getDescriptorSetCount(ReflectionTypeLayout typeLayout);

            [LibraryImport("slang")]
            public static partial Int spReflectionTypeLayout_getDescriptorSetSpaceOffset(
                ReflectionTypeLayout typeLayout,
                Int setIndex
            );

            [LibraryImport("slang")]
            public static partial Int spReflectionTypeLayout_getDescriptorSetDescriptorRangeCount(
                ReflectionTypeLayout typeLayout,
                Int setIndex
            );

            [LibraryImport("slang")]
            public static partial Int spReflectionTypeLayout_getDescriptorSetDescriptorRangeIndexOffset(
                ReflectionTypeLayout typeLayout,
                Int setIndex,
                Int rangeIndex
            );

            [LibraryImport("slang")]
            public static partial Int spReflectionTypeLayout_getDescriptorSetDescriptorRangeDescriptorCount(
                ReflectionTypeLayout typeLayout,
                Int setIndex,
                Int rangeIndex
            );

            [LibraryImport("slang")]
            public static partial Brutal.SlangApi.BindingType spReflectionTypeLayout_getDescriptorSetDescriptorRangeType(
                ReflectionTypeLayout typeLayout,
                Int setIndex,
                Int rangeIndex
            );

            [LibraryImport("slang")]
            public static partial Brutal.SlangApi.ParameterCategory spReflectionTypeLayout_getDescriptorSetDescriptorRangeCategory(
                ReflectionTypeLayout typeLayout,
                Int setIndex,
                Int rangeIndex
            );

            [LibraryImport("slang")]
            public static partial Int spReflectionTypeLayout_getSubObjectRangeCount(ReflectionTypeLayout typeLayout);

            [LibraryImport("slang")]
            public static partial Int spReflectionTypeLayout_getSubObjectRangeBindingRangeIndex(
                ReflectionTypeLayout typeLayout,
                Int subObjectRangeIndex
            );

            [LibraryImport("slang")]
            public static partial Int spReflectionTypeLayout_getSubObjectRangeSpaceOffset(
                ReflectionTypeLayout typeLayout,
                Int subObjectRangeIndex
            );

            [LibraryImport("slang")]
            public static partial ReflectionVariableLayout spReflectionTypeLayout_getSubObjectRangeOffset(
                ReflectionTypeLayout typeLayout,
                Int subObjectRangeIndex
            );

            [LibraryImport(
                "slang",
                StringMarshalling = StringMarshalling.Custom,
                StringMarshallingCustomType = typeof(Brutal.Interop.InteropStringMarshaller)
            )]
            public static partial string spReflectionVariable_GetName(ReflectionVariable var);

            [LibraryImport("slang")]
            public static partial ReflectionType spReflectionVariable_GetType(ReflectionVariable var);

            [LibraryImport("slang")]
            public static partial ReflectionModifier spReflectionVariable_FindModifier(
                ReflectionVariable var,
                Brutal.SlangApi.ModifierID modifierID
            );

            [LibraryImport("slang")]
            public static partial uint spReflectionVariable_GetUserAttributeCount(ReflectionVariable var);

            [LibraryImport("slang")]
            public static partial Brutal.SlangApi.ReflectionUserAttribute spReflectionVariable_GetUserAttribute(
                ReflectionVariable var,
                uint index
            );

            [LibraryImport("slang")]
            public static partial Brutal.SlangApi.ReflectionUserAttribute spReflectionVariable_FindUserAttributeByName(
                ReflectionVariable var,
                nint globalSession,
                RefString8 name
            );

            [LibraryImport("slang")]
            public static partial Brutal.Bool8 spReflectionVariable_HasDefaultValue(ReflectionVariable inVar);

            [LibraryImport("slang")]
            public static partial Result spReflectionVariable_GetDefaultValueInt(ReflectionVariable inVar, long* rs);

            [LibraryImport("slang")]
            public static partial Result spReflectionVariable_GetDefaultValueFloat(ReflectionVariable inVar, float* rs);

            [LibraryImport("slang")]
            public static partial ReflectionGeneric spReflectionVariable_GetGenericContainer(ReflectionVariable var);

            [LibraryImport("slang")]
            public static partial ReflectionVariable spReflectionVariable_applySpecializations(
                ReflectionVariable var,
                ReflectionGeneric generic
            );

            [LibraryImport("slang")]
            public static partial ReflectionVariable spReflectionVariableLayout_GetVariable(
                ReflectionVariableLayout var
            );

            [LibraryImport("slang")]
            public static partial ReflectionTypeLayout spReflectionVariableLayout_GetTypeLayout(
                ReflectionVariableLayout var
            );

            [LibraryImport("slang")]
            public static partial ByteSize spReflectionVariableLayout_GetOffset(
                ReflectionVariableLayout var,
                Brutal.SlangApi.ParameterCategory category
            );

            [LibraryImport("slang")]
            public static partial ByteSize spReflectionVariableLayout_GetSpace(
                ReflectionVariableLayout var,
                Brutal.SlangApi.ParameterCategory category
            );

            [LibraryImport("slang")]
            public static partial Brutal.SlangApi.ImageFormat spReflectionVariableLayout_GetImageFormat(
                ReflectionVariableLayout var
            );

            [LibraryImport(
                "slang",
                StringMarshalling = StringMarshalling.Custom,
                StringMarshallingCustomType = typeof(Brutal.Interop.InteropStringMarshaller)
            )]
            public static partial string spReflectionVariableLayout_GetSemanticName(ReflectionVariableLayout var);

            [LibraryImport("slang")]
            public static partial ByteSize spReflectionVariableLayout_GetSemanticIndex(ReflectionVariableLayout var);

            [LibraryImport("slang")]
            public static partial ReflectionDecl spReflectionFunction_asDecl(ReflectionFunction func);

            [LibraryImport(
                "slang",
                StringMarshalling = StringMarshalling.Custom,
                StringMarshallingCustomType = typeof(Brutal.Interop.InteropStringMarshaller)
            )]
            public static partial string spReflectionFunction_GetName(ReflectionFunction func);

            [LibraryImport("slang")]
            public static partial ReflectionModifier spReflectionFunction_FindModifier(
                ReflectionFunction var,
                Brutal.SlangApi.ModifierID modifierID
            );

            [LibraryImport("slang")]
            public static partial uint spReflectionFunction_GetUserAttributeCount(ReflectionFunction func);

            [LibraryImport("slang")]
            public static partial Brutal.SlangApi.ReflectionUserAttribute spReflectionFunction_GetUserAttribute(
                ReflectionFunction func,
                uint index
            );

            [LibraryImport("slang")]
            public static partial Brutal.SlangApi.ReflectionUserAttribute spReflectionFunction_FindUserAttributeByName(
                ReflectionFunction func,
                nint globalSession,
                RefString8 name
            );

            [LibraryImport("slang")]
            public static partial uint spReflectionFunction_GetParameterCount(ReflectionFunction func);

            [LibraryImport("slang")]
            public static partial ReflectionVariable spReflectionFunction_GetParameter(
                ReflectionFunction func,
                uint index
            );

            [LibraryImport("slang")]
            public static partial ReflectionType spReflectionFunction_GetResultType(ReflectionFunction func);

            [LibraryImport("slang")]
            public static partial ReflectionGeneric spReflectionFunction_GetGenericContainer(ReflectionFunction func);

            [LibraryImport("slang")]
            public static partial ReflectionFunction spReflectionFunction_applySpecializations(
                ReflectionFunction func,
                ReflectionGeneric generic
            );

            [LibraryImport("slang")]
            public static partial ReflectionFunction spReflectionFunction_specializeWithArgTypes(
                ReflectionFunction func,
                Int argTypeCount,
                in ReflectionType argTypes
            );

            [LibraryImport("slang")]
            public static partial Brutal.Bool8 spReflectionFunction_isOverloaded(ReflectionFunction func);

            [LibraryImport("slang")]
            public static partial uint spReflectionFunction_getOverloadCount(ReflectionFunction func);

            [LibraryImport("slang")]
            public static partial ReflectionFunction spReflectionFunction_getOverload(
                ReflectionFunction func,
                uint index
            );

            [LibraryImport("slang")]
            public static partial uint spReflectionDecl_getChildrenCount(ReflectionDecl parentDecl);

            [LibraryImport("slang")]
            public static partial ReflectionDecl spReflectionDecl_getChild(ReflectionDecl parentDecl, uint index);

            [LibraryImport(
                "slang",
                StringMarshalling = StringMarshalling.Custom,
                StringMarshallingCustomType = typeof(Brutal.Interop.InteropStringMarshaller)
            )]
            public static partial string spReflectionDecl_getName(ReflectionDecl decl);

            [LibraryImport("slang")]
            public static partial Brutal.SlangApi.DeclKind spReflectionDecl_getKind(ReflectionDecl decl);

            [LibraryImport("slang")]
            public static partial ReflectionFunction spReflectionDecl_castToFunction(ReflectionDecl decl);

            [LibraryImport("slang")]
            public static partial ReflectionVariable spReflectionDecl_castToVariable(ReflectionDecl decl);

            [LibraryImport("slang")]
            public static partial ReflectionGeneric spReflectionDecl_castToGeneric(ReflectionDecl decl);

            [LibraryImport("slang")]
            public static partial ReflectionType spReflection_getTypeFromDecl(ReflectionDecl decl);

            [LibraryImport("slang")]
            public static partial ReflectionDecl spReflectionDecl_getParent(ReflectionDecl decl);

            [LibraryImport("slang")]
            public static partial ReflectionModifier spReflectionDecl_findModifier(
                ReflectionDecl decl,
                Brutal.SlangApi.ModifierID modifierID
            );

            [LibraryImport("slang")]
            public static partial ReflectionDecl spReflectionGeneric_asDecl(ReflectionGeneric generic);

            [LibraryImport(
                "slang",
                StringMarshalling = StringMarshalling.Custom,
                StringMarshallingCustomType = typeof(Brutal.Interop.InteropStringMarshaller)
            )]
            public static partial string spReflectionGeneric_GetName(ReflectionGeneric generic);

            [LibraryImport("slang")]
            public static partial uint spReflectionGeneric_GetTypeParameterCount(ReflectionGeneric generic);

            [LibraryImport("slang")]
            public static partial ReflectionVariable spReflectionGeneric_GetTypeParameter(
                ReflectionGeneric generic,
                uint index
            );

            [LibraryImport("slang")]
            public static partial uint spReflectionGeneric_GetValueParameterCount(ReflectionGeneric generic);

            [LibraryImport("slang")]
            public static partial ReflectionVariable spReflectionGeneric_GetValueParameter(
                ReflectionGeneric generic,
                uint index
            );

            [LibraryImport("slang")]
            public static partial uint spReflectionGeneric_GetTypeParameterConstraintCount(
                ReflectionGeneric generic,
                ReflectionVariable typeParam
            );

            [LibraryImport("slang")]
            public static partial ReflectionType spReflectionGeneric_GetTypeParameterConstraintType(
                ReflectionGeneric generic,
                ReflectionVariable typeParam,
                uint index
            );

            [LibraryImport("slang")]
            public static partial Brutal.SlangApi.DeclKind spReflectionGeneric_GetInnerKind(ReflectionGeneric generic);

            [LibraryImport("slang")]
            public static partial ReflectionDecl spReflectionGeneric_GetInnerDecl(ReflectionGeneric generic);

            [LibraryImport("slang")]
            public static partial ReflectionGeneric spReflectionGeneric_GetOuterGenericContainer(
                ReflectionGeneric generic
            );

            [LibraryImport("slang")]
            public static partial ReflectionType spReflectionGeneric_GetConcreteType(
                ReflectionGeneric generic,
                ReflectionVariable typeParam
            );

            [LibraryImport("slang")]
            public static partial long spReflectionGeneric_GetConcreteIntVal(
                ReflectionGeneric generic,
                ReflectionVariable valueParam
            );

            [LibraryImport("slang")]
            public static partial ReflectionGeneric spReflectionGeneric_applySpecializations(
                ReflectionGeneric currGeneric,
                ReflectionGeneric generic
            );

            [LibraryImport("slang")]
            public static partial Brutal.SlangApi.Stage spReflectionVariableLayout_getStage(
                ReflectionVariableLayout var
            );

            [LibraryImport("slang")]
            public static partial ReflectionVariableLayout spReflectionVariableLayout_getPendingDataLayout(
                ReflectionVariableLayout var
            );

            [LibraryImport("slang")]
            public static partial uint spReflectionParameter_GetBindingIndex(ReflectionParameter parameter);

            [LibraryImport("slang")]
            public static partial uint spReflectionParameter_GetBindingSpace(ReflectionParameter parameter);

            [LibraryImport("slang")]
            public static partial Result spIsParameterLocationUsed(
                CompileRequest request,
                Int entryPointIndex,
                Int targetIndex,
                Brutal.SlangApi.ParameterCategory category,
                UInt spaceIndex,
                UInt registerIndex,
                ref Brutal.Bool8 outUsed
            );

            [LibraryImport(
                "slang",
                StringMarshalling = StringMarshalling.Custom,
                StringMarshallingCustomType = typeof(Brutal.Interop.InteropStringMarshaller)
            )]
            public static partial string spReflectionEntryPoint_getName(ReflectionEntryPoint entryPoint);

            [LibraryImport(
                "slang",
                StringMarshalling = StringMarshalling.Custom,
                StringMarshallingCustomType = typeof(Brutal.Interop.InteropStringMarshaller)
            )]
            public static partial string spReflectionEntryPoint_getNameOverride(ReflectionEntryPoint entryPoint);

            [LibraryImport("slang")]
            public static partial ReflectionFunction spReflectionEntryPoint_getFunction(
                ReflectionEntryPoint entryPoint
            );

            [LibraryImport("slang")]
            public static partial uint spReflectionEntryPoint_getParameterCount(ReflectionEntryPoint entryPoint);

            [LibraryImport("slang")]
            public static partial ReflectionVariableLayout spReflectionEntryPoint_getParameterByIndex(
                ReflectionEntryPoint entryPoint,
                uint index
            );

            [LibraryImport("slang")]
            public static partial Brutal.SlangApi.Stage spReflectionEntryPoint_getStage(
                ReflectionEntryPoint entryPoint
            );

            [LibraryImport("slang")]
            public static partial void spReflectionEntryPoint_getComputeThreadGroupSize(
                ReflectionEntryPoint entryPoint,
                UInt axisCount,
                UInt* outSizeAlongAxis
            );

            [LibraryImport("slang")]
            public static partial void spReflectionEntryPoint_getComputeWaveSize(
                ReflectionEntryPoint entryPoint,
                out UInt outWaveSize
            );

            [LibraryImport("slang")]
            public static partial int spReflectionEntryPoint_usesAnySampleRateInput(ReflectionEntryPoint entryPoint);

            [LibraryImport("slang")]
            public static partial ReflectionVariableLayout spReflectionEntryPoint_getVarLayout(
                ReflectionEntryPoint entryPoint
            );

            [LibraryImport("slang")]
            public static partial ReflectionVariableLayout spReflectionEntryPoint_getResultVarLayout(
                ReflectionEntryPoint entryPoint
            );

            [LibraryImport("slang")]
            public static partial int spReflectionEntryPoint_hasDefaultConstantBuffer(ReflectionEntryPoint entryPoint);

            [LibraryImport(
                "slang",
                StringMarshalling = StringMarshalling.Custom,
                StringMarshallingCustomType = typeof(Brutal.Interop.InteropStringMarshaller)
            )]
            public static partial string spReflectionTypeParameter_GetName(ReflectionTypeParameter typeParam);

            [LibraryImport("slang")]
            public static partial uint spReflectionTypeParameter_GetIndex(ReflectionTypeParameter typeParam);

            [LibraryImport("slang")]
            public static partial uint spReflectionTypeParameter_GetConstraintCount(ReflectionTypeParameter typeParam);

            [LibraryImport("slang")]
            public static partial ReflectionType spReflectionTypeParameter_GetConstraintByIndex(
                ReflectionTypeParameter typeParam,
                uint index
            );

            [LibraryImport("slang")]
            public static partial Result spReflection_ToJson(
                Reflection reflection,
                CompileRequest request,
                out nint outBlob
            );

            [LibraryImport("slang")]
            public static partial uint spReflection_GetParameterCount(Reflection reflection);

            [LibraryImport("slang")]
            public static partial ReflectionParameter spReflection_GetParameterByIndex(
                Reflection reflection,
                uint index
            );

            [LibraryImport("slang")]
            public static partial uint spReflection_GetTypeParameterCount(Reflection reflection);

            [LibraryImport("slang")]
            public static partial ReflectionTypeParameter spReflection_GetTypeParameterByIndex(
                Reflection reflection,
                uint index
            );

            [LibraryImport("slang")]
            public static partial ReflectionTypeParameter spReflection_FindTypeParameter(
                Reflection reflection,
                RefString8 name
            );

            [LibraryImport("slang")]
            public static partial ReflectionType spReflection_FindTypeByName(Reflection reflection, RefString8 name);

            [LibraryImport("slang")]
            public static partial ReflectionTypeLayout spReflection_GetTypeLayout(
                Reflection reflection,
                ReflectionType reflectionType,
                Brutal.SlangApi.LayoutRules rules
            );

            [LibraryImport("slang")]
            public static partial ReflectionFunction spReflection_FindFunctionByName(
                Reflection reflection,
                RefString8 name
            );

            [LibraryImport("slang")]
            public static partial ReflectionFunction spReflection_FindFunctionByNameInType(
                Reflection reflection,
                ReflectionType reflType,
                RefString8 name
            );

            [LibraryImport("slang")]
            public static partial ReflectionVariable spReflection_FindVarByNameInType(
                Reflection reflection,
                ReflectionType reflType,
                RefString8 name
            );

            [LibraryImport("slang")]
            public static partial ReflectionFunction spReflection_TryResolveOverloadedFunction(
                Reflection reflection,
                uint candidateCount,
                in ReflectionFunction candidates
            );

            [LibraryImport("slang")]
            public static partial UInt spReflection_getEntryPointCount(Reflection reflection);

            [LibraryImport("slang")]
            public static partial ReflectionEntryPoint spReflection_getEntryPointByIndex(
                Reflection reflection,
                UInt index
            );

            [LibraryImport("slang")]
            public static partial ReflectionEntryPoint spReflection_findEntryPointByName(
                Reflection reflection,
                RefString8 name
            );

            [LibraryImport("slang")]
            public static partial UInt spReflection_getGlobalConstantBufferBinding(Reflection reflection);

            [LibraryImport("slang")]
            public static partial ByteSize spReflection_getGlobalConstantBufferSize(Reflection reflection);

            [LibraryImport("slang")]
            public static partial ReflectionType spReflection_specializeType(
                Reflection reflection,
                ReflectionType type,
                Int specializationArgCount,
                in ReflectionType specializationArgs,
                out nint outDiagnostics
            );

            [LibraryImport("slang")]
            public static partial ReflectionGeneric spReflection_specializeGeneric(
                Reflection inProgramLayout,
                ReflectionGeneric generic,
                Int argCount,
                in Brutal.SlangApi.ReflectionGenericArgType argTypes,
                in Brutal.SlangApi.ReflectionGenericArg args,
                out nint outDiagnostics
            );

            [LibraryImport("slang")]
            public static partial Brutal.Bool8 spReflection_isSubType(
                Reflection reflection,
                ReflectionType subType,
                ReflectionType superType
            );

            [LibraryImport("slang")]
            public static partial UInt spReflection_getHashedStringCount(Reflection reflection);

            [LibraryImport(
                "slang",
                StringMarshalling = StringMarshalling.Custom,
                StringMarshallingCustomType = typeof(Brutal.Interop.InteropStringMarshaller)
            )]
            public static partial string spReflection_getHashedString(
                Reflection reflection,
                UInt index,
                out ByteSize outCount
            );

            [LibraryImport("slang")]
            public static partial UInt32 spComputeStringHash(RefString8 chars, ByteSize count);

            [LibraryImport("slang")]
            public static partial ReflectionTypeLayout spReflection_getGlobalParamsTypeLayout(Reflection reflection);

            [LibraryImport("slang")]
            public static partial ReflectionVariableLayout spReflection_getGlobalParamsVarLayout(Reflection reflection);

            [LibraryImport(
                "slang",
                StringMarshalling = StringMarshalling.Custom,
                StringMarshallingCustomType = typeof(Brutal.Interop.InteropStringMarshaller)
            )]
            public static partial string spGetTranslationUnitSource(CompileRequest request, int translationUnitIndex);

            [LibraryImport("slang")]
            public static partial Int spReflection_getBindlessSpaceIndex(Reflection reflection);

            [LibraryImport("slang")]
            public static partial Result spCompileRequest_getProgram(CompileRequest request, out nint outProgram);

            [LibraryImport("slang")]
            public static partial Result spCompileRequest_getProgramWithEntryPoints(
                CompileRequest request,
                out nint outProgram
            );

            [LibraryImport("slang")]
            public static partial Result spCompileRequest_getEntryPoint(
                CompileRequest request,
                Int entryPointIndex,
                out nint outEntryPoint
            );

            [LibraryImport("slang")]
            public static partial Result spCompileRequest_getModule(
                CompileRequest request,
                Int translationUnitIndex,
                out nint outModule
            );

            [LibraryImport("slang")]
            public static partial Result spCompileRequest_getSession(CompileRequest request, out nint outSession);

            [LibraryImport("slang")]
            public static partial nint slang_createBlob(void* data, ByteSize size);

            [LibraryImport("slang")]
            public static partial nint slang_loadModuleFromSource(
                nint session,
                RefString8 moduleName,
                RefString8 path,
                RefString8 source,
                ByteSize sourceSize,
                out nint outDiagnostics
            );

            [LibraryImport("slang")]
            public static partial nint slang_loadModuleFromIRBlob(
                nint session,
                RefString8 moduleName,
                RefString8 path,
                void* source,
                ByteSize sourceSize,
                out nint outDiagnostics
            );

            [LibraryImport("slang")]
            public static partial Result slang_loadModuleInfoFromIRBlob(
                nint session,
                void* source,
                ByteSize sourceSize,
                ref Int outModuleVersion,
                ref byte* outModuleCompilerVersion,
                ref byte* outModuleName
            );

            [LibraryImport("slang")]
            public static partial Result slang_createGlobalSession(Int apiVersion, out nint outGlobalSession);

            [LibraryImport("slang")]
            public static partial Result slang_createGlobalSession2(
                in Brutal.SlangApi.GlobalSessionDesc desc,
                out nint outGlobalSession
            );

            [LibraryImport("slang")]
            public static partial Result slang_createGlobalSessionWithoutCoreModule(
                Int apiVersion,
                out nint outGlobalSession
            );

            [LibraryImport("slang")]
            public static partial void slang_shutdown();

            [LibraryImport("slang")]
            public static partial void slang_enableRecordLayer(Brutal.Bool8 enable);

            [LibraryImport("slang")]
            public static partial Brutal.Bool8 slang_isRecordLayerEnabled();

            [LibraryImport("slang")]
            public static partial void slang_setReplayDirectory(RefString8 path);

            [LibraryImport(
                "slang",
                StringMarshalling = StringMarshalling.Custom,
                StringMarshallingCustomType = typeof(Brutal.Interop.InteropStringMarshaller)
            )]
            public static partial string slang_getReplayDirectory();

            [LibraryImport(
                "slang",
                StringMarshalling = StringMarshalling.Custom,
                StringMarshallingCustomType = typeof(Brutal.Interop.InteropStringMarshaller)
            )]
            public static partial string slang_getCurrentReplayPath();

            [LibraryImport("slang")]
            public static partial Result slang_loadReplay(RefString8 folderPath);

            [LibraryImport("slang")]
            public static partial Result slang_loadLatestReplay();

            [LibraryImport("slang")]
            public static partial void slang_replayMarker(RefString8 label);

            [LibraryImport(
                "slang",
                StringMarshalling = StringMarshalling.Custom,
                StringMarshallingCustomType = typeof(Brutal.Interop.InteropStringMarshaller)
            )]
            public static partial string slang_getLastInternalErrorMessage();

            [LibraryImport("slang")]
            public static partial Result slang_createByteCodeRunner(
                in Brutal.SlangApi.ByteCodeRunnerDesc desc,
                out nint outByteCodeRunner
            );

            [LibraryImport("slang")]
            public static partial Result slang_disassembleByteCode(nint moduleBlob, out nint outDisassemblyBlob);
        };

        public static string GetBuildTagString()
        {
            return PInvoke.spGetBuildTagString();
        }

        internal static nint CreateSession(RefString8 deprecated)
        {
            return PInvoke.spCreateSession(deprecated);
        }

        internal static void DestroySession(nint session)
        {
            PInvoke.spDestroySession(session);
        }

        internal static void SessionSetSharedLibraryLoader(nint session, nint loader)
        {
            PInvoke.spSessionSetSharedLibraryLoader(session, loader);
        }

        internal static nint SessionGetSharedLibraryLoader(nint session)
        {
            return PInvoke.spSessionGetSharedLibraryLoader(session);
        }

        internal static Result SessionCheckCompileTargetSupport(nint session, Brutal.SlangApi.CompileTarget target)
        {
            return PInvoke.spSessionCheckCompileTargetSupport(session, target);
        }

        internal static Result SessionCheckPassThroughSupport(nint session, Brutal.SlangApi.PassThrough passThrough)
        {
            return PInvoke.spSessionCheckPassThroughSupport(session, passThrough);
        }

        internal static void AddBuiltins(nint session, RefString8 sourcePath, RefString8 sourceString)
        {
            PInvoke.spAddBuiltins(session, sourcePath, sourceString);
        }

        internal static CompileRequest CreateCompileRequest(nint session)
        {
            return PInvoke.spCreateCompileRequest(session);
        }

        internal static Brutal.SlangApi.ProfileID FindProfile(nint session, RefString8 name)
        {
            return PInvoke.spFindProfile(session, name);
        }

        internal static Brutal.SlangApi.CapabilityID FindCapability(nint session, RefString8 name)
        {
            return PInvoke.spFindCapability(session, name);
        }

        internal static unsafe Result ExtractRepro(
            nint session,
            void* reproData,
            ByteSize reproDataSize,
            nint fileSystem
        )
        {
            return PInvoke.spExtractRepro(session, reproData, reproDataSize, fileSystem);
        }

        internal static unsafe Result LoadReproAsFileSystem(
            nint session,
            void* reproData,
            ByteSize reproDataSize,
            nint replaceFileSystem,
            out nint outFileSystem
        )
        {
            return PInvoke.spLoadReproAsFileSystem(
                session,
                reproData,
                reproDataSize,
                replaceFileSystem,
                out outFileSystem
            );
        }

        public static Brutal.SlangApi.TypeKind ReflectionTypeGetKind(ReflectionType type)
        {
            return PInvoke.spReflectionType_GetKind(type);
        }

        public static uint ReflectionTypeGetUserAttributeCount(ReflectionType type)
        {
            return PInvoke.spReflectionType_GetUserAttributeCount(type);
        }

        public static Brutal.SlangApi.ReflectionUserAttribute ReflectionTypeGetUserAttribute(
            ReflectionType type,
            uint index
        )
        {
            return PInvoke.spReflectionType_GetUserAttribute(type, index);
        }

        public static Brutal.SlangApi.ReflectionUserAttribute ReflectionTypeFindUserAttributeByName(
            ReflectionType type,
            RefString8 name
        )
        {
            return PInvoke.spReflectionType_FindUserAttributeByName(type, name);
        }

        public static ReflectionType ReflectionTypeApplySpecializations(ReflectionType type, ReflectionGeneric generic)
        {
            return PInvoke.spReflectionType_applySpecializations(type, generic);
        }

        public static uint ReflectionTypeGetFieldCount(ReflectionType type)
        {
            return PInvoke.spReflectionType_GetFieldCount(type);
        }

        public static ReflectionVariable ReflectionTypeGetFieldByIndex(ReflectionType type, uint index)
        {
            return PInvoke.spReflectionType_GetFieldByIndex(type, index);
        }

        public static ByteSize ReflectionTypeGetElementCount(ReflectionType type)
        {
            return PInvoke.spReflectionType_GetElementCount(type);
        }

        public static ByteSize ReflectionTypeGetSpecializedElementCount(ReflectionType type, Reflection reflection)
        {
            return PInvoke.spReflectionType_GetSpecializedElementCount(type, reflection);
        }

        public static ReflectionType ReflectionTypeGetElementType(ReflectionType type)
        {
            return PInvoke.spReflectionType_GetElementType(type);
        }

        public static uint ReflectionTypeGetRowCount(ReflectionType type)
        {
            return PInvoke.spReflectionType_GetRowCount(type);
        }

        public static uint ReflectionTypeGetColumnCount(ReflectionType type)
        {
            return PInvoke.spReflectionType_GetColumnCount(type);
        }

        public static Brutal.SlangApi.ScalarType ReflectionTypeGetScalarType(ReflectionType type)
        {
            return PInvoke.spReflectionType_GetScalarType(type);
        }

        public static Brutal.SlangApi.ResourceShape ReflectionTypeGetResourceShape(ReflectionType type)
        {
            return PInvoke.spReflectionType_GetResourceShape(type);
        }

        public static Brutal.SlangApi.ResourceAccess ReflectionTypeGetResourceAccess(ReflectionType type)
        {
            return PInvoke.spReflectionType_GetResourceAccess(type);
        }

        public static ReflectionType ReflectionTypeGetResourceResultType(ReflectionType type)
        {
            return PInvoke.spReflectionType_GetResourceResultType(type);
        }

        public static string ReflectionTypeGetName(ReflectionType type)
        {
            return PInvoke.spReflectionType_GetName(type);
        }

        internal static Result ReflectionTypeGetFullName(ReflectionType type, out nint outNameBlob)
        {
            return PInvoke.spReflectionType_GetFullName(type, out outNameBlob);
        }

        public static ReflectionGeneric ReflectionTypeGetGenericContainer(ReflectionType type)
        {
            return PInvoke.spReflectionType_GetGenericContainer(type);
        }

        public static ReflectionType ReflectionTypeLayoutGetType(ReflectionTypeLayout type)
        {
            return PInvoke.spReflectionTypeLayout_GetType(type);
        }

        public static Brutal.SlangApi.TypeKind ReflectionTypeLayoutGetKind(ReflectionTypeLayout type)
        {
            return PInvoke.spReflectionTypeLayout_getKind(type);
        }

        public static ByteSize ReflectionTypeLayoutGetSize(
            ReflectionTypeLayout type,
            Brutal.SlangApi.ParameterCategory category
        )
        {
            return PInvoke.spReflectionTypeLayout_GetSize(type, category);
        }

        public static ByteSize ReflectionTypeLayoutGetStride(
            ReflectionTypeLayout type,
            Brutal.SlangApi.ParameterCategory category
        )
        {
            return PInvoke.spReflectionTypeLayout_GetStride(type, category);
        }

        public static int ReflectionTypeLayoutGetAlignment(
            ReflectionTypeLayout type,
            Brutal.SlangApi.ParameterCategory category
        )
        {
            return PInvoke.spReflectionTypeLayout_getAlignment(type, category);
        }

        public static uint ReflectionTypeLayoutGetFieldCount(ReflectionTypeLayout type)
        {
            return PInvoke.spReflectionTypeLayout_GetFieldCount(type);
        }

        public static ReflectionVariableLayout ReflectionTypeLayoutGetFieldByIndex(
            ReflectionTypeLayout type,
            uint index
        )
        {
            return PInvoke.spReflectionTypeLayout_GetFieldByIndex(type, index);
        }

        public static Int ReflectionTypeLayoutFindFieldIndexByName(
            ReflectionTypeLayout typeLayout,
            RefString8 nameBegin,
            RefString8 nameEnd
        )
        {
            return PInvoke.spReflectionTypeLayout_findFieldIndexByName(typeLayout, nameBegin, nameEnd);
        }

        public static ReflectionVariableLayout ReflectionTypeLayoutGetExplicitCounter(ReflectionTypeLayout typeLayout)
        {
            return PInvoke.spReflectionTypeLayout_GetExplicitCounter(typeLayout);
        }

        public static ByteSize ReflectionTypeLayoutGetElementStride(
            ReflectionTypeLayout type,
            Brutal.SlangApi.ParameterCategory category
        )
        {
            return PInvoke.spReflectionTypeLayout_GetElementStride(type, category);
        }

        public static ReflectionTypeLayout ReflectionTypeLayoutGetElementTypeLayout(ReflectionTypeLayout type)
        {
            return PInvoke.spReflectionTypeLayout_GetElementTypeLayout(type);
        }

        public static ReflectionVariableLayout ReflectionTypeLayoutGetElementVarLayout(ReflectionTypeLayout type)
        {
            return PInvoke.spReflectionTypeLayout_GetElementVarLayout(type);
        }

        public static ReflectionVariableLayout ReflectionTypeLayoutGetContainerVarLayout(ReflectionTypeLayout type)
        {
            return PInvoke.spReflectionTypeLayout_getContainerVarLayout(type);
        }

        public static Brutal.SlangApi.ParameterCategory ReflectionTypeLayoutGetParameterCategory(
            ReflectionTypeLayout type
        )
        {
            return PInvoke.spReflectionTypeLayout_GetParameterCategory(type);
        }

        public static uint ReflectionTypeLayoutGetCategoryCount(ReflectionTypeLayout type)
        {
            return PInvoke.spReflectionTypeLayout_GetCategoryCount(type);
        }

        public static Brutal.SlangApi.ParameterCategory ReflectionTypeLayoutGetCategoryByIndex(
            ReflectionTypeLayout type,
            uint index
        )
        {
            return PInvoke.spReflectionTypeLayout_GetCategoryByIndex(type, index);
        }

        public static Brutal.SlangApi.MatrixLayoutMode ReflectionTypeLayoutGetMatrixLayoutMode(
            ReflectionTypeLayout type
        )
        {
            return PInvoke.spReflectionTypeLayout_GetMatrixLayoutMode(type);
        }

        public static int ReflectionTypeLayoutGetGenericParamIndex(ReflectionTypeLayout type)
        {
            return PInvoke.spReflectionTypeLayout_getGenericParamIndex(type);
        }

        public static ReflectionTypeLayout ReflectionTypeLayoutGetPendingDataTypeLayout(ReflectionTypeLayout type)
        {
            return PInvoke.spReflectionTypeLayout_getPendingDataTypeLayout(type);
        }

        public static ReflectionVariableLayout ReflectionTypeLayoutGetSpecializedTypePendingDataVarLayout(
            ReflectionTypeLayout type
        )
        {
            return PInvoke.spReflectionTypeLayout_getSpecializedTypePendingDataVarLayout(type);
        }

        public static Int ReflectionTypeGetSpecializedTypeArgCount(ReflectionType type)
        {
            return PInvoke.spReflectionType_getSpecializedTypeArgCount(type);
        }

        public static ReflectionType ReflectionTypeGetSpecializedTypeArgType(ReflectionType type, Int index)
        {
            return PInvoke.spReflectionType_getSpecializedTypeArgType(type, index);
        }

        public static Int ReflectionTypeLayoutGetBindingRangeCount(ReflectionTypeLayout typeLayout)
        {
            return PInvoke.spReflectionTypeLayout_getBindingRangeCount(typeLayout);
        }

        public static Brutal.SlangApi.BindingType ReflectionTypeLayoutGetBindingRangeType(
            ReflectionTypeLayout typeLayout,
            Int index
        )
        {
            return PInvoke.spReflectionTypeLayout_getBindingRangeType(typeLayout, index);
        }

        public static Int ReflectionTypeLayoutIsBindingRangeSpecializable(ReflectionTypeLayout typeLayout, Int index)
        {
            return PInvoke.spReflectionTypeLayout_isBindingRangeSpecializable(typeLayout, index);
        }

        public static Int ReflectionTypeLayoutGetBindingRangeBindingCount(ReflectionTypeLayout typeLayout, Int index)
        {
            return PInvoke.spReflectionTypeLayout_getBindingRangeBindingCount(typeLayout, index);
        }

        public static ReflectionTypeLayout ReflectionTypeLayoutGetBindingRangeLeafTypeLayout(
            ReflectionTypeLayout typeLayout,
            Int index
        )
        {
            return PInvoke.spReflectionTypeLayout_getBindingRangeLeafTypeLayout(typeLayout, index);
        }

        public static ReflectionVariable ReflectionTypeLayoutGetBindingRangeLeafVariable(
            ReflectionTypeLayout typeLayout,
            Int index
        )
        {
            return PInvoke.spReflectionTypeLayout_getBindingRangeLeafVariable(typeLayout, index);
        }

        public static Brutal.SlangApi.ImageFormat ReflectionTypeLayoutGetBindingRangeImageFormat(
            ReflectionTypeLayout typeLayout,
            Int index
        )
        {
            return PInvoke.spReflectionTypeLayout_getBindingRangeImageFormat(typeLayout, index);
        }

        public static Int ReflectionTypeLayoutGetFieldBindingRangeOffset(
            ReflectionTypeLayout typeLayout,
            Int fieldIndex
        )
        {
            return PInvoke.spReflectionTypeLayout_getFieldBindingRangeOffset(typeLayout, fieldIndex);
        }

        public static Int ReflectionTypeLayoutGetExplicitCounterBindingRangeOffset(ReflectionTypeLayout inTypeLayout)
        {
            return PInvoke.spReflectionTypeLayout_getExplicitCounterBindingRangeOffset(inTypeLayout);
        }

        public static Int ReflectionTypeLayoutGetBindingRangeDescriptorSetIndex(
            ReflectionTypeLayout typeLayout,
            Int index
        )
        {
            return PInvoke.spReflectionTypeLayout_getBindingRangeDescriptorSetIndex(typeLayout, index);
        }

        public static Int ReflectionTypeLayoutGetBindingRangeFirstDescriptorRangeIndex(
            ReflectionTypeLayout typeLayout,
            Int index
        )
        {
            return PInvoke.spReflectionTypeLayout_getBindingRangeFirstDescriptorRangeIndex(typeLayout, index);
        }

        public static Int ReflectionTypeLayoutGetBindingRangeDescriptorRangeCount(
            ReflectionTypeLayout typeLayout,
            Int index
        )
        {
            return PInvoke.spReflectionTypeLayout_getBindingRangeDescriptorRangeCount(typeLayout, index);
        }

        public static Int ReflectionTypeLayoutGetDescriptorSetCount(ReflectionTypeLayout typeLayout)
        {
            return PInvoke.spReflectionTypeLayout_getDescriptorSetCount(typeLayout);
        }

        public static Int ReflectionTypeLayoutGetDescriptorSetSpaceOffset(ReflectionTypeLayout typeLayout, Int setIndex)
        {
            return PInvoke.spReflectionTypeLayout_getDescriptorSetSpaceOffset(typeLayout, setIndex);
        }

        public static Int ReflectionTypeLayoutGetDescriptorSetDescriptorRangeCount(
            ReflectionTypeLayout typeLayout,
            Int setIndex
        )
        {
            return PInvoke.spReflectionTypeLayout_getDescriptorSetDescriptorRangeCount(typeLayout, setIndex);
        }

        public static Int ReflectionTypeLayoutGetDescriptorSetDescriptorRangeIndexOffset(
            ReflectionTypeLayout typeLayout,
            Int setIndex,
            Int rangeIndex
        )
        {
            return PInvoke.spReflectionTypeLayout_getDescriptorSetDescriptorRangeIndexOffset(
                typeLayout,
                setIndex,
                rangeIndex
            );
        }

        public static Int ReflectionTypeLayoutGetDescriptorSetDescriptorRangeDescriptorCount(
            ReflectionTypeLayout typeLayout,
            Int setIndex,
            Int rangeIndex
        )
        {
            return PInvoke.spReflectionTypeLayout_getDescriptorSetDescriptorRangeDescriptorCount(
                typeLayout,
                setIndex,
                rangeIndex
            );
        }

        public static Brutal.SlangApi.BindingType ReflectionTypeLayoutGetDescriptorSetDescriptorRangeType(
            ReflectionTypeLayout typeLayout,
            Int setIndex,
            Int rangeIndex
        )
        {
            return PInvoke.spReflectionTypeLayout_getDescriptorSetDescriptorRangeType(typeLayout, setIndex, rangeIndex);
        }

        public static Brutal.SlangApi.ParameterCategory ReflectionTypeLayoutGetDescriptorSetDescriptorRangeCategory(
            ReflectionTypeLayout typeLayout,
            Int setIndex,
            Int rangeIndex
        )
        {
            return PInvoke.spReflectionTypeLayout_getDescriptorSetDescriptorRangeCategory(
                typeLayout,
                setIndex,
                rangeIndex
            );
        }

        public static Int ReflectionTypeLayoutGetSubObjectRangeCount(ReflectionTypeLayout typeLayout)
        {
            return PInvoke.spReflectionTypeLayout_getSubObjectRangeCount(typeLayout);
        }

        public static Int ReflectionTypeLayoutGetSubObjectRangeBindingRangeIndex(
            ReflectionTypeLayout typeLayout,
            Int subObjectRangeIndex
        )
        {
            return PInvoke.spReflectionTypeLayout_getSubObjectRangeBindingRangeIndex(typeLayout, subObjectRangeIndex);
        }

        public static Int ReflectionTypeLayoutGetSubObjectRangeSpaceOffset(
            ReflectionTypeLayout typeLayout,
            Int subObjectRangeIndex
        )
        {
            return PInvoke.spReflectionTypeLayout_getSubObjectRangeSpaceOffset(typeLayout, subObjectRangeIndex);
        }

        public static ReflectionVariableLayout ReflectionTypeLayoutGetSubObjectRangeOffset(
            ReflectionTypeLayout typeLayout,
            Int subObjectRangeIndex
        )
        {
            return PInvoke.spReflectionTypeLayout_getSubObjectRangeOffset(typeLayout, subObjectRangeIndex);
        }

        public static string ReflectionVariableGetName(ReflectionVariable var)
        {
            return PInvoke.spReflectionVariable_GetName(var);
        }

        public static ReflectionType ReflectionVariableGetType(ReflectionVariable var)
        {
            return PInvoke.spReflectionVariable_GetType(var);
        }

        public static ReflectionModifier ReflectionVariableFindModifier(
            ReflectionVariable var,
            Brutal.SlangApi.ModifierID modifierID
        )
        {
            return PInvoke.spReflectionVariable_FindModifier(var, modifierID);
        }

        public static uint ReflectionVariableGetUserAttributeCount(ReflectionVariable var)
        {
            return PInvoke.spReflectionVariable_GetUserAttributeCount(var);
        }

        public static Brutal.SlangApi.ReflectionUserAttribute ReflectionVariableGetUserAttribute(
            ReflectionVariable var,
            uint index
        )
        {
            return PInvoke.spReflectionVariable_GetUserAttribute(var, index);
        }

        internal static Brutal.SlangApi.ReflectionUserAttribute ReflectionVariableFindUserAttributeByName(
            ReflectionVariable var,
            nint globalSession,
            RefString8 name
        )
        {
            return PInvoke.spReflectionVariable_FindUserAttributeByName(var, globalSession, name);
        }

        public static Brutal.Bool8 ReflectionVariableHasDefaultValue(ReflectionVariable inVar)
        {
            return PInvoke.spReflectionVariable_HasDefaultValue(inVar);
        }

        public static unsafe Result ReflectionVariableGetDefaultValueInt(ReflectionVariable inVar, long* rs)
        {
            return PInvoke.spReflectionVariable_GetDefaultValueInt(inVar, rs);
        }

        public static unsafe Result ReflectionVariableGetDefaultValueFloat(ReflectionVariable inVar, float* rs)
        {
            return PInvoke.spReflectionVariable_GetDefaultValueFloat(inVar, rs);
        }

        public static ReflectionGeneric ReflectionVariableGetGenericContainer(ReflectionVariable var)
        {
            return PInvoke.spReflectionVariable_GetGenericContainer(var);
        }

        public static ReflectionVariable ReflectionVariableApplySpecializations(
            ReflectionVariable var,
            ReflectionGeneric generic
        )
        {
            return PInvoke.spReflectionVariable_applySpecializations(var, generic);
        }

        public static ReflectionDecl ReflectionFunctionAsDecl(ReflectionFunction func)
        {
            return PInvoke.spReflectionFunction_asDecl(func);
        }

        public static string ReflectionFunctionGetName(ReflectionFunction func)
        {
            return PInvoke.spReflectionFunction_GetName(func);
        }

        public static ReflectionModifier ReflectionFunctionFindModifier(
            ReflectionFunction var,
            Brutal.SlangApi.ModifierID modifierID
        )
        {
            return PInvoke.spReflectionFunction_FindModifier(var, modifierID);
        }

        public static uint ReflectionFunctionGetUserAttributeCount(ReflectionFunction func)
        {
            return PInvoke.spReflectionFunction_GetUserAttributeCount(func);
        }

        public static Brutal.SlangApi.ReflectionUserAttribute ReflectionFunctionGetUserAttribute(
            ReflectionFunction func,
            uint index
        )
        {
            return PInvoke.spReflectionFunction_GetUserAttribute(func, index);
        }

        internal static Brutal.SlangApi.ReflectionUserAttribute ReflectionFunctionFindUserAttributeByName(
            ReflectionFunction func,
            nint globalSession,
            RefString8 name
        )
        {
            return PInvoke.spReflectionFunction_FindUserAttributeByName(func, globalSession, name);
        }

        public static uint ReflectionFunctionGetParameterCount(ReflectionFunction func)
        {
            return PInvoke.spReflectionFunction_GetParameterCount(func);
        }

        public static ReflectionVariable ReflectionFunctionGetParameter(ReflectionFunction func, uint index)
        {
            return PInvoke.spReflectionFunction_GetParameter(func, index);
        }

        public static ReflectionType ReflectionFunctionGetResultType(ReflectionFunction func)
        {
            return PInvoke.spReflectionFunction_GetResultType(func);
        }

        public static ReflectionGeneric ReflectionFunctionGetGenericContainer(ReflectionFunction func)
        {
            return PInvoke.spReflectionFunction_GetGenericContainer(func);
        }

        public static ReflectionFunction ReflectionFunctionApplySpecializations(
            ReflectionFunction func,
            ReflectionGeneric generic
        )
        {
            return PInvoke.spReflectionFunction_applySpecializations(func, generic);
        }

        public static ReflectionFunction ReflectionFunctionSpecializeWithArgTypes(
            ReflectionFunction func,
            Int argTypeCount,
            in ReflectionType argTypes
        )
        {
            return PInvoke.spReflectionFunction_specializeWithArgTypes(func, argTypeCount, in argTypes);
        }

        public static Brutal.Bool8 ReflectionFunctionIsOverloaded(ReflectionFunction func)
        {
            return PInvoke.spReflectionFunction_isOverloaded(func);
        }

        public static uint ReflectionFunctionGetOverloadCount(ReflectionFunction func)
        {
            return PInvoke.spReflectionFunction_getOverloadCount(func);
        }

        public static ReflectionFunction ReflectionFunctionGetOverload(ReflectionFunction func, uint index)
        {
            return PInvoke.spReflectionFunction_getOverload(func, index);
        }

        public static uint ReflectionDeclGetChildrenCount(ReflectionDecl parentDecl)
        {
            return PInvoke.spReflectionDecl_getChildrenCount(parentDecl);
        }

        public static ReflectionDecl ReflectionDeclGetChild(ReflectionDecl parentDecl, uint index)
        {
            return PInvoke.spReflectionDecl_getChild(parentDecl, index);
        }

        public static string ReflectionDeclGetName(ReflectionDecl decl)
        {
            return PInvoke.spReflectionDecl_getName(decl);
        }

        public static Brutal.SlangApi.DeclKind ReflectionDeclGetKind(ReflectionDecl decl)
        {
            return PInvoke.spReflectionDecl_getKind(decl);
        }

        public static ReflectionFunction ReflectionDeclCastToFunction(ReflectionDecl decl)
        {
            return PInvoke.spReflectionDecl_castToFunction(decl);
        }

        public static ReflectionVariable ReflectionDeclCastToVariable(ReflectionDecl decl)
        {
            return PInvoke.spReflectionDecl_castToVariable(decl);
        }

        public static ReflectionGeneric ReflectionDeclCastToGeneric(ReflectionDecl decl)
        {
            return PInvoke.spReflectionDecl_castToGeneric(decl);
        }

        public static ReflectionType ReflectionGetTypeFromDecl(ReflectionDecl decl)
        {
            return PInvoke.spReflection_getTypeFromDecl(decl);
        }

        public static ReflectionDecl ReflectionDeclGetParent(ReflectionDecl decl)
        {
            return PInvoke.spReflectionDecl_getParent(decl);
        }

        public static ReflectionModifier ReflectionDeclFindModifier(
            ReflectionDecl decl,
            Brutal.SlangApi.ModifierID modifierID
        )
        {
            return PInvoke.spReflectionDecl_findModifier(decl, modifierID);
        }

        public static ReflectionDecl ReflectionGenericAsDecl(ReflectionGeneric generic)
        {
            return PInvoke.spReflectionGeneric_asDecl(generic);
        }

        public static string ReflectionGenericGetName(ReflectionGeneric generic)
        {
            return PInvoke.spReflectionGeneric_GetName(generic);
        }

        public static uint ReflectionGenericGetTypeParameterCount(ReflectionGeneric generic)
        {
            return PInvoke.spReflectionGeneric_GetTypeParameterCount(generic);
        }

        public static ReflectionVariable ReflectionGenericGetTypeParameter(ReflectionGeneric generic, uint index)
        {
            return PInvoke.spReflectionGeneric_GetTypeParameter(generic, index);
        }

        public static uint ReflectionGenericGetValueParameterCount(ReflectionGeneric generic)
        {
            return PInvoke.spReflectionGeneric_GetValueParameterCount(generic);
        }

        public static ReflectionVariable ReflectionGenericGetValueParameter(ReflectionGeneric generic, uint index)
        {
            return PInvoke.spReflectionGeneric_GetValueParameter(generic, index);
        }

        public static uint ReflectionGenericGetTypeParameterConstraintCount(
            ReflectionGeneric generic,
            ReflectionVariable typeParam
        )
        {
            return PInvoke.spReflectionGeneric_GetTypeParameterConstraintCount(generic, typeParam);
        }

        public static ReflectionType ReflectionGenericGetTypeParameterConstraintType(
            ReflectionGeneric generic,
            ReflectionVariable typeParam,
            uint index
        )
        {
            return PInvoke.spReflectionGeneric_GetTypeParameterConstraintType(generic, typeParam, index);
        }

        public static Brutal.SlangApi.DeclKind ReflectionGenericGetInnerKind(ReflectionGeneric generic)
        {
            return PInvoke.spReflectionGeneric_GetInnerKind(generic);
        }

        public static ReflectionDecl ReflectionGenericGetInnerDecl(ReflectionGeneric generic)
        {
            return PInvoke.spReflectionGeneric_GetInnerDecl(generic);
        }

        public static ReflectionGeneric ReflectionGenericGetOuterGenericContainer(ReflectionGeneric generic)
        {
            return PInvoke.spReflectionGeneric_GetOuterGenericContainer(generic);
        }

        public static ReflectionType ReflectionGenericGetConcreteType(
            ReflectionGeneric generic,
            ReflectionVariable typeParam
        )
        {
            return PInvoke.spReflectionGeneric_GetConcreteType(generic, typeParam);
        }

        public static long ReflectionGenericGetConcreteIntVal(ReflectionGeneric generic, ReflectionVariable valueParam)
        {
            return PInvoke.spReflectionGeneric_GetConcreteIntVal(generic, valueParam);
        }

        public static ReflectionGeneric ReflectionGenericApplySpecializations(
            ReflectionGeneric currGeneric,
            ReflectionGeneric generic
        )
        {
            return PInvoke.spReflectionGeneric_applySpecializations(currGeneric, generic);
        }

        public static string ReflectionTypeParameterGetName(ReflectionTypeParameter typeParam)
        {
            return PInvoke.spReflectionTypeParameter_GetName(typeParam);
        }

        public static uint ReflectionTypeParameterGetIndex(ReflectionTypeParameter typeParam)
        {
            return PInvoke.spReflectionTypeParameter_GetIndex(typeParam);
        }

        public static uint ReflectionTypeParameterGetConstraintCount(ReflectionTypeParameter typeParam)
        {
            return PInvoke.spReflectionTypeParameter_GetConstraintCount(typeParam);
        }

        public static ReflectionType ReflectionTypeParameterGetConstraintByIndex(
            ReflectionTypeParameter typeParam,
            uint index
        )
        {
            return PInvoke.spReflectionTypeParameter_GetConstraintByIndex(typeParam, index);
        }

        public static UInt32 ComputeStringHash(RefString8 chars, ByteSize count)
        {
            return PInvoke.spComputeStringHash(chars, count);
        }

        internal static unsafe nint CreateBlob(void* data, ByteSize size)
        {
            return PInvoke.slang_createBlob(data, size);
        }

        internal static nint LoadModuleFromSource(
            nint session,
            RefString8 moduleName,
            RefString8 path,
            RefString8 source,
            ByteSize sourceSize,
            out nint outDiagnostics
        )
        {
            return PInvoke.slang_loadModuleFromSource(
                session,
                moduleName,
                path,
                source,
                sourceSize,
                out outDiagnostics
            );
        }

        internal static unsafe nint LoadModuleFromIRBlob(
            nint session,
            RefString8 moduleName,
            RefString8 path,
            void* source,
            ByteSize sourceSize,
            out nint outDiagnostics
        )
        {
            return PInvoke.slang_loadModuleFromIRBlob(
                session,
                moduleName,
                path,
                source,
                sourceSize,
                out outDiagnostics
            );
        }

        internal static unsafe Result LoadModuleInfoFromIRBlob(
            nint session,
            void* source,
            ByteSize sourceSize,
            ref Int outModuleVersion,
            ref byte* outModuleCompilerVersion,
            ref byte* outModuleName
        )
        {
            return PInvoke.slang_loadModuleInfoFromIRBlob(
                session,
                source,
                sourceSize,
                ref outModuleVersion,
                ref outModuleCompilerVersion,
                ref outModuleName
            );
        }

        internal static Result CreateGlobalSession(Int apiVersion, out nint outGlobalSession)
        {
            return PInvoke.slang_createGlobalSession(apiVersion, out outGlobalSession);
        }

        internal static Result CreateGlobalSession2(
            in Brutal.SlangApi.GlobalSessionDesc desc,
            out nint outGlobalSession
        )
        {
            return PInvoke.slang_createGlobalSession2(in desc, out outGlobalSession);
        }

        internal static Result CreateGlobalSessionWithoutCoreModule(Int apiVersion, out nint outGlobalSession)
        {
            return PInvoke.slang_createGlobalSessionWithoutCoreModule(apiVersion, out outGlobalSession);
        }

        public static void Shutdown()
        {
            PInvoke.slang_shutdown();
        }

        public static void EnableRecordLayer(Brutal.Bool8 enable)
        {
            PInvoke.slang_enableRecordLayer(enable);
        }

        public static Brutal.Bool8 IsRecordLayerEnabled()
        {
            return PInvoke.slang_isRecordLayerEnabled();
        }

        public static void SetReplayDirectory(RefString8 path)
        {
            PInvoke.slang_setReplayDirectory(path);
        }

        public static string GetReplayDirectory()
        {
            return PInvoke.slang_getReplayDirectory();
        }

        public static string GetCurrentReplayPath()
        {
            return PInvoke.slang_getCurrentReplayPath();
        }

        public static Result LoadReplay(RefString8 folderPath)
        {
            return PInvoke.slang_loadReplay(folderPath);
        }

        public static Result LoadLatestReplay()
        {
            return PInvoke.slang_loadLatestReplay();
        }

        public static void ReplayMarker(RefString8 label)
        {
            PInvoke.slang_replayMarker(label);
        }

        public static string GetLastInternalErrorMessage()
        {
            return PInvoke.slang_getLastInternalErrorMessage();
        }

        internal static Result CreateByteCodeRunner(
            in Brutal.SlangApi.ByteCodeRunnerDesc desc,
            out nint outByteCodeRunner
        )
        {
            return PInvoke.slang_createByteCodeRunner(in desc, out outByteCodeRunner);
        }

        internal static Result DisassembleByteCode(nint moduleBlob, out nint outDisassemblyBlob)
        {
            return PInvoke.slang_disassembleByteCode(moduleBlob, out outDisassemblyBlob);
        }
    };
}
