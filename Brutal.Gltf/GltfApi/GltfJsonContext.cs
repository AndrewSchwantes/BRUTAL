using System.Text.Json;
using System.Text.Json.Serialization;

namespace Brutal.GltfApi;

/// <summary>
/// Source-generated System.Text.Json metadata for the glTF schema, so that loading and saving
/// works without reflection (trimming and Native AOT).
/// </summary>
/// <remarks>
/// The generator emits the other half of this class (<c>GeneratedSchema/GltfJsonContext.gen.cs</c>) with every
/// <see cref="JsonSerializableAttribute"/>: the document root, every extension root type, the nested enums and
/// <see cref="JsonElement"/>. Don't add <see cref="JsonSerializableAttribute"/> here; the System.Text.Json source
/// generator fails when more than one partial declaration carries it.
/// </remarks>
[JsonSourceGenerationOptions(
    // glTF has no concept of an explicit JSON null; absent optional properties must be
    // omitted entirely or strict validators reject the asset (e.g. "uri": null in GLB).
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    Converters = [
        typeof(JsonHelpers.Float3JsonConverter),
        typeof(JsonHelpers.Float4JsonConverter),
        typeof(JsonHelpers.FloatQuatJsonConverter),
        typeof(JsonHelpers.Float4X4JsonConverter),
    ]
)]
internal partial class GltfJsonContext : JsonSerializerContext;
