namespace Brutal.VulkanApi.Abstractions;

/// <summary>
/// Classification helpers for <see cref="VkFormat"/>.
/// </summary>
public static class FormatExtensions
{
    /// <summary>
    /// True when the format has a depth component (including packed depth-stencil formats).
    /// </summary>
    public static bool IsDepthFormat(this VkFormat format) =>
        format switch
        {
            VkFormat.D16UNorm
            or VkFormat.X8D24UNormPack32
            or VkFormat.D32SFloat
            or VkFormat.D16UNormS8UInt
            or VkFormat.D24UNormS8UInt
            or VkFormat.D32SFloatS8UInt => true,
            _ => false,
        };

    /// <summary>
    /// True when the format has a stencil component (including packed depth-stencil formats).
    /// </summary>
    public static bool IsStencilFormat(this VkFormat format) =>
        format switch
        {
            VkFormat.S8UInt or VkFormat.D16UNormS8UInt or VkFormat.D24UNormS8UInt or VkFormat.D32SFloatS8UInt => true,
            _ => false,
        };

    /// <summary>
    /// True when the format has both a depth and a stencil component.
    /// </summary>
    public static bool IsDepthStencilFormat(this VkFormat format) =>
        format switch
        {
            VkFormat.D16UNormS8UInt or VkFormat.D24UNormS8UInt or VkFormat.D32SFloatS8UInt => true,
            _ => false,
        };

    /// <summary>
    /// The full aspect mask implied by the format: depth and/or stencil bits for depth-stencil
    /// formats, otherwise <see cref="VkImageAspectFlags.ColorBit"/>.
    /// </summary>
    public static VkImageAspectFlags GetAspectMask(this VkFormat format) =>
        format switch
        {
            VkFormat.D16UNormS8UInt or VkFormat.D24UNormS8UInt or VkFormat.D32SFloatS8UInt =>
                VkImageAspectFlags.DepthBit | VkImageAspectFlags.StencilBit,
            VkFormat.S8UInt => VkImageAspectFlags.StencilBit,
            VkFormat.D16UNorm or VkFormat.X8D24UNormPack32 or VkFormat.D32SFloat => VkImageAspectFlags.DepthBit,
            _ => VkImageAspectFlags.ColorBit,
        };
}
