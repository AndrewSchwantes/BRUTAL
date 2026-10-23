namespace Brutal.VulkanApi;

public partial class Vulkan
{
    // The dll name we expect LibraryImport functions calling Vulkan to use
    private const string PInvokeDllName = "vulkan";

    // Cached reference to the loaded Vulkan dll
    private static readonly nint Dll;

    /// <summary>
    /// The function pointer to vkGetInstanceProcAddr, loaded from the Vulkan library.
    /// </summary>
    public static VkGetInstanceProcAddr GetInstanceProcAddr { get; }

    static Vulkan()
    {
        // Load the correct Vulkan library for the platform
        Dll = LoadVulkanLibrary();

        // Assign the native library to the assembly resolver
        NativeLibrary.SetDllImportResolver(
            typeof(Vulkan).Assembly,
            (libraryName, _, _) => libraryName == PInvokeDllName ? Dll : nint.Zero
        );

        // Load the vkGetInstanceProcAddr function
        unsafe
        {
            GetInstanceProcAddr = new VkGetInstanceProcAddr(
                (delegate* unmanaged[Cdecl]<VkInstance, byte*, PfnVkVoidFunction>)
                    NativeLibrary.GetExport(Dll, "vkGetInstanceProcAddr")
            );
            if (GetInstanceProcAddr.Invoke == null)
                throw new NullReferenceException("Failed to load vkGetInstanceProcAddr from Vulkan library");
        }
    }

    private static nint LoadVulkanLibrary()
    {
        var assembly = typeof(Vulkan).Assembly;

        if (OperatingSystem.IsWindows())
            return NativeLibrary.Load("vulkan-1", assembly, DllImportSearchPath.System32);

        // Prefer the system Vulkan loader, falling back to the old names
        string[] candidates = [];

        // TODO: Package the Vulkan loader on macOS so it works without Homebrew or the SDK installed
        if (OperatingSystem.IsMacOS())
            candidates =
            [
                "libvulkan.1.dylib",
                "/opt/homebrew/lib/libvulkan.1.dylib",
                "/usr/local/lib/libvulkan.1.dylib",
                "VulkanEx",
            ];

        // libvulkan.so (what "vulkan" resolves to) only ships in -dev packages
        if (OperatingSystem.IsLinux())
            candidates = ["libvulkan.so.1", "vulkan"];

        foreach (var candidate in candidates)
        {
            if (NativeLibrary.TryLoad(candidate, assembly, DllImportSearchPath.LegacyBehavior, out var dll))
                return dll;
        }

        throw new DllNotFoundException($"Failed to load Vulkan library, tried: {string.Join(", ", candidates)}");
    }
}
