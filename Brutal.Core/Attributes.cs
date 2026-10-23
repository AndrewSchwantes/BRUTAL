namespace Brutal.Attributes;

/// <summary>
/// Indicates that an entity only exposes memory that has a fixed address. Marking an entity [Fixed] allows the analyser
/// to check that the entity is only used in contexts where the memory is guaranteed to be fixed, such as when it is
/// returned from a method or passed as a parameter. <para />
/// The attribute can be applied to return values, parameters, fields, properties, interfaces, classes, and structs.
/// </summary>
[AttributeUsage(
    AttributeTargets.ReturnValue
        | AttributeTargets.Parameter
        | AttributeTargets.Field
        | AttributeTargets.Property
        | AttributeTargets.Interface
        | AttributeTargets.Class
        | AttributeTargets.Struct
)]
public sealed class FixedAttribute : Attribute { }

/// <summary>
/// Acts like the [Fixed] attribute but only if the referenced type is itself fixed. Pass the <c>nameof</c> a type
/// parameter of the containing method or type to resolve its type argument, or the <c>typeof</c> a concrete type or
/// interface.
/// </summary>
[AttributeUsage(AttributeTargets.ReturnValue | AttributeTargets.Property | AttributeTargets.Parameter)]
public sealed class FixedWhenAttribute : Attribute
{
    public FixedWhenAttribute(string typeName)
    {
        TypeName = typeName;
    }

    public FixedWhenAttribute(Type type)
    {
        Type = type;
    }

    public string? TypeName { get; }

    public Type? Type { get; }
}
