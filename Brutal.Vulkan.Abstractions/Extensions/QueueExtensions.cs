namespace Brutal.VulkanApi.Abstractions;

/// <summary>
/// Provides extension methods for submitting command buffers and handling synchronization for Vulkan queues.
/// </summary>
public static class QueueExtensions
{
    /// <summary>
    /// Submits command buffers to a Vulkan queue with specified wait and signal semaphores, and a fence for synchronization.
    /// </summary>
    /// <param name="queue">The Vulkan queue to submit command buffers to.</param>
    /// <param name="waitSemaphores">A span of semaphores to wait on before executing the command buffers.</param>
    /// <param name="waitDstStageMask">A span of pipeline stage flags indicating the stages to wait for.</param>
    /// <param name="commandBuffers">A span of command buffers to execute.</param>
    /// <param name="signalSemaphores">A span of semaphores to signal after executing the command buffers.</param>
    /// <param name="fence">A fence that is signaled when the queue finishes executing the submitted command buffers.</param>
    public static void Submit(
        this Queue queue,
        Span<VkSemaphore> waitSemaphores,
        Span<VkPipelineStageFlags> waitDstStageMask,
        Span<CommandBuffer> commandBuffers,
        Span<VkSemaphore> signalSemaphores,
        VkFence fence
    )
    {
        // The implementation reads WaitSemaphoreCount stage masks, so a shorter span is an out-of-bounds read
        if (waitDstStageMask.Length != waitSemaphores.Length)
            throw new ArgumentException(
                "waitDstStageMask must be the same length as waitSemaphores",
                nameof(waitDstStageMask)
            );

        unsafe
        {
            var commandBuffersPtr = stackalloc VkCommandBuffer[commandBuffers.Length];
            for (var i = 0; i < commandBuffers.Length; i++)
                commandBuffersPtr[i] = commandBuffers[i].Handle;

            fixed (VkSemaphore* waitSemaphoresPtr = waitSemaphores)
            fixed (VkPipelineStageFlags* waitDstStageMaskPtr = waitDstStageMask)
            fixed (VkSemaphore* signalSemaphoresPtr = signalSemaphores)
            {
                var submitInfo = new VkSubmitInfo
                {
                    WaitSemaphoreCount = waitSemaphores.Length,
                    WaitSemaphores = waitSemaphoresPtr,
                    WaitDstStageMask = waitDstStageMaskPtr,
                    CommandBufferCount = commandBuffers.Length,
                    CommandBuffers = commandBuffersPtr,
                    SignalSemaphoreCount = signalSemaphores.Length,
                    SignalSemaphores = signalSemaphoresPtr,
                };

                queue.Submit([submitInfo], fence);
            }
        }
    }

    /// <summary>
    /// Submits command buffers with timeline-semaphore wait/signal values, chaining a
    /// <see href="https://registry.khronos.org/vulkan/specs/1.3-extensions/man/html/VkTimelineSemaphoreSubmitInfo.html">VkTimelineSemaphoreSubmitInfo</see>
    /// onto the submit. The value spans must be the same length as their semaphore spans;
    /// entries for binary semaphores are ignored by the implementation, so leave them zero.
    /// </summary>
    /// <param name="queue">The Vulkan queue to submit command buffers to.</param>
    /// <param name="waitSemaphores">A span of semaphores to wait on before executing the command buffers.</param>
    /// <param name="waitDstStageMask">A span of pipeline stage flags indicating the stages to wait for.</param>
    /// <param name="waitSemaphoreValues">Timeline values to wait for, one per wait semaphore.</param>
    /// <param name="commandBuffers">A span of command buffers to execute.</param>
    /// <param name="signalSemaphores">A span of semaphores to signal after executing the command buffers.</param>
    /// <param name="signalSemaphoreValues">Timeline values to signal, one per signal semaphore.</param>
    /// <param name="fence">A fence that is signaled when the queue finishes executing the submitted command buffers.</param>
    public static void Submit(
        this Queue queue,
        Span<VkSemaphore> waitSemaphores,
        Span<VkPipelineStageFlags> waitDstStageMask,
        ReadOnlySpan<ulong> waitSemaphoreValues,
        Span<CommandBuffer> commandBuffers,
        Span<VkSemaphore> signalSemaphores,
        ReadOnlySpan<ulong> signalSemaphoreValues,
        VkFence fence
    )
    {
        // The implementation reads WaitSemaphoreCount stage masks, so a shorter span is an out-of-bounds read
        if (waitDstStageMask.Length != waitSemaphores.Length)
            throw new ArgumentException(
                "waitDstStageMask must be the same length as waitSemaphores",
                nameof(waitDstStageMask)
            );
        if (waitSemaphoreValues.Length != waitSemaphores.Length)
            throw new ArgumentException(
                "waitSemaphoreValues must be the same length as waitSemaphores",
                nameof(waitSemaphoreValues)
            );
        if (signalSemaphoreValues.Length != signalSemaphores.Length)
            throw new ArgumentException(
                "signalSemaphoreValues must be the same length as signalSemaphores",
                nameof(signalSemaphoreValues)
            );

        unsafe
        {
            var commandBuffersPtr = stackalloc VkCommandBuffer[commandBuffers.Length];
            for (var i = 0; i < commandBuffers.Length; i++)
                commandBuffersPtr[i] = commandBuffers[i].Handle;

            fixed (VkSemaphore* waitSemaphoresPtr = waitSemaphores)
            fixed (VkPipelineStageFlags* waitDstStageMaskPtr = waitDstStageMask)
            fixed (VkSemaphore* signalSemaphoresPtr = signalSemaphores)
            fixed (ulong* waitValuesPtr = waitSemaphoreValues)
            fixed (ulong* signalValuesPtr = signalSemaphoreValues)
            {
                var timelineInfo = new VkTimelineSemaphoreSubmitInfo
                {
                    WaitSemaphoreValueCount = waitSemaphoreValues.Length,
                    WaitSemaphoreValues = (nint*)waitValuesPtr,
                    SignalSemaphoreValueCount = signalSemaphoreValues.Length,
                    SignalSemaphoreValues = (nint*)signalValuesPtr,
                };

                var submitInfo = new VkSubmitInfo
                {
                    Next = &timelineInfo,
                    WaitSemaphoreCount = waitSemaphores.Length,
                    WaitSemaphores = waitSemaphoresPtr,
                    WaitDstStageMask = waitDstStageMaskPtr,
                    CommandBufferCount = commandBuffers.Length,
                    CommandBuffers = commandBuffersPtr,
                    SignalSemaphoreCount = signalSemaphores.Length,
                    SignalSemaphores = signalSemaphoresPtr,
                };

                queue.Submit([submitInfo], fence);
            }
        }
    }
}
