namespace MaterialGenerator.Application.Common;

public sealed class ResourcePath
{
    public static ResourcePath ItemTexture(string id) => new($"Textures/Item/{id}.png");
    
    private ResourcePath(string value) { Value = value; }
    
    public string Value { get; }

    public override string ToString() => Value;

    public static implicit operator string(ResourcePath value) => value.Value;
    public static implicit operator ResourcePath(string value) => new(value);
}