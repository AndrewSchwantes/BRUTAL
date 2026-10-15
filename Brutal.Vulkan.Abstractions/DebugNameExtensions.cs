using System.Diagnostics;

namespace Brutal.VulkanApi.Abstractions;

public static class DebugNameExtensions
{
    private const int MAX_DEBUG_NAME_LENGTH = 256;

    /// <summary>
    /// Set a debug name for the handle. Names longer than 255 UTF-8 bytes are truncated.
    /// </summary>
    /// <param name="device">Handle to the device that will assign the name</param>
    /// <param name="handle">The Vulkan handle we want to assign a name to</param>
    /// <param name="name">The name to assign to the handle</param>
    [Conditional("DEBUG")]
    public static void SetDebugName<T>(this Device device, T handle, string name)
        where T : IVulkanHandle
    {
        if (!device.PhysicalDevice.Instance.IsExtensionEnabled(VK.EXT_DEBUG_UTILS_EXTENSION_NAME))
            return;

        Span<byte> bytes = stackalloc byte[MAX_DEBUG_NAME_LENGTH];
        // Encode as much as fits in the 255-byte budget (one byte reserved for the terminator).
        // FromUtf16 stops at a character boundary when the destination runs out, so a truncated
        // name is still valid UTF-8 rather than ending mid-character.
        System.Text.Unicode.Utf8.FromUtf16(
            name,
            bytes[..(MAX_DEBUG_NAME_LENGTH - 1)],
            out _,
            out var written,
            replaceInvalidSequences: true,
            isFinalBlock: true
        );
        bytes[written] = 0;

        // Include the terminator: the driver receives a raw char* and reads until NUL
        device.SetDebugName(handle.VkObjectType, handle.VkHandle, bytes[..(written + 1)]);
    }

    /// <summary>
    /// Set a debug name for the handle. The name does not need to be null-terminated;
    /// a terminator is appended when missing.
    /// </summary>
    /// <param name="device">Handle to the device that will assign the name</param>
    /// <param name="handle">The Vulkan handle we want to assign a name to</param>
    /// <param name="name">The name to assign to the handle</param>
    [Conditional("DEBUG")]
    public static void SetDebugName<T>(this Device device, T handle, ReadOnlySpan<byte> name)
        where T : IVulkanHandle
    {
        if (!device.PhysicalDevice.Instance.IsExtensionEnabled(VK.EXT_DEBUG_UTILS_EXTENSION_NAME))
            return;

        // The driver reads until NUL, so the bytes handed over must include one
        if (name.Length > 0 && name[^1] == 0)
        {
            device.SetDebugName(handle.VkObjectType, handle.VkHandle, name);
            return;
        }

        var length = Math.Min(name.Length, MAX_DEBUG_NAME_LENGTH - 1);
        // Don't cut a multi-byte character in half: back up over any continuation bytes (10xxxxxx)
        // so the truncated prefix is still valid UTF-8
        while (length > 0 && length < name.Length && (name[length] & 0xC0) == 0x80)
            length--;
        Span<byte> terminated = stackalloc byte[MAX_DEBUG_NAME_LENGTH];
        name[..length].CopyTo(terminated);
        terminated[length] = 0;

        device.SetDebugName(handle.VkObjectType, handle.VkHandle, terminated[..(length + 1)]);
    }

    private static unsafe void SetDebugName(
        this Device device,
        VkObjectType objectType,
        nint handle,
        ReadOnlySpan<byte> name
    )
    {
        fixed (byte* namePtr = name)
            device.SetDebugUtilsObjectNameEXT(
                new VkDebugUtilsObjectNameInfoEXT
                {
                    ObjectType = objectType,
                    ObjectHandle = handle,
                    ObjectName = namePtr,
                }
            );
    }
}
