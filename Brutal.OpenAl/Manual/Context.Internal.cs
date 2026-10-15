using System.Collections.Concurrent;

namespace Brutal.OpenAlApi;

public sealed partial class Context
{
    // Weak so an abandoned wrapper is never kept alive; leaked entries are pruned on registration.
    private static readonly ConcurrentDictionary<AlcContext.Handle, WeakReference<Context>> LiveContexts = new();

    internal static List<Context> EvictDeviceEntries(Device device)
    {
        List<Context> orphans = [];

        foreach (var entry in LiveContexts)
        {
            if (!entry.Value.TryGetTarget(out var context))
            {
                LiveContexts.TryRemove(entry);
                Al.EvictContextRoots(entry.Key.Value);
            }
            else if (ReferenceEquals(context.Device, device))
            {
                LiveContexts.TryRemove(entry);
                Al.EvictContextRoots(entry.Key.Value);
                orphans.Add(context);
            }
        }

        return orphans;
    }

    internal static void MarkOrphaned(List<Context> orphans)
    {
        foreach (var context in orphans)
            context.Handle = AlcContext.Null;
    }
}
