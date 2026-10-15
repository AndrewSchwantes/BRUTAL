namespace Brutal.VulkanApi.Abstractions;

/// <summary>
/// Wrapper struct for a Vulkan image view
/// </summary>
public readonly struct ImageViewEx(
    Device device,
    ImageEx image,
    VkImageView imageView,
    VkImageViewType imageViewType,
    VkImageSubresourceRange subresourceRange,
    VkAllocator? allocator
) : IDisposable, INullable, IImageView, IImageSubresourceRange
{
    public readonly ImageEx Image = image;
    public readonly VkImageView VkImageView = imageView;
    public readonly VkImageViewType Type = imageViewType;
    public readonly VkImageSubresourceRange SubresourceRange = subresourceRange;

    public bool IsNull() => VkImageView.IsNull();

    VkImageView IImageView.VkImageView => VkImageView;
    VkImage IImageSubresourceRange.VkImage => Image.VkImage;
    VkImageSubresourceRange IImageSubresourceRange.SubresourceRange => SubresourceRange;

    // The covered region starts at the range's base mip, so report that level's dimensions rather
    // than mip 0's; only a 3D view spans depth, every other view type is one slice deep
    VkExtent3D IImageSubresourceRange.Extent =>
        new()
        {
            Width = Math.Max(1, Image.Extent.Width >> SubresourceRange.BaseMipLevel),
            Height = Math.Max(1, Image.Extent.Height >> SubresourceRange.BaseMipLevel),
            Depth = Type == VkImageViewType._3D ? Math.Max(1, Image.Extent.Depth >> SubresourceRange.BaseMipLevel) : 1,
        };

    /// <inheritdoc />
    public void Dispose()
    {
        device.DestroyImageView(VkImageView, allocator);
    }

    public ImageViewPartitionInfo AsPartition()
    {
        if (IsNull())
            throw new NullReferenceException(nameof(VkImageView));

        return new ImageViewPartitionInfo(
            this,
            Image.AllocationInfo.ArrayLayers,
            0,
            Image.Extent,
            new VkOffset3D(),
            null
        );
    }

    public static implicit operator ImageViewPartitionInfo(ImageViewEx inValue) => inValue.AsPartition();
}
