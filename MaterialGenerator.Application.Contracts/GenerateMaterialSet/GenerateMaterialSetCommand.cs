using RowanWillis.Common.Application;

namespace MaterialGenerator.Application.Contracts.GenerateMaterialSet;

public sealed record GenerateMaterialSetCommand : ICommand<GenerateMaterialSetResult>
{
    public required string Name { get; init; }
    public required string BaseColour { get; init; }
}

public sealed record GenerateMaterialSetResult;