using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace Brutal.GltfApi;

public interface IExtensionGltfObject
{
    public Dictionary<string, object>? Extensions { get; }
}

public static class GltfExtensions
{
    /// <summary>
    /// Gets an extension object by name.
    /// </summary>
    /// <remarks>
    /// Under trimming or Native AOT, <typeparamref name="T"/> must be one of the extension types this library
    /// defines; for your own extension types use the overload that takes a <see cref="JsonTypeInfo{T}"/>.
    /// </remarks>
    public static T? GetExtension<T>(this IExtensionGltfObject obj, string extensionName)
        where T : class
    {
        return GetExtensionCore<T>(obj, extensionName, jsonTypeInfo: null);
    }

    /// <summary>
    /// Gets an extension object by name, reading it with the given source-generated metadata.
    /// </summary>
    public static T? GetExtension<T>(this IExtensionGltfObject obj, string extensionName, JsonTypeInfo<T> jsonTypeInfo)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(jsonTypeInfo);
        return GetExtensionCore(obj, extensionName, jsonTypeInfo);
    }

    private static T? GetExtensionCore<T>(IExtensionGltfObject obj, string extensionName, JsonTypeInfo<T>? jsonTypeInfo)
        where T : class
    {
        var extensionMap = obj.Extensions;
        if (extensionMap == null || !extensionMap.TryGetValue(extensionName, out var extObj))
            return null;
        // Extensions assigned programmatically hold the typed object directly; extensions coming
        // from deserialization hold the raw JsonElement.
        if (extObj is T typed)
            return typed;
        // Metadata is resolved only here: for an unregistered T it throws under Native AOT, and in JIT builds
        // for types System.Text.Json can't describe, which must not break lookups that never deserialize.
        if (extObj is JsonElement je)
            return je.Deserialize(jsonTypeInfo ?? (JsonTypeInfo<T>)GltfUtility.JsonOptions.GetTypeInfo(typeof(T)));
        throw new InvalidCastException(
            $"Extension '{extensionName}' holds a {extObj.GetType()}, expected {typeof(T)} or JsonElement"
        );
    }
}
