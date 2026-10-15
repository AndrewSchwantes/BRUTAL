namespace Brutal.GltfApi;

public partial class Image
{
    /// <summary>
    /// PNG MIME type. One of the two image formats sanctioned by core glTF 2.0.
    /// </summary>
    public const string MimeTypePng = "image/png";

    /// <summary>
    /// JPEG MIME type. One of the two image formats sanctioned by core glTF 2.0.
    /// </summary>
    public const string MimeTypeJpeg = "image/jpeg";

    /// <summary>
    /// KTX2 MIME type. Sanctioned by the KHR_texture_basisu extension; assets using it
    /// must declare that extension in extensionsUsed to be spec-conformant.
    /// </summary>
    public const string MimeTypeKtx2 = "image/ktx2";
}
