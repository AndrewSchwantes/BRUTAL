namespace Brutal.VulkanApi.Abstractions;

/// <summary>
/// An allocator that can allocate both buffers and images. Systems that need to allocate
/// resources of both kinds can depend on this single interface instead of taking
/// <see cref="IBufferAllocator"/> and <see cref="IImageAllocator"/> separately.
/// </summary>
public interface IGlobalAllocator : IBufferAllocator, IImageAllocator { }
