namespace Brutal.SdlApi.Mixer;

public delegate void MixTrackStoppedHandler(MixTrack track);

public delegate void MixTrackMixHandler(MixTrack track, in SdlAudioSpec spec, Span<float> pcm);

public delegate void MixGroupMixHandler(MixGroup group, in SdlAudioSpec spec, Span<float> pcm);

public delegate void MixPostMixHandler(MixMixer mixer, in SdlAudioSpec spec, Span<float> pcm);

public static unsafe partial class SdlMixer
{
    // Which callback slot a registration occupies. SDL_mixer allows one callback of each kind per
    // object; a track can have all three of TrackStopped / TrackRaw / TrackCooked at once, so the kind
    // is part of the registry key (setting Raw must not disturb Cooked, and vice versa).
    private enum CallbackKind
    {
        TrackStopped,
        TrackRaw,
        TrackCooked,
        GroupPostMix,
        PostMix,
    }

    // Registered callback delegates, keyed by (native object pointer, kind). Each GCHandle roots the
    // managed delegate for as long as SDL_mixer holds the native trampoline pointer against that
    // object, and its IntPtr is the userdata SDL threads back to the trampoline so it can dispatch to
    // the right delegate. Guarded by a lock because SDL_mixer permits (un)registering from any thread,
    // and the audio thread reads the GCHandle target concurrently. Freed on replace, on clear (setting
    // a null handler), and wholesale on Quit.
    private static readonly Dictionary<(nint Object, CallbackKind Kind), GCHandle> Callbacks = new();
    private static readonly Lock CallbackLock = new();

    // Callers hold CallbackLock and free an old GCHandle only after the native setter returns.

    private static (GCHandle NewHandle, nint Userdata) PrepareCallback(Delegate? callback)
    {
        if (callback is null)
            return (default, 0);

        var handle = GCHandle.Alloc(callback);
        return (handle, GCHandle.ToIntPtr(handle));
    }

    private static void CommitCallback((nint, CallbackKind) key, GCHandle newHandle)
    {
        if (Callbacks.Remove(key, out var old))
            old.Free();

        if (newHandle.IsAllocated)
            Callbacks[key] = newHandle;
    }

    // The native setter failed and left the old registration in place.
    private static void RollbackCallback(GCHandle newHandle)
    {
        if (newHandle.IsAllocated)
            newHandle.Free();
    }

    public static void SetTrackStoppedCallback(MixTrack track, MixTrackStoppedHandler? callback)
    {
        ExceptionAggregator.CheckExceptions();
        var key = (track.Pointer.Value, CallbackKind.TrackStopped);
        lock (CallbackLock)
        {
            var (newHandle, userdata) = PrepareCallback(callback);
            delegate* unmanaged[Cdecl]<void*, MixTrack, void> fn = callback is null ? null : &TrackStoppedTrampoline;
            if (PInvoke.SetTrackStoppedCallback(track, fn, (void*)userdata) == 0)
            {
                RollbackCallback(newHandle);
                throw SdlException.FromError("MIX_SetTrackStoppedCallback failed");
            }
            CommitCallback(key, newHandle);
        }
    }

    public static void SetTrackRawCallback(MixTrack track, MixTrackMixHandler? callback)
    {
        ExceptionAggregator.CheckExceptions();
        var key = (track.Pointer.Value, CallbackKind.TrackRaw);
        lock (CallbackLock)
        {
            var (newHandle, userdata) = PrepareCallback(callback);
            delegate* unmanaged[Cdecl]<void*, MixTrack, SdlAudioSpec*, float*, int, void> fn = callback is null
                ? null
                : &TrackMixTrampoline;
            if (PInvoke.SetTrackRawCallback(track, fn, (void*)userdata) == 0)
            {
                RollbackCallback(newHandle);
                throw SdlException.FromError("MIX_SetTrackRawCallback failed");
            }
            CommitCallback(key, newHandle);
        }
    }

    public static void SetTrackCookedCallback(MixTrack track, MixTrackMixHandler? callback)
    {
        ExceptionAggregator.CheckExceptions();
        var key = (track.Pointer.Value, CallbackKind.TrackCooked);
        lock (CallbackLock)
        {
            var (newHandle, userdata) = PrepareCallback(callback);
            delegate* unmanaged[Cdecl]<void*, MixTrack, SdlAudioSpec*, float*, int, void> fn = callback is null
                ? null
                : &TrackMixTrampoline;
            if (PInvoke.SetTrackCookedCallback(track, fn, (void*)userdata) == 0)
            {
                RollbackCallback(newHandle);
                throw SdlException.FromError("MIX_SetTrackCookedCallback failed");
            }
            CommitCallback(key, newHandle);
        }
    }

