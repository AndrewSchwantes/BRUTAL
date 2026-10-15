namespace Brutal.VulkanApi.Abstractions;

/// <summary>
/// Anything that exposes a Vulkan image view. Systems that hold a binding by interface
/// (rather than a raw <see cref="VkImageView"/>) can re-resolve the view after the
/// underlying resource is recreated, e.g. on a resize.
/// </summary>
public interface IImageView
{
    /// <summary>
    /// The current image view handle.
    /// </summary>
    VkImageView VkImageView { get; }
}

/// <summary>
/// Anything that exposes an image together with a subresource range and extent -
/// the shape that barrier, copy, and staging helpers operate on.
/// </summary>
public interface IImageSubresourceRange
{
    /// <summary>
    /// The image handle.
    /// </summary>
    VkImage VkImage { get; }

    /// <summary>
    /// The subresource range this object covers.
    /// </summary>
    VkImageSubresourceRange SubresourceRange { get; }

    /// <summary>
    /// The extent of the covered region at the range's base mip level (mip 0 of the image
    /// shifted down by <c>BaseMipLevel</c>, clamped to 1).
    /// </summary>
    VkExtent3D Extent { get; }
}
