using System.Text.Json;

namespace Brutal.GltfApi;

public interface IExtensionGltfObject
{
    public Dictionary<string, object>? Extensions { get; }
}

public static class GltfExtensions
{
    public static T? GetExtension<T>(this IExtensionGltfObject obj, string extensionName)
        where T : class
    {
        var extensionMap = obj.Extensions;
        if (extensionMap == null || !extensionMap.TryGetValue(extensionName, out var extObj))
            return null;
        // Extensions assigned programmatically hold the typed object directly; extensions coming
        // from deserialization hold the raw JsonElement.
        if (extObj is T typed)
            return typed;
        if (extObj is JsonElement je)
            return je.Deserialize<T>(GltfUtility.JsonOptions);
        throw new InvalidCastException(
            $"Extension '{extensionName}' holds a {extObj.GetType()}, expected {typeof(T)} or JsonElement"
        );
    }
}