    public static void SetGroupPostMixCallback(MixGroup group, MixGroupMixHandler? callback)
    {
        ExceptionAggregator.CheckExceptions();
        var key = (group.Pointer.Value, CallbackKind.GroupPostMix);
        lock (CallbackLock)
        {
            var (newHandle, userdata) = PrepareCallback(callback);
            delegate* unmanaged[Cdecl]<void*, MixGroup, SdlAudioSpec*, float*, int, void> fn = callback is null
                ? null
                : &GroupMixTrampoline;
            if (PInvoke.SetGroupPostMixCallback(group, fn, (void*)userdata) == 0)
            {
                RollbackCallback(newHandle);
                throw SdlException.FromError("MIX_SetGroupPostMixCallback failed");
            }
            CommitCallback(key, newHandle);
        }
    }

    public static void SetPostMixCallback(MixMixer mixer, MixPostMixHandler? callback)
    {
        ExceptionAggregator.CheckExceptions();
        var key = (mixer.Pointer.Value, CallbackKind.PostMix);
        lock (CallbackLock)
        {
            var (newHandle, userdata) = PrepareCallback(callback);
            delegate* unmanaged[Cdecl]<void*, MixMixer, SdlAudioSpec*, float*, int, void> fn = callback is null
                ? null
                : &PostMixTrampoline;
            if (PInvoke.SetPostMixCallback(mixer, fn, (void*)userdata) == 0)
            {
                RollbackCallback(newHandle);
                throw SdlException.FromError("MIX_SetPostMixCallback failed");
            }
            CommitCallback(key, newHandle);
        }
    }

    // Called by Quit after MIX_Quit, when no trampoline can fire.
    internal static void ClearCallbacks()
    {
        lock (CallbackLock)
        {
            foreach (var (_, handle) in Callbacks)
                handle.Free();
            Callbacks.Clear();
        }
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void TrackStoppedTrampoline(void* userdata, MixTrack track)
    {
        try
        {
            if (GCHandle.FromIntPtr((nint)userdata).Target is MixTrackStoppedHandler handler)
                handler(track);
            else
                ExceptionAggregator.Push(DeadHandleMessage("track-stopped"));
        }
        catch (Exception ex)
        {
            ExceptionAggregator.Push("An exception occurred in an SDL_mixer track-stopped callback.", ex);
        }
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void TrackMixTrampoline(void* userdata, MixTrack track, SdlAudioSpec* spec, float* pcm, int samples)
    {
        try
        {
            if (GCHandle.FromIntPtr((nint)userdata).Target is MixTrackMixHandler handler)
                handler(track, in *spec, new Span<float>(pcm, samples));
            else
                ExceptionAggregator.Push(DeadHandleMessage("track mix"));
        }
        catch (Exception ex)
        {
            ExceptionAggregator.Push("An exception occurred in an SDL_mixer track mix callback.", ex);
        }
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void GroupMixTrampoline(void* userdata, MixGroup group, SdlAudioSpec* spec, float* pcm, int samples)
    {
        try
        {
            if (GCHandle.FromIntPtr((nint)userdata).Target is MixGroupMixHandler handler)
                handler(group, in *spec, new Span<float>(pcm, samples));
            else
                ExceptionAggregator.Push(DeadHandleMessage("group post-mix"));
        }
        catch (Exception ex)
        {
            ExceptionAggregator.Push("An exception occurred in an SDL_mixer group post-mix callback.", ex);
        }
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void PostMixTrampoline(void* userdata, MixMixer mixer, SdlAudioSpec* spec, float* pcm, int samples)
    {
        try
        {
            if (GCHandle.FromIntPtr((nint)userdata).Target is MixPostMixHandler handler)
                handler(mixer, in *spec, new Span<float>(pcm, samples));
            else
                ExceptionAggregator.Push(DeadHandleMessage("post-mix"));
        }
        catch (Exception ex)
        {
            ExceptionAggregator.Push("An exception occurred in an SDL_mixer post-mix callback.", ex);
        }
    }

    private static string DeadHandleMessage(string family) =>
        $"An SDL_mixer {family} callback fired with a dead or foreign GCHandle (its target was not the "
        + "expected handler); the callback was skipped.";
}
