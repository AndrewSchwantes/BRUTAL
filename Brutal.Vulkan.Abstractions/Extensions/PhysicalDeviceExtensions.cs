using System.Numerics;

namespace Brutal.VulkanApi.Abstractions;

public static class PhysicalDeviceExtensions
{
    public static DeviceEx CreateDevice(this PhysicalDevice physicalDevice, DeviceEx.CreateInfo createInfo)
    {
        return new DeviceEx(physicalDevice, createInfo);
    }

    /// <summary>
    /// Return the first format from <paramref name="candidates"/> that supports all of
    /// <paramref name="features"/> under the given tiling, or <see cref="VkFormat.Undefined"/>
    /// when none qualifies. Typical use: picking a supported depth(-stencil) format.
    /// </summary>
    public static VkFormat FindSupportedFormat(
        this PhysicalDevice physicalDevice,
        ReadOnlySpan<VkFormat> candidates,
        VkImageTiling tiling,
        VkFormatFeatureFlags features
    )
    {
        foreach (var format in candidates)
        {
            var properties = physicalDevice.GetFormatProperties(format);

            var supported = tiling switch
            {
                VkImageTiling.Optimal => properties.OptimalTilingFeatures,
                VkImageTiling.Linear => properties.LinearTilingFeatures,
                _ => VkFormatFeatureFlags.None,
            };

            if ((supported & features) == features)
                return format;
        }

        return VkFormat.Undefined;
    }

    /// <summary>
    /// Find the queue family that has all of <paramref name="requiredFlags"/>, none of
    /// <paramref name="excludedFlags"/>, and the fewest extra capabilities - so a dedicated
    /// family (e.g. transfer-only, or compute without graphics) wins over the do-everything
    /// family when one exists. Returns -1 when no family qualifies.
    /// </summary>
    public static int FindQueueFamily(
        this PhysicalDevice physicalDevice,
        VkQueueFlags requiredFlags,
        VkQueueFlags excludedFlags = VkQueueFlags.None
    )
    {
        using var properties = SpanOwner<VkQueueFamilyProperties>.Allocate(
            physicalDevice.GetQueueFamilyProperties_Count()
        );
        physicalDevice.GetQueueFamilyProperties_Write(properties.Span);

        var bestFamily = -1;
        var bestExtraFlags = int.MaxValue;

        for (var i = 0; i < properties.Length; ++i)
        {
            var flags = properties.Span[i].QueueFlags;

            if ((flags & requiredFlags) != requiredFlags)
                continue;
            if ((flags & excludedFlags) != 0)
                continue;

            var extraFlags = BitOperations.PopCount(unchecked((uint)(int)(flags & ~requiredFlags)));
            if (extraFlags >= bestExtraFlags)
                continue;

            bestFamily = i;
            bestExtraFlags = extraFlags;
        }

        return bestFamily;
    }
}
