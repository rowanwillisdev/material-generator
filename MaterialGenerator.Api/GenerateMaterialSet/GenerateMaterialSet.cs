namespace MaterialGenerator.Api.GenerateMaterialSet;

public sealed record GenerateMaterialSetRequest
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string BaseColour { get; init; }
}

public sealed record GenerateMaterialSetResponse
{
    public required string[] GeneratedResources { get; init; } = [];
}