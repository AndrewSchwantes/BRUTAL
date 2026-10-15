namespace Brutal.VulkanApi.Abstractions;

/// <summary>
/// Structure to hold synchronization objects for the Renderer
/// </summary>
public class SyncObjects(Device inDevice) : IDisposable
{
    /// <summary>
    /// Semaphore signalled when the acquired swapchain image is ready to be rendered to.
    /// </summary>
    public required VkSemaphore ImageReadySemaphore;

    /// <summary>
    /// Semaphore signalled when rendering has finished and the image can be presented.
    /// </summary>
    public required VkSemaphore RenderingFinishedSemaphore;

    /// <summary>
    /// Signaled Fence
    /// </summary>
    public required VkFence Fence;

    /// <summary>
    /// A CommandBuffer from the Graphics pool which can be used for rendering commands.
    /// </summary>
    public required CommandBuffer CommandBuffer;

    /// <summary>
    /// The command pool that <see cref="CommandBuffer"/> was allocated from.
    /// When set, <see cref="Dispose"/> frees the command buffer back to this pool; when left
    /// default, the caller retains ownership of the command buffer and must free it themselves
    /// (otherwise every rebuild permanently grows the pool).
    /// </summary>
    public VkCommandPool CommandPool = default;

    /// <summary>
    /// The allocator the sync objects were created with
    /// </summary>
    public required VkAllocator? Allocator;

    public void Dispose()
    {
        if (!CommandPool.IsNull())
            inDevice.FreeCommandBuffers(CommandPool, [CommandBuffer]);

        inDevice.DestroyFence(Fence, Allocator);
        inDevice.DestroySemaphore(ImageReadySemaphore, Allocator);
        inDevice.DestroySemaphore(RenderingFinishedSemaphore, Allocator);
    }
}
