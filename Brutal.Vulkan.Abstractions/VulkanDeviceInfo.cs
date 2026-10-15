namespace Brutal.VulkanApi.Abstractions;

/// <summary>
/// Formats <see cref="VkPhysicalDeviceProperties"/> fields into human-readable strings,
/// for logs, benchmark metadata, and device pickers.
/// </summary>
public static class VulkanDeviceInfo
{
    private const int VENDOR_ID_NVIDIA = 0x10DE;
    private const int VENDOR_ID_INTEL = 0x8086;

    /// <summary>
    /// The device name as a managed string.
    /// </summary>
    public static unsafe string GetDeviceName(in VkPhysicalDeviceProperties properties)
    {
        fixed (byte* name = properties.DeviceName)
            return Utf8.GetString(Utf8.GetNullTerminatedSpan(name));
    }

    /// <summary>
    /// The device name as a managed string.
    /// </summary>
    public static string GetDeviceName(this PhysicalDevice physicalDevice)
    {
        var properties = physicalDevice.GetProperties();
        return GetDeviceName(in properties);
    }

    /// <summary>
    /// Format a Vulkan-packed API version ("major.minor.patch", with a "variant:" prefix for
    /// non-zero variants).
    /// </summary>
    public static string FormatApiVersion(int version)
    {
        var value = unchecked((uint)version);
        var variant = value >> 29;
        var major = (value >> 22) & 0x7F;
        var minor = (value >> 12) & 0x3FF;
        var patch = value & 0xFFF;

        return variant == 0 ? $"{major}.{minor}.{patch}" : $"{variant}:{major}.{minor}.{patch}";
    }

    /// <summary>
    /// Format a driver version using the vendor's packing where it is known
    /// (NVIDIA and Intel-on-Windows deviate from the Vulkan convention).
    /// </summary>
    public static string FormatDriverVersion(int vendorId, int version)
    {
        var value = unchecked((uint)version);

        switch (vendorId)
        {
            case VENDOR_ID_NVIDIA:
                return $"{(value >> 22) & 0x3FF}.{(value >> 14) & 0xFF}.{(value >> 6) & 0xFF}.{value & 0x3F}";
            case VENDOR_ID_INTEL when OperatingSystem.IsWindows():
                return $"{value >> 14}.{value & 0x3FFF}";
            default:
                return FormatApiVersion(version);
        }
    }

    /// <summary>
    /// Format a driver version from the device's own properties.
    /// </summary>
    public static string FormatDriverVersion(in VkPhysicalDeviceProperties properties)
    {
        return FormatDriverVersion(properties.VendorID, properties.DriverVersion);
    }
}
