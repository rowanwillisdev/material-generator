using RowanWillis.Common.Application;

namespace MaterialGenerator.Application.Contracts.GenerateMaterialSet;

public sealed record GenerateMaterialSet : ICommand<GenerateMaterialSetResult>
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string BaseColour { get; init; }
}

public sealed record GenerateMaterialSetResult
{
    public required string[] GeneratedResources { get; init; } = [];
}