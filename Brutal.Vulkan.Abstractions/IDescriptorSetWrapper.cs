namespace Brutal.VulkanApi.Abstractions;

/// <summary>
/// Anything that owns a descriptor set layout.
/// </summary>
public interface IDescriptorSetLayoutWrapper
{
    /// <summary>
    /// The descriptor set layout handle.
    /// </summary>
    VkDescriptorSetLayout DescriptorSetLayout { get; }
}

/// <summary>
/// Anything that owns a descriptor set (and its layout). Lets command recording bind any
/// subsystem's set uniformly via
/// <see cref="DeviceExtensions.BindDescriptorSet(CommandBuffer, VkPipelineBindPoint, VkPipelineLayout, int, IDescriptorSetWrapper)"/>.
/// </summary>
public interface IDescriptorSetWrapper : IDescriptorSetLayoutWrapper
{
    /// <summary>
    /// The descriptor set handle.
    /// </summary>
    VkDescriptorSet DescriptorSet { get; }

    /// <summary>
    /// Dynamic offsets to bind with the set. Empty by default; implement for sets that
    /// contain dynamic uniform or storage buffers. Read-only by design - Vulkan only reads
    /// the offsets (<c>pDynamicOffsets</c> is a const pointer), and the owner updates its
    /// own backing storage directly rather than through this view.
    /// </summary>
    ReadOnlySpan<ByteSize32> DynamicOffsets => [];
}
