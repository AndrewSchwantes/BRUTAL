using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using Brutal.Interop;
using Brutal.Strings;

namespace Brutal.SlangApi
{
    [GeneratedComInterface]
    [Guid("00000000-0000-0000-c000-000000000046")]
    public unsafe partial interface ISlangUnknown { };

    [GeneratedComInterface]
    [Guid("87ede0e1-4852-44b0-8bf2-cb31874de239")]
    public unsafe partial interface ISlangCastable
    {
        [PreserveSig]
        void* CastAs(ref Brutal.SlangApi.Uuid guid);
    };

    [GeneratedComInterface]
    [Guid("1ec36168-e9f4-430d-bb17-048a8046b31f")]
    public unsafe partial interface ISlangClonable : Brutal.SlangApi.ISlangCastable
    {
        [PreserveSig]
        void* Clone(ref Brutal.SlangApi.Uuid guid);
    };

    [GeneratedComInterface]
    [Guid("8ba5fb08-5195-40e2-ac58-0d989c3a0102")]
    public unsafe partial interface ISlangBlob
    {
        [PreserveSig]
        void* GetBufferPointer();

        [PreserveSig]
        [return: MarshalUsing(typeof(SlangSizeMarshaller))]
        ByteSize GetBufferSize();
    };

    [GeneratedComInterface]
    [Guid("003a09fc-3a4d-4ba0-ad60-1fd863a915ab")]
    public unsafe partial interface ISlangFileSystem : Brutal.SlangApi.ISlangCastable
    {
        [PreserveSig]
        Result LoadFile(
            byte* path,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<ISlangBlob>))] out ISlangBlob outBlob
        );
    };

    [GeneratedComInterface]
    [Guid("9c9d5bc5-eb61-496f-80d7-d147c4a23730")]
    public unsafe partial interface ISlangSharedLibrary_Dep1
    {
        [PreserveSig]
        void* FindSymbolAddressByName(byte* name);
    };

    [GeneratedComInterface]
    [Guid("70dbc7c4-dc3b-4a07-ae7e-752af6a81555")]
    public unsafe partial interface ISlangSharedLibrary : Brutal.SlangApi.ISlangCastable
    {
        [PreserveSig]
        void* FindSymbolAddressByName(byte* name);
    };

    [GeneratedComInterface]
    [Guid("6264ab2b-a3e8-4a06-97f1-49bc2d2ab14d")]
    public unsafe partial interface ISlangSharedLibraryLoader
    {
        [PreserveSig]
        Result LoadSharedLibrary(
            byte* path,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<ISlangSharedLibrary>))]
                out ISlangSharedLibrary sharedLibraryOut
        );
    };

    [GeneratedComInterface]
    [Guid("5fb632d2-979d-4481-9fee-663c3f1449e1")]
    public unsafe partial interface ISlangFileSystemExt : Brutal.SlangApi.ISlangFileSystem
    {
        [PreserveSig]
        Result GetFileUniqueIdentity(
            byte* path,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<ISlangBlob>))] out ISlangBlob outUniqueIdentity
        );

        [PreserveSig]
        Result CalcCombinedPath(
            Brutal.SlangApi.PathType fromPathType,
            byte* fromPath,
            byte* path,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<ISlangBlob>))] out ISlangBlob pathOut
        );

        [PreserveSig]
        Result GetPathType(byte* path, out Brutal.SlangApi.PathType pathTypeOut);

        [PreserveSig]
        Result GetPath(
            PathKind kind,
            byte* path,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<ISlangBlob>))] out ISlangBlob outPath
        );

        [PreserveSig]
        void ClearCache();

        [PreserveSig]
        Result EnumeratePathContents(byte* path, FileSystemContentsCallBack callback, void* userData);

        [PreserveSig]
        OSPathKind GetOSPathKind();
    };

    [GeneratedComInterface]
    [Guid("a058675c-1d65-452a-8458-ccded1427105")]
    public unsafe partial interface ISlangMutableFileSystem : Brutal.SlangApi.ISlangFileSystemExt
    {
        [PreserveSig]
        Result SaveFile(byte* path, void* data, [MarshalUsing(typeof(SlangSizeMarshaller))] ByteSize size);

        [PreserveSig]
        Result SaveFileBlob(
            byte* path,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<ISlangBlob>))] ISlangBlob dataBlob
        );

        [PreserveSig]
        Result Remove(byte* path);

        [PreserveSig]
        Result CreateDirectory(byte* path);
    };

    [GeneratedComInterface]
    [Guid("ec457f0e-9add-4e6b-851c-d7fa716d15fd")]
    public unsafe partial interface ISlangWriter
    {
        [PreserveSig]
        byte* BeginAppendBuffer([MarshalUsing(typeof(SlangSizeMarshaller))] ByteSize maxNumChars);

        [PreserveSig]
        Result EndAppendBuffer(byte* buffer, [MarshalUsing(typeof(SlangSizeMarshaller))] ByteSize numChars);

        [PreserveSig]
        Result Write(byte* chars, [MarshalUsing(typeof(SlangSizeMarshaller))] ByteSize numChars);

        [PreserveSig]
        void Flush();

        [PreserveSig]
        [return: MarshalUsing(typeof(SlangBoolMarshaller))]
        Bool IsConsole();

        [PreserveSig]
        Result SetMode(Brutal.SlangApi.WriterMode mode);
    };

    [GeneratedComInterface]
    [Guid("197772c7-0155-4b91-84e8-6668baff0619")]
    public unsafe partial interface ISlangProfiler
    {
        [PreserveSig]
        [return: MarshalUsing(typeof(SlangSizeMarshaller))]
        ByteSize GetEntryCount();

        [PreserveSig]
        byte* GetEntryName(uint index);

        [PreserveSig]
        long GetEntryTimeMS(uint index);

        [PreserveSig]
        uint GetEntryInvocationTimes(uint index);
    };

    [GeneratedComInterface]
    [Guid("c140b5fd-0c78-452e-ba7c-1a1e70c7f71c")]
    public unsafe partial interface IGlobalSession
    {
        [PreserveSig]
        Result CreateSession(
            ref Brutal.SlangApi.SessionDesc desc,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<Brutal.SlangApi.ISession>))]
                out Brutal.SlangApi.ISession outSession
        );

        [PreserveSig]
        Brutal.SlangApi.ProfileID FindProfile(byte* name);

        [PreserveSig]
        void SetDownstreamCompilerPath(Brutal.SlangApi.PassThrough passThrough, byte* path);

        [PreserveSig]
        void SetDownstreamCompilerPrelude(Brutal.SlangApi.PassThrough passThrough, byte* preludeText);

        [PreserveSig]
        void GetDownstreamCompilerPrelude(
            Brutal.SlangApi.PassThrough passThrough,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<ISlangBlob>))] out ISlangBlob outPrelude
        );

        [PreserveSig]
        byte* GetBuildTagString();

        [PreserveSig]
        Result SetDefaultDownstreamCompiler(
            Brutal.SlangApi.SourceLanguage sourceLanguage,
            Brutal.SlangApi.PassThrough defaultCompiler
        );

        [PreserveSig]
        Brutal.SlangApi.PassThrough GetDefaultDownstreamCompiler(Brutal.SlangApi.SourceLanguage sourceLanguage);

        [PreserveSig]
        void SetLanguagePrelude(Brutal.SlangApi.SourceLanguage sourceLanguage, byte* preludeText);

        [PreserveSig]
        void GetLanguagePrelude(
            Brutal.SlangApi.SourceLanguage sourceLanguage,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<ISlangBlob>))] out ISlangBlob outPrelude
        );

        [PreserveSig]
        Result CreateCompileRequest(
            [MarshalUsing(typeof(SlangHandleMarshaller<Brutal.SlangApi.ICompileRequest>))]
                out Brutal.SlangApi.ICompileRequest outCompileRequest
        );

        [PreserveSig]
        void AddBuiltins(byte* sourcePath, byte* sourceString);

        [PreserveSig]
        void SetSharedLibraryLoader(
            [MarshalUsing(typeof(SlangInterfaceMarshaller<ISlangSharedLibraryLoader>))] ISlangSharedLibraryLoader loader
        );

        [PreserveSig]
        [return: MarshalUsing(typeof(BorrowedSlangInterfaceMarshaller<ISlangSharedLibraryLoader>))]
        ISlangSharedLibraryLoader? GetSharedLibraryLoader();

        [PreserveSig]
        Result CheckCompileTargetSupport(Brutal.SlangApi.CompileTarget target);

        [PreserveSig]
        Result CheckPassThroughSupport(Brutal.SlangApi.PassThrough passThrough);

        [PreserveSig]
        Result CompileCoreModule(CompileCoreModuleFlags flags);

        [PreserveSig]
        Result LoadCoreModule(
            void* coreModule,
            [MarshalUsing(typeof(SlangSizeMarshaller))] ByteSize coreModuleSizeInBytes
        );

        [PreserveSig]
        Result SaveCoreModule(
            Brutal.SlangApi.ArchiveType archiveType,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<ISlangBlob>))] out ISlangBlob outBlob
        );

        [PreserveSig]
        Brutal.SlangApi.CapabilityID FindCapability(byte* name);

        [PreserveSig]
        void SetDownstreamCompilerForTransition(
            Brutal.SlangApi.CompileTarget source,
            Brutal.SlangApi.CompileTarget target,
            Brutal.SlangApi.PassThrough compiler
        );

        [PreserveSig]
        Brutal.SlangApi.PassThrough GetDownstreamCompilerForTransition(
            Brutal.SlangApi.CompileTarget source,
            Brutal.SlangApi.CompileTarget target
        );

        [PreserveSig]
        void GetCompilerElapsedTime(out double outTotalTime, out double outDownstreamTime);

        [PreserveSig]
        Result SetSpirvCoreGrammar(byte* jsonPath);

        [PreserveSig]
        Result ParseCommandLineArguments(
            int argc,
            byte** argv,
            out Brutal.SlangApi.SessionDesc outSessionDesc,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<ISlangUnknown>))] out ISlangUnknown outAuxAllocation
        );

        [PreserveSig]
        Result GetSessionDescDigest(
            in Brutal.SlangApi.SessionDesc sessionDesc,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<ISlangBlob>))] out ISlangBlob outBlob
        );

        [PreserveSig]
        Result CompileBuiltinModule(Brutal.SlangApi.BuiltinModuleName module, CompileCoreModuleFlags flags);

        [PreserveSig]
        Result LoadBuiltinModule(
            Brutal.SlangApi.BuiltinModuleName module,
            void* moduleData,
            [MarshalUsing(typeof(SlangSizeMarshaller))] ByteSize sizeInBytes
        );

        [PreserveSig]
        Result SaveBuiltinModule(
            Brutal.SlangApi.BuiltinModuleName module,
            Brutal.SlangApi.ArchiveType archiveType,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<ISlangBlob>))] out ISlangBlob outBlob
        );
    };

    [GeneratedComInterface]
    [Guid("67618701-d116-468f-ab3b-474bedce0e3d")]
    public unsafe partial interface ISession
    {
        [PreserveSig]
        [return: MarshalUsing(typeof(BorrowedSlangInterfaceMarshaller<Brutal.SlangApi.IGlobalSession>))]
        Brutal.SlangApi.IGlobalSession? GetGlobalSession();

        [PreserveSig]
        [return: MarshalUsing(typeof(BorrowedSlangInterfaceMarshaller<Brutal.SlangApi.IModule>))]
        Brutal.SlangApi.IModule? LoadModule(
            byte* moduleName,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<IBlob>))] out IBlob outDiagnostics
        );

        [PreserveSig]
        [return: MarshalUsing(typeof(BorrowedSlangInterfaceMarshaller<Brutal.SlangApi.IModule>))]
        Brutal.SlangApi.IModule? LoadModuleFromSource(
            byte* moduleName,
            byte* path,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<IBlob>))] IBlob source,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<IBlob>))] out IBlob outDiagnostics
        );

        [PreserveSig]
        Result CreateCompositeComponentType(
            nint* componentTypes,
            Int componentTypeCount,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<Brutal.SlangApi.IComponentType>))]
                out Brutal.SlangApi.IComponentType outCompositeComponentType,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<ISlangBlob>))] out ISlangBlob outDiagnostics
        );

        [PreserveSig]
        [return: MarshalUsing(typeof(SlangHandleMarshaller<Brutal.SlangApi.TypeReflection>))]
        Brutal.SlangApi.TypeReflection SpecializeType(
            [MarshalUsing(typeof(SlangHandleMarshaller<Brutal.SlangApi.TypeReflection>))]
                Brutal.SlangApi.TypeReflection type,
            in Brutal.SlangApi.SpecializationArg specializationArgs,
            Int specializationArgCount,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<ISlangBlob>))] out ISlangBlob outDiagnostics
        );

        [PreserveSig]
        [return: MarshalUsing(typeof(SlangHandleMarshaller<Brutal.SlangApi.TypeLayoutReflection>))]
        Brutal.SlangApi.TypeLayoutReflection GetTypeLayout(
            [MarshalUsing(typeof(SlangHandleMarshaller<Brutal.SlangApi.TypeReflection>))]
                Brutal.SlangApi.TypeReflection type,
            Int targetIndex,
            Brutal.SlangApi.LayoutRules rules,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<ISlangBlob>))] out ISlangBlob outDiagnostics
        );

        [PreserveSig]
        [return: MarshalUsing(typeof(SlangHandleMarshaller<Brutal.SlangApi.TypeReflection>))]
        Brutal.SlangApi.TypeReflection GetContainerType(
            [MarshalUsing(typeof(SlangHandleMarshaller<Brutal.SlangApi.TypeReflection>))]
                Brutal.SlangApi.TypeReflection elementType,
            Brutal.SlangApi.ContainerType containerType,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<ISlangBlob>))] out ISlangBlob outDiagnostics
        );

        [PreserveSig]
        [return: MarshalUsing(typeof(SlangHandleMarshaller<Brutal.SlangApi.TypeReflection>))]
        Brutal.SlangApi.TypeReflection GetDynamicType();

        [PreserveSig]
        Result GetTypeRTTIMangledName(
            [MarshalUsing(typeof(SlangHandleMarshaller<Brutal.SlangApi.TypeReflection>))]
                Brutal.SlangApi.TypeReflection type,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<ISlangBlob>))] out ISlangBlob outNameBlob
        );

        [PreserveSig]
        Result GetTypeConformanceWitnessMangledName(
            [MarshalUsing(typeof(SlangHandleMarshaller<Brutal.SlangApi.TypeReflection>))]
                Brutal.SlangApi.TypeReflection type,
            [MarshalUsing(typeof(SlangHandleMarshaller<Brutal.SlangApi.TypeReflection>))]
                Brutal.SlangApi.TypeReflection interfaceType,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<ISlangBlob>))] out ISlangBlob outNameBlob
        );

        [PreserveSig]
        Result GetTypeConformanceWitnessSequentialID(
            [MarshalUsing(typeof(SlangHandleMarshaller<Brutal.SlangApi.TypeReflection>))]
                Brutal.SlangApi.TypeReflection type,
            [MarshalUsing(typeof(SlangHandleMarshaller<Brutal.SlangApi.TypeReflection>))]
                Brutal.SlangApi.TypeReflection interfaceType,
            out uint outId
        );

        [PreserveSig]
        Result CreateCompileRequest(
            [MarshalUsing(typeof(SlangHandleMarshaller<CompileRequest>))] out CompileRequest outCompileRequest
        );

        [PreserveSig]
        Result CreateTypeConformanceComponentType(
            [MarshalUsing(typeof(SlangHandleMarshaller<Brutal.SlangApi.TypeReflection>))]
                Brutal.SlangApi.TypeReflection type,
            [MarshalUsing(typeof(SlangHandleMarshaller<Brutal.SlangApi.TypeReflection>))]
                Brutal.SlangApi.TypeReflection interfaceType,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<Brutal.SlangApi.ITypeConformance>))]
                out Brutal.SlangApi.ITypeConformance outConformance,
            Int conformanceIdOverride,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<ISlangBlob>))] out ISlangBlob outDiagnostics
        );

        [PreserveSig]
        [return: MarshalUsing(typeof(BorrowedSlangInterfaceMarshaller<Brutal.SlangApi.IModule>))]
        Brutal.SlangApi.IModule? LoadModuleFromIRBlob(
            byte* moduleName,
            byte* path,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<IBlob>))] IBlob source,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<IBlob>))] out IBlob outDiagnostics
        );

        [PreserveSig]
        Int GetLoadedModuleCount();

        [PreserveSig]
        [return: MarshalUsing(typeof(BorrowedSlangInterfaceMarshaller<Brutal.SlangApi.IModule>))]
        Brutal.SlangApi.IModule? GetLoadedModule(Int index);

        [PreserveSig]
        [return: MarshalUsing(typeof(SlangBoolMarshaller))]
        Brutal.Bool8 IsBinaryModuleUpToDate(
            byte* modulePath,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<IBlob>))] IBlob binaryModuleBlob
        );

        [PreserveSig]
        [return: MarshalUsing(typeof(BorrowedSlangInterfaceMarshaller<Brutal.SlangApi.IModule>))]
        Brutal.SlangApi.IModule? LoadModuleFromSourceString(
            byte* moduleName,
            byte* path,
            byte* @string,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<IBlob>))] out IBlob outDiagnostics
        );

        [PreserveSig]
        Result GetDynamicObjectRTTIBytes(
            [MarshalUsing(typeof(SlangHandleMarshaller<Brutal.SlangApi.TypeReflection>))]
                Brutal.SlangApi.TypeReflection type,
            [MarshalUsing(typeof(SlangHandleMarshaller<Brutal.SlangApi.TypeReflection>))]
                Brutal.SlangApi.TypeReflection interfaceType,
            uint* outRTTIDataBuffer,
            uint bufferSizeInBytes
        );

        [PreserveSig]
        Result LoadModuleInfoFromIRBlob(
            [MarshalUsing(typeof(SlangInterfaceMarshaller<IBlob>))] IBlob source,
            ref Int outModuleVersion,
            ref byte* outModuleCompilerVersion,
            ref byte* outModuleName
        );

        [PreserveSig]
        Result GetDeclSourceLocation(
            [MarshalUsing(typeof(SlangHandleMarshaller<Brutal.SlangApi.DeclReflection>))]
                Brutal.SlangApi.DeclReflection decl,
            out Brutal.SlangApi.SourceLocation outLocation
        );
    };

    [GeneratedComInterface]
    [Guid("5bc42be8-5c50-4929-9e5e-d15e7c24015f")]
    public unsafe partial interface IComponentType
    {
        [PreserveSig]
        [return: MarshalUsing(typeof(BorrowedSlangInterfaceMarshaller<Brutal.SlangApi.ISession>))]
        Brutal.SlangApi.ISession? GetSession();

        [PreserveSig]
        [return: MarshalUsing(typeof(SlangHandleMarshaller<ProgramLayout>))]
        ProgramLayout GetLayout(
            Int targetIndex,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<IBlob>))] out IBlob outDiagnostics
        );

        [PreserveSig]
        Int GetSpecializationParamCount();

        [PreserveSig]
        Result GetEntryPointCode(
            Int entryPointIndex,
            Int targetIndex,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<IBlob>))] out IBlob outCode,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<IBlob>))] out IBlob outDiagnostics
        );

        [PreserveSig]
        Result GetResultAsFileSystem(
            Int entryPointIndex,
            Int targetIndex,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<ISlangMutableFileSystem>))]
                out ISlangMutableFileSystem outFileSystem
        );

        [PreserveSig]
        void GetEntryPointHash(
            Int entryPointIndex,
            Int targetIndex,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<IBlob>))] out IBlob outHash
        );

        [PreserveSig]
        Result Specialize(
            in Brutal.SlangApi.SpecializationArg specializationArgs,
            Int specializationArgCount,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<Brutal.SlangApi.IComponentType>))]
                out Brutal.SlangApi.IComponentType outSpecializedComponentType,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<ISlangBlob>))] out ISlangBlob outDiagnostics
        );

        [PreserveSig]
        Result Link(
            [MarshalUsing(typeof(SlangInterfaceMarshaller<Brutal.SlangApi.IComponentType>))]
                out Brutal.SlangApi.IComponentType outLinkedComponentType,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<ISlangBlob>))] out ISlangBlob outDiagnostics
        );

        [PreserveSig]
        Result GetEntryPointHostCallable(
            int entryPointIndex,
            int targetIndex,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<ISlangSharedLibrary>))]
                out ISlangSharedLibrary outSharedLibrary,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<IBlob>))] out IBlob outDiagnostics
        );

        [PreserveSig]
        Result RenameEntryPoint(
            byte* newName,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<Brutal.SlangApi.IComponentType>))]
                out Brutal.SlangApi.IComponentType outEntryPoint
        );

        [PreserveSig]
        Result LinkWithOptions(
            [MarshalUsing(typeof(SlangInterfaceMarshaller<Brutal.SlangApi.IComponentType>))]
                out Brutal.SlangApi.IComponentType outLinkedComponentType,
            uint compilerOptionEntryCount,
            in Brutal.SlangApi.CompilerOptionEntry compilerOptionEntries,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<ISlangBlob>))] out ISlangBlob outDiagnostics
        );

        [PreserveSig]
        Result GetTargetCode(
            Int targetIndex,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<IBlob>))] out IBlob outCode,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<IBlob>))] out IBlob outDiagnostics
        );

        [PreserveSig]
        Result GetTargetMetadata(
            Int targetIndex,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<Brutal.SlangApi.IMetadata>))]
                out Brutal.SlangApi.IMetadata outMetadata,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<IBlob>))] out IBlob outDiagnostics
        );

        [PreserveSig]
        Result GetEntryPointMetadata(
            Int entryPointIndex,
            Int targetIndex,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<Brutal.SlangApi.IMetadata>))]
                out Brutal.SlangApi.IMetadata outMetadata,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<IBlob>))] out IBlob outDiagnostics
        );
    };

    [GeneratedComInterface]
    [Guid("0c720e64-8722-4d31-8990-638a98b1c279")]
    public unsafe partial interface IModule : Brutal.SlangApi.IComponentType
    {
        [PreserveSig]
        Result FindEntryPointByName(
            byte* name,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<Brutal.SlangApi.IEntryPoint>))]
                out Brutal.SlangApi.IEntryPoint outEntryPoint
        );

        [PreserveSig]
        Int32 GetDefinedEntryPointCount();

        [PreserveSig]
        Result GetDefinedEntryPoint(
            Int32 index,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<Brutal.SlangApi.IEntryPoint>))]
                out Brutal.SlangApi.IEntryPoint outEntryPoint
        );

        [PreserveSig]
        Result Serialize([MarshalUsing(typeof(SlangInterfaceMarshaller<ISlangBlob>))] out ISlangBlob outSerializedBlob);

        [PreserveSig]
        Result WriteToFile(byte* fileName);

        [PreserveSig]
        byte* GetName();

        [PreserveSig]
        byte* GetFilePath();

        [PreserveSig]
        byte* GetUniqueIdentity();

        [PreserveSig]
        Result FindAndCheckEntryPoint(
            byte* name,
            Brutal.SlangApi.Stage stage,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<Brutal.SlangApi.IEntryPoint>))]
                out Brutal.SlangApi.IEntryPoint outEntryPoint,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<ISlangBlob>))] out ISlangBlob outDiagnostics
        );

        [PreserveSig]
        Int32 GetDependencyFileCount();

        [PreserveSig]
        byte* GetDependencyFilePath(Int32 index);

        [PreserveSig]
        [return: MarshalUsing(typeof(SlangHandleMarshaller<Brutal.SlangApi.DeclReflection>))]
        Brutal.SlangApi.DeclReflection GetModuleReflection();

        [PreserveSig]
        Result Disassemble([MarshalUsing(typeof(SlangInterfaceMarshaller<IBlob>))] out IBlob outDisassembledBlob);
    };

    [GeneratedComInterface]
    [Guid("73eb3147-e544-41b5-b8f0-a244df21940b")]
    public unsafe partial interface ITypeConformance : Brutal.SlangApi.IComponentType { };

    [GeneratedComInterface]
    [Guid("8044a8a3-ddc0-4b7f-af8e-026e905d7332")]
    public unsafe partial interface IMetadata : Brutal.SlangApi.ISlangCastable
    {
        [PreserveSig]
        Result IsParameterLocationUsed(
            Brutal.SlangApi.ParameterCategory category,
            UInt spaceIndex,
            UInt registerIndex,
            [MarshalUsing(typeof(SlangBoolMarshaller))] ref Brutal.Bool8 outUsed
        );

        [PreserveSig]
        byte* GetDebugBuildIdentifier();
    };

    [GeneratedComInterface]
    [Guid("5fa9380e-b62f-41e5-9f12-4bad4d9eaae4")]
    public unsafe partial interface ICompileResult : Brutal.SlangApi.ISlangCastable
    {
        [PreserveSig]
        uint GetItemCount();

        [PreserveSig]
        Result GetItemData(uint index, [MarshalUsing(typeof(SlangInterfaceMarshaller<IBlob>))] out IBlob outblob);

        [PreserveSig]
        Result GetMetadata(
            [MarshalUsing(typeof(SlangInterfaceMarshaller<Brutal.SlangApi.IMetadata>))]
                out Brutal.SlangApi.IMetadata outMetadata
        );
    };

    [GeneratedComInterface]
    [Guid("8f241361-f5bd-4ca0-a3ac-02f7fa2402b8")]
    public unsafe partial interface IEntryPoint : Brutal.SlangApi.IComponentType
    {
        [PreserveSig]
        [return: MarshalUsing(typeof(SlangHandleMarshaller<Brutal.SlangApi.FunctionReflection>))]
        Brutal.SlangApi.FunctionReflection GetFunctionReflection();
    };

    [GeneratedComInterface]
    [Guid("9c2a4b3d-7f68-4e91-a52c-8b193e457a9f")]
    public unsafe partial interface IComponentType2
    {
        [PreserveSig]
        Result GetTargetCompileResult(
            Int targetIndex,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<Brutal.SlangApi.ICompileResult>))]
                out Brutal.SlangApi.ICompileResult outCompileResult,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<IBlob>))] out IBlob outDiagnostics
        );

        [PreserveSig]
        Result GetEntryPointCompileResult(
            Int entryPointIndex,
            Int targetIndex,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<Brutal.SlangApi.ICompileResult>))]
                out Brutal.SlangApi.ICompileResult outCompileResult,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<IBlob>))] out IBlob outDiagnostics
        );

        [PreserveSig]
        Result GetTargetHostCallable(
            int targetIndex,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<ISlangSharedLibrary>))]
                out ISlangSharedLibrary outSharedLibrary,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<IBlob>))] out IBlob outDiagnostics
        );
    };

    [GeneratedComInterface]
    [Guid("8e12e8e3-5fcd-433e-afcb-13a088bc5ee5")]
    public unsafe partial interface IModulePrecompileService_Experimental
    {
        [PreserveSig]
        Result PrecompileForTarget(
            Brutal.SlangApi.CompileTarget target,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<ISlangBlob>))] out ISlangBlob outDiagnostics
        );

        [PreserveSig]
        Result GetPrecompiledTargetCode(
            Brutal.SlangApi.CompileTarget target,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<IBlob>))] out IBlob outCode,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<IBlob>))] out IBlob outDiagnostics
        );

        [PreserveSig]
        Int GetModuleDependencyCount();

        [PreserveSig]
        Result GetModuleDependency(
            Int dependencyIndex,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<Brutal.SlangApi.IModule>))]
                out Brutal.SlangApi.IModule outModule,
            [MarshalUsing(typeof(SlangInterfaceMarshaller<IBlob>))] out IBlob outDiagnostics
        );
    };

    [GeneratedComInterface]
    [Guid("afdab195-361f-42cb-9513-9006261dd8cd")]
    public unsafe partial interface IByteCodeRunner
    {
        [PreserveSig]
        Result LoadModule([MarshalUsing(typeof(SlangInterfaceMarshaller<IBlob>))] IBlob moduleBlob);

        [PreserveSig]
        Result SelectFunctionByIndex(uint functionIndex);

        [PreserveSig]
        int FindFunctionByName(byte* name);

        [PreserveSig]
        Result GetFunctionInfo(uint index, out Brutal.SlangApi.ByteCodeFuncInfo outInfo);

        [PreserveSig]
        void* GetCurrentWorkingSet();

        [PreserveSig]
        Result Execute(void* argumentData, [MarshalUsing(typeof(SlangSizeMarshaller))] ByteSize argumentSize);

        [PreserveSig]
        void GetErrorString([MarshalUsing(typeof(SlangInterfaceMarshaller<IBlob>))] out IBlob outBlob);

        [PreserveSig]
        void* GetReturnValue([MarshalUsing(typeof(SlangSizeMarshaller))] out ByteSize outValueSize);

        [PreserveSig]
        void SetExtInstHandlerUserData(void* userData);

        [PreserveSig]
        Result RegisterExtCall(byte* name, VMExtFunction functionPtr);

        [PreserveSig]
        Result SetPrintCallback(VMPrintFunc callback, void* userData);
    };
}
