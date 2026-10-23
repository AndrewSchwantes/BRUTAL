namespace Brutal.VulkanApi
{
    public static partial class Vulkan
    {
        /// <summary>
        ///     <see href="https://registry.khronos.org/vulkan/specs/1.3-extensions/man/html/VK_MAKE_VERSION.html">VK_MAKE_VERSION</see> - Construct an API version number
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         <c>VK_MAKE_VERSION</c> constructs an API version number.
        ///     </para>
        ///     <code>
        ///         // Provided by VK_VERSION_1_0
        ///         #define VK_MAKE_VERSION(major, minor, patch) \
        ///         ((((uint32_t)(major)) &lt;&lt; 22U) | (((uint32_t)(minor)) &lt;&lt; 12U) | ((uint32_t)(patch)))
        ///     </code>
        /// </remarks>
        /// <param name="major">is the major version number.</param>
        /// <param name="minor">is the minor version number.</param>
        /// <param name="patch">is the patch version number.</param>
        public static int MakeVersion(int major, int minor, int patch) =>
            unchecked((int)((((uint)(major)) << 22) | (((uint)(minor)) << 12) | ((uint)(patch))));

        /// <summary>
        ///     <see href="https://registry.khronos.org/vulkan/specs/1.3-extensions/man/html/VK_VERSION_MAJOR.html">VK_VERSION_MAJOR</see> - Extract API major version number
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         <c>VK_VERSION_MAJOR</c> extracts the API major version number from a packed
        ///         version number:
        ///     </para>
        ///     <code>
        ///         // Provided by VK_VERSION_1_0
        ///         #define VK_VERSION_MAJOR(version) ((uint32_t)(version) &gt;&gt; 22U)
        ///     </code>
        /// </remarks>
        public static int VersionMajor(int version) => unchecked((int)((uint)(version) >> 22));

        /// <summary>
        ///     <see href="https://registry.khronos.org/vulkan/specs/1.3-extensions/man/html/VK_VERSION_MINOR.html">VK_VERSION_MINOR</see> - Extract API minor version number
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         <c>VK_VERSION_MINOR</c> extracts the API minor version number from a packed
        ///         version number:
        ///     </para>
        ///     <code>
        ///         // Provided by VK_VERSION_1_0
        ///         #define VK_VERSION_MINOR(version) (((uint32_t)(version) &gt;&gt; 12U) &amp; 0x3FFU)
        ///     </code>
        /// </remarks>
        public static int VersionMinor(int version) => unchecked((int)(((uint)(version) >> 12) & 0x3FF));

        /// <summary>
        ///     <see href="https://registry.khronos.org/vulkan/specs/1.3-extensions/man/html/VK_VERSION_PATCH.html">VK_VERSION_PATCH</see> - Extract API patch version number
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         <c>VK_VERSION_PATCH</c> extracts the API patch version number from a packed
        ///         version number:
        ///     </para>
        ///     <code>
        ///         // Provided by VK_VERSION_1_0
        ///         #define VK_VERSION_PATCH(version) ((uint32_t)(version) &amp; 0xFFFU)
        ///     </code>
        /// </remarks>
        public static int VersionPatch(int version) => unchecked((int)((uint)(version) & 0xFFF));

        /// <summary>
        ///     <see href="https://registry.khronos.org/vulkan/specs/1.3-extensions/man/html/VK_MAKE_API_VERSION.html">VK_MAKE_API_VERSION</see> - Construct an API version number
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         <c>VK_MAKE_API_VERSION</c> constructs an API version number.
        ///     </para>
        ///     <code>
        ///         // Provided by VK_VERSION_1_0
        ///         #define VK_MAKE_API_VERSION(variant, major, minor, patch) \
        ///         ((((uint32_t)(variant)) &lt;&lt; 29U) | (((uint32_t)(major)) &lt;&lt; 22U) | (((uint32_t)(minor)) &lt;&lt; 12U) | ((uint32_t)(patch)))
        ///     </code>
        /// </remarks>
        /// <param name="variant">is the variant number.</param>
        /// <param name="major">is the major version number.</param>
        /// <param name="minor">is the minor version number.</param>
        /// <param name="patch">is the patch version number.</param>
        public static int MakeApiVersion(int variant, int major, int minor, int patch) =>
            unchecked(
                (int)((((uint)(variant)) << 29) | (((uint)(major)) << 22) | (((uint)(minor)) << 12) | ((uint)(patch)))
            );

        /// <summary>
        ///     <see href="https://registry.khronos.org/vulkan/specs/1.3-extensions/man/html/VK_API_VERSION_VARIANT.html">VK_API_VERSION_VARIANT</see> - Extract API variant number
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         <c>VK_API_VERSION_VARIANT</c> extracts the API variant number from a packed
        ///         version number:
        ///     </para>
        ///     <code>
        ///         // Provided by VK_VERSION_1_0
        ///         #define VK_API_VERSION_VARIANT(version) ((uint32_t)(version) &gt;&gt; 29U)
        ///     </code>
        /// </remarks>
        public static int ApiVersionVariant(int version) => unchecked((int)((uint)(version) >> 29));

        /// <summary>
        ///     <see href="https://registry.khronos.org/vulkan/specs/1.3-extensions/man/html/VK_API_VERSION_MAJOR.html">VK_API_VERSION_MAJOR</see> - Extract API major version number
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         <c>VK_API_VERSION_MAJOR</c> extracts the API major version number from a
        ///         packed version number:
        ///     </para>
        ///     <code>
        ///         // Provided by VK_VERSION_1_0
        ///         #define VK_API_VERSION_MAJOR(version) (((uint32_t)(version) &gt;&gt; 22U) &amp; 0x7FU)
        ///     </code>
        /// </remarks>
        public static int ApiVersionMajor(int version) => unchecked((int)(((uint)(version) >> 22) & 0x7F));

        /// <summary>
        ///     <see href="https://registry.khronos.org/vulkan/specs/1.3-extensions/man/html/VK_API_VERSION_MINOR.html">VK_API_VERSION_MINOR</see> - Extract API minor version number
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         <c>VK_API_VERSION_MINOR</c> extracts the API minor version number from a
        ///         packed version number:
        ///     </para>
        ///     <code>
        ///         // Provided by VK_VERSION_1_0
        ///         #define VK_API_VERSION_MINOR(version) (((uint32_t)(version) &gt;&gt; 12U) &amp; 0x3FFU)
        ///     </code>
        /// </remarks>
        public static int ApiVersionMinor(int version) => unchecked((int)(((uint)(version) >> 12) & 0x3FF));

        /// <summary>
        ///     <see href="https://registry.khronos.org/vulkan/specs/1.3-extensions/man/html/VK_API_VERSION_PATCH.html">VK_API_VERSION_PATCH</see> - Extract API patch version number
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         <c>VK_API_VERSION_PATCH</c> extracts the API patch version number from a
        ///         packed version number:
        ///     </para>
        ///     <code>
        ///         // Provided by VK_VERSION_1_0
        ///         #define VK_API_VERSION_PATCH(version) ((uint32_t)(version) &amp; 0xFFFU)
        ///     </code>
        /// </remarks>
        public static int ApiVersionPatch(int version) => unchecked((int)((uint)(version) & 0xFFF));

        /// <summary>
        ///     VK_MAKE_VIDEO_STD_VERSION
        /// </summary>
        public static int MakeVideoStdVersion(int major, int minor, int patch) =>
            unchecked((int)((((uint)(major)) << 22) | (((uint)(minor)) << 12) | ((uint)(patch))));
    }
}
