using System.Runtime.InteropServices.Marshalling;
using Brutal.Strings;

namespace Brutal.SlangApi;

/// <summary>
///     Hand-written ergonomic helpers on top of the generated <see cref="ICompileRequest"/>
///     handle struct. The deprecated <c>sp*</c> compile-request flow trafficks in COM
///     interfaces as raw <c>nint</c> (because flat-C entry points can't carry
///     <c>[GeneratedComInterface]</c> proxies through source-gen marshalling); this layer
///     bridges those nints to/from managed COM proxies via <see cref="Slang.WrapComInstance"/>
///     and <see cref="Slang.GetComInstance"/>.
/// </summary>
public static unsafe class ICompileRequestExtensions
{
    /// <summary>
    ///     Returns the <see cref="ISession"/> the request was created against, wrapping the
    ///     raw COM pointer that the deprecated <c>spCompileRequest_getSession</c> entry point
    ///     produces. Returns <c>null</c> when the call fails or yields a null pointer.
    /// </summary>
    public static Result GetSession(this ICompileRequest self, out ISession? session)
    {
        var hr = self.GetSession(out var raw);
        session = Slang.WrapComInstance<ISession>(hr, raw);
        return hr;
    }

    /// <summary>
    ///     Returns the linked program assembled from the request's translation units as a
    ///     managed <see cref="IComponentType"/>. Returns <c>null</c> when the call fails or
    ///     slang yields a null pointer.
    /// </summary>
    public static Result GetProgram(this ICompileRequest self, out IComponentType? program)
    {
        var hr = self.GetProgram(out var raw);
        program = Slang.WrapComInstance<IComponentType>(hr, raw);
        return hr;
    }

    /// <summary>
    ///     Returns the linked program plus its entry points as a single
    ///     <see cref="IComponentType"/>. Returns <c>null</c> when the call fails or slang
    ///     yields a null pointer.
    /// </summary>
    public static Result GetProgramWithEntryPoints(this ICompileRequest self, out IComponentType? program)
    {
        var hr = self.GetProgramWithEntryPoints(out var raw);
        program = Slang.WrapComInstance<IComponentType>(hr, raw);
        return hr;
    }

    /// <summary>
    ///     Returns the entry point at <paramref name="entryPointIndex"/> as a standalone
    ///     <see cref="IComponentType"/>. Returns <c>null</c> when the call fails or slang
    ///     yields a null pointer.
    /// </summary>
    public static Result GetEntryPoint(this ICompileRequest self, Int entryPointIndex, out IComponentType? entryPoint)
    {
        var hr = self.GetEntryPoint(entryPointIndex, out var raw);
        entryPoint = Slang.WrapComInstance<IComponentType>(hr, raw);
        return hr;
    }

    /// <summary>
    ///     Returns the loaded <see cref="IModule"/> for the translation unit at
    ///     <paramref name="translationUnitIndex"/>. Returns <c>null</c> when the call fails
    ///     or slang yields a null pointer.
    /// </summary>
    public static Result GetModule(this ICompileRequest self, Int translationUnitIndex, out IModule? module)
    {
        var hr = self.GetModule(translationUnitIndex, out var raw);
        module = Slang.WrapComInstance<IModule>(hr, raw);
        return hr;
    }

    /// <summary>
    ///     Sets the <see cref="ISlangFileSystem"/> used to resolve source includes and other
    ///     file lookups for this request. Pass <c>null</c> to restore slang's default file
    ///     system. The native side AddRefs the supplied object internally.
    /// </summary>
    public static void SetFileSystem(this ICompileRequest self, ISlangFileSystem? fileSystem)
    {
        if (fileSystem is null)
        {
            self.SetFileSystem(0);
            return;
        }
        var raw = Slang.GetComInstance(fileSystem);
        try
        {
            self.SetFileSystem(raw);
        }
        finally
        {
            Marshal.Release(raw);
        }
    }

    /// <summary>
    ///     Redirects output on <paramref name="channel"/> to the supplied
    ///     <see cref="ISlangWriter"/>. Pass <c>null</c> to restore the channel's default
    ///     writer. The native side AddRefs the supplied object internally.
    /// </summary>
    public static void SetWriter(this ICompileRequest self, WriterChannel channel, ISlangWriter? writer)
    {
        if (writer is null)
        {
            self.SetWriter(channel, 0);
            return;
        }
        var raw = Slang.GetComInstance(writer);
        try
        {
            self.SetWriter(channel, raw);
        }
        finally
        {
            Marshal.Release(raw);
        }
    }

    /// <summary>
    ///     Returns the <see cref="ISlangWriter"/> currently installed for
    ///     <paramref name="channel"/>, or <c>null</c> if the channel is unset. Slang's
    ///     getter lends the pointer without an AddRef; the returned proxy takes its own
    ///     reference, so release it with <see cref="Slang.Release"/>.
    /// </summary>
    public static ISlangWriter? GetWriter(this ICompileRequest self, WriterChannel channel) =>
        Slang.WrapBorrowedComInstance<ISlangWriter>(self.GetWriter(channel));

    /// <summary>
    ///     Adds a translation-unit source whose contents are provided as an
    ///     <see cref="ISlangBlob"/>. The native side AddRefs the supplied blob internally.
    /// </summary>
    public static void AddTranslationUnitSourceBlob(
        this ICompileRequest self,
        int translationUnitIndex,
        RefString8 path,
        ISlangBlob sourceBlob
    )
    {
        var raw = Slang.GetComInstance(sourceBlob);
        try
        {
            self.AddTranslationUnitSourceBlob(translationUnitIndex, path, raw);
        }
        finally
        {
            Marshal.Release(raw);
        }
    }
}
