using System.Runtime.InteropServices;

namespace Brutal.JoltApi;

// Hand-written LibraryImports for the contact event listener (JoltContactEventsShim.cpp).
// These are fixed-shape, not derived from the parse.
public static partial class Jolt
{
    public static unsafe partial class PInvoke
    {
        /// <summary>
        ///   <para>
        ///     Creates a buffering contact listener holding up to capacity events of the kinds in mask.
        ///   </para>
        /// </summary>
        [LibraryImport("jolt", EntryPoint = "Jolt_ContactEventListenerCreate")]
        public static partial nint ContactEventListenerCreate(uint capacity, uint mask);

        /// <summary>
        ///   <para>
        ///     Frees a buffering contact listener. It must not be installed on a physics system.
        ///   </para>
        /// </summary>
        [LibraryImport("jolt", EntryPoint = "Jolt_ContactEventListenerDestroy")]
        public static partial void ContactEventListenerDestroy(nint listener);

        /// <summary>
        ///   <para>
        ///     The listener as the ContactListener PhysicsSystem.SetContactListener takes; borrowed from it.
        ///   </para>
        /// </summary>
        [LibraryImport("jolt", EntryPoint = "Jolt_ContactEventListenerGetBase")]
        public static partial ContactListener ContactEventListenerGetBase(nint listener);

        /// <summary>
        ///   <para>
        ///     Copies up to capacity pending events into events, oldest first, and returns how many were copied.
        ///   </para>
        /// </summary>
        [LibraryImport("jolt", EntryPoint = "Jolt_ContactEventListenerDrain")]
        public static partial uint ContactEventListenerDrain(
            nint listener,
            ContactEvent* events,
            uint capacity,
            uint* remaining
        );

        /// <summary>
        ///   <para>
        ///     Throws away every pending event.
        ///   </para>
        /// </summary>
        [LibraryImport("jolt", EntryPoint = "Jolt_ContactEventListenerClear")]
        public static partial void ContactEventListenerClear(nint listener);

        /// <summary>
        ///   <para>
        ///     How many events are waiting to be drained.
        ///   </para>
        /// </summary>
        [LibraryImport("jolt", EntryPoint = "Jolt_ContactEventListenerGetPending")]
        public static partial uint ContactEventListenerGetPending(nint listener);

        /// <summary>
        ///   <para>
        ///     How many events were dropped over the listener's life because the buffer was full.
        ///   </para>
        /// </summary>
        [LibraryImport("jolt", EntryPoint = "Jolt_ContactEventListenerGetDropped")]
        public static partial uint ContactEventListenerGetDropped(nint listener);

        /// <summary>
        ///   <para>
        ///     How many events the buffer holds.
        ///   </para>
        /// </summary>
        [LibraryImport("jolt", EntryPoint = "Jolt_ContactEventListenerGetCapacity")]
        public static partial uint ContactEventListenerGetCapacity(nint listener);
    }
}
