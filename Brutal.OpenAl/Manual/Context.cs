namespace Brutal.OpenAlApi;

public sealed partial class Context : IDisposable, INullable
{
    public ref struct CreateInfo()
    {
        public ReadOnlySpan<(AlcContextAttribute Attribute, int Value)> Attributes = [];
    }

    public Device Device { get; }

    public AlcContext Handle { get; private set; }

    public static Context? Current
    {
        get
        {
            var current = Alc.PInvoke.GetCurrentContext();
            if (current.IsNull())
                return null;
            return LiveContexts.TryGetValue(current.Pointer, out var weak) && weak.TryGetTarget(out var context)
                ? context
                : null;
        }
    }

    public bool IsCurrent => !IsNull() && Handle.Pointer == Alc.PInvoke.GetCurrentContext().Pointer;

    internal unsafe Context(Device device, CreateInfo info)
    {
        Device = device;

        // An empty attribute list flattens to a null pointer - see Alc.FlattenAttributes.
        using var owner = Alc.FlattenAttributes(info.Attributes);
        fixed (int* p = owner.Span)
            Handle = Alc.PInvoke.CreateContext(device, p);

        // Failure detail is recorded on the input device, not globally.
        if (Handle.IsNull())
            throw new AlcException(Alc.GetError(device), "alcCreateContext failed.");

        // Prune entries for leaked wrappers; remove by (key, value) so a re-registered entry survives.
        foreach (var entry in LiveContexts)
        {
            if (!entry.Value.TryGetTarget(out _))
                LiveContexts.TryRemove(entry);
        }
        LiveContexts[Handle.Pointer] = new WeakReference<Context>(this);
    }

    public bool IsNull() => Handle.IsNull();

    public static void MakeCurrent(Context? context)
    {
        if (context is null)
        {
            if (!Alc.PInvoke.MakeContextCurrent(AlcContext.Null))
                throw new AlcException(Alc.GetError(), "alcMakeContextCurrent(NULL) failed.");
            return;
        }

        // alcMakeContextCurrent(NULL) detaches, so a disposed context must throw, not pass through.
        if (context.IsNull())
            throw new AlcException(AlcError.InvalidContext, "The context has been disposed.");

        // Failure detail is recorded on the context's device.
        if (!Alc.PInvoke.MakeContextCurrent(context.Handle))
            throw new AlcException(Alc.GetError(context.Device), "alcMakeContextCurrent failed.");
    }

    public static bool TryMakeCurrent(Context? context)
    {
        if (context is null)
            return Alc.PInvoke.MakeContextCurrent(AlcContext.Null);
        return !context.IsNull() && Alc.PInvoke.MakeContextCurrent(context.Handle);
    }

    public void MakeCurrent() => MakeCurrent(this);

    public bool TryMakeCurrent() => TryMakeCurrent(this);

    public static Context? CurrentThread
    {
        get
        {
            var current = Alc.PInvoke.GetThreadContext();
            if (current.IsNull())
                return null;
            return LiveContexts.TryGetValue(current.Pointer, out var weak) && weak.TryGetTarget(out var context)
                ? context
                : null;
        }
    }

    public static void MakeThreadCurrent(Context? context)
    {
        if (context is null)
        {
            if (!Alc.PInvoke.SetThreadContext(AlcContext.Null))
                throw new AlcException(Alc.GetError(), "alcSetThreadContext(NULL) failed.");
            return;
        }

        if (context.IsNull())
            throw new AlcException(AlcError.InvalidContext, "The context has been disposed.");

        // Failure detail is recorded on the context's device.
        if (!Alc.PInvoke.SetThreadContext(context.Handle))
            throw new AlcException(Alc.GetError(context.Device), "alcSetThreadContext failed.");
    }

    public static bool TryMakeThreadCurrent(Context? context)
    {
        if (context is null)
            return Alc.PInvoke.SetThreadContext(AlcContext.Null);
        return !context.IsNull() && Alc.PInvoke.SetThreadContext(context.Handle);
    }

    public void MakeThreadCurrent() => MakeThreadCurrent(this);

    public bool TryMakeThreadCurrent() => TryMakeThreadCurrent(this);

    private AlcContext CheckedHandle
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsNull(), this);
            return Handle;
        }
    }

    public void Process() => Alc.PInvoke.ProcessContext(CheckedHandle);

    public void Suspend() => Alc.PInvoke.SuspendContext(CheckedHandle);

    public void Dispose()
    {
        if (IsNull())
            return;

        // While this context is alive the native object still occupies its address, so the entry
        // under this key is necessarily our own.
        LiveContexts.TryRemove(Handle.Pointer, out _);

        // Release any callback handlers rooted under this address (Al events, AlDebug) so a
        // recycled context address can never resolve to a stale handler.
        Al.EvictContextRoots(Handle.Pointer.Value);

        // alcDestroyContext returns nothing; an invalid context sets the global error - check
        // Alc.GetError() explicitly if you need to know.
        Alc.PInvoke.DestroyContext(Handle);
        Handle = AlcContext.Null;
    }

    public static implicit operator AlcContext(Context context) => context.Handle;
}
