namespace Brutal.SdlApi;

public delegate bool SdlEventWatch(ref SdlEvent ev);

public static unsafe partial class Sdl
{
    public static unsafe partial class Events
    {
        // Registered event watches, keyed by the managed delegate the caller passed. The GCHandle
        // roots the delegate for as long as SDL holds the native function pointer, and its IntPtr is
        // the userdata SDL threads back to the trampoline so it can dispatch to the right delegate.
        // SDL_RemoveEventWatch matches on (function pointer, userdata), so the same handle value must
        // be handed back on removal, hence the map. Guarded by a lock because SDL permits adding and
        // removing watches from any thread.
        private static readonly Dictionary<SdlEventWatch, GCHandle> Watches = new();
        private static readonly Lock WatchLock = new();

        public static bool PollEvent(out SdlEvent ev)
        {
            ev = default;

            bool dequeued;
            fixed (SdlEvent* p = &ev)
                dequeued = PInvoke.PollEvent(p) != 0;

            Sdl.ExceptionAggregator.CheckExceptions();
            return dequeued;
        }

        public static void AddEventWatch(SdlEventWatch callback)
        {
            ArgumentNullException.ThrowIfNull(callback);

            lock (WatchLock)
            {
                if (Watches.ContainsKey(callback))
                    return;

                var handle = GCHandle.Alloc(callback);
                if (PInvoke.AddEventWatch(&EventWatchTrampoline, (void*)GCHandle.ToIntPtr(handle)) == 0)
                {
                    handle.Free();
                    throw SdlException.FromError("SDL_AddEventWatch failed");
                }

                Watches[callback] = handle;
            }
        }

        public static void RemoveEventWatch(SdlEventWatch callback)
        {
            ArgumentNullException.ThrowIfNull(callback);

            lock (WatchLock)
            {
                if (!Watches.Remove(callback, out var handle))
                    return;

                PInvoke.RemoveEventWatch(&EventWatchTrampoline, (void*)GCHandle.ToIntPtr(handle));
                handle.Free();
            }
        }

        // Called from Sdl.Init.Quit so the managed map does not outlive SDL's own watch list.
        internal static void ClearWatches()
        {
            lock (WatchLock)
            {
                foreach (var (_, handle) in Watches)
                {
                    // Remove the native registration first (still valid pre-SDL_Quit), then release
                    // the managed root. Both are matched on (function pointer, userdata).
                    PInvoke.RemoveEventWatch(&EventWatchTrampoline, (void*)GCHandle.ToIntPtr(handle));
                    handle.Free();
                }

                Watches.Clear();
            }
        }

        // The single native entry point for every registered watch. SDL passes the GCHandle IntPtr
        // back as userdata; we resolve it to the managed delegate and invoke it inside a try/catch so
        // no exception ever escapes into native code. Failures are parked in the aggregator and
        // rethrown from the next PollEvent.
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        private static byte EventWatchTrampoline(void* userdata, SdlEvent* ev)
        {
            try
            {
                var handle = GCHandle.FromIntPtr((nint)userdata);
                if (handle.Target is SdlEventWatch filter)
                    return filter(ref *ev) ? (byte)1 : (byte)0;

                Sdl.ExceptionAggregator.Push(
                    "An SDL event watch fired with a dead or foreign GCHandle (its target was not an "
                        + "SdlEventWatch); the callback was skipped."
                );
            }
            catch (Exception ex)
            {
                Sdl.ExceptionAggregator.Push("An exception occurred in an SDL event watch callback.", ex);
            }

            // Event watches ignore the return value; permit by default.
            return 1;
        }
    }
}
