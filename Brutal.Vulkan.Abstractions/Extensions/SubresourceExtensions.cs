namespace Brutal.VulkanApi.Abstractions;

/// <summary>
/// Factories for the common <see cref="VkImageSubresourceRange"/> shapes.
/// </summary>
public static class Subresources
{
    /// <summary>
    /// All mip levels and array layers of an image, using <see cref="VK.REMAINING_MIP_LEVELS"/> /
    /// <see cref="VK.REMAINING_ARRAY_LAYERS"/>. Valid for barriers and image views; copy commands
    /// need explicit counts.
    /// </summary>
    public static VkImageSubresourceRange Full(VkImageAspectFlags aspectMask) =>
        new()
        {
            AspectMask = aspectMask,
            BaseMipLevel = 0,
            LevelCount = VK.REMAINING_MIP_LEVELS,
            BaseArrayLayer = 0,
            LayerCount = VK.REMAINING_ARRAY_LAYERS,
        };

    /// <summary>
    /// A single mip level of a single array layer - the shape of most render targets.
    /// </summary>
    public static VkImageSubresourceRange Single(VkImageAspectFlags aspectMask, int mipLevel = 0, int arrayLayer = 0) =>
        new()
        {
            AspectMask = aspectMask,
            BaseMipLevel = mipLevel,
            LevelCount = 1,
            BaseArrayLayer = arrayLayer,
            LayerCount = 1,
        };

    /// <summary>
    /// A contiguous run of mip levels across a single array layer.
    /// </summary>
    public static VkImageSubresourceRange Mips(
        VkImageAspectFlags aspectMask,
        int baseMipLevel,
        int levelCount,
        int arrayLayer = 0
    ) =>
        new()
        {
            AspectMask = aspectMask,
            BaseMipLevel = baseMipLevel,
            LevelCount = levelCount,
            BaseArrayLayer = arrayLayer,
            LayerCount = 1,
        };

    /// <summary>
    /// A contiguous run of array layers at a single mip level.
    /// </summary>
    public static VkImageSubresourceRange Layers(
        VkImageAspectFlags aspectMask,
        int baseArrayLayer,
        int layerCount,
        int mipLevel = 0
    ) =>
        new()
        {
            AspectMask = aspectMask,
            BaseMipLevel = mipLevel,
            LevelCount = 1,
            BaseArrayLayer = baseArrayLayer,
            LayerCount = layerCount,
        };
}

/// <summary>
/// Conversions between <see cref="VkImageSubresourceRange"/> and <see cref="VkImageSubresourceLayers"/>.
/// </summary>
public static class SubresourceExtensions
{
    /// <summary>
    /// Convert a subresource range to the <see cref="VkImageSubresourceLayers"/> for one of its mip levels,
    /// as consumed by copy and blit commands. The range's <c>LayerCount</c> must be explicit
    /// (not <see cref="VK.REMAINING_ARRAY_LAYERS"/>).
    /// </summary>
    /// <param name="range">The source range.</param>
    /// <param name="mipLevel">Mip level relative to the range's <c>BaseMipLevel</c>.</param>
    public static VkImageSubresourceLayers ToLayers(this VkImageSubresourceRange range, int mipLevel = 0) =>
        new()
        {
            AspectMask = range.AspectMask,
            MipLevel = range.BaseMipLevel + mipLevel,
            BaseArrayLayer = range.BaseArrayLayer,
            LayerCount = range.LayerCount,
        };
}
