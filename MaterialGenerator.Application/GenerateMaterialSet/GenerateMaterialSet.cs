using MaterialGenerator.Application.Common;
using MaterialGenerator.Application.Contracts.GenerateMaterialSet;
using MaterialGenerator.Application.TextureGeneration;
using Microsoft.Extensions.Logging;
using RowanWillis.Common.Application;
using RowanWillis.Common.LanguageExtensions;
using RowanWillis.Common.LanguageExtensions.Process;
using SixLabors.ImageSharp;
using Void = RowanWillis.Common.LanguageExtensions.Void;

namespace MaterialGenerator.Application.GenerateMaterialSet;

internal sealed record GenerateMaterialSetAction
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required Color BaseColour { get; init; }
}

internal sealed class GenerateMaterialSetHandler : BaseCommandHandler<
    Contracts.GenerateMaterialSet.GenerateMaterialSet,
    GenerateMaterialSetAction,
    GenerateMaterialSetResult>
{
    public GenerateMaterialSetHandler(
        IAuthorizer<Contracts.GenerateMaterialSet.GenerateMaterialSet> authorizer,
        IValidator<Contracts.GenerateMaterialSet.GenerateMaterialSet, GenerateMaterialSetAction> validator,
        IExecutor<GenerateMaterialSetAction, GenerateMaterialSetResult> executor,
        IUnitOfWork unitOfWork, ILogger<ICommandHandler<Contracts.GenerateMaterialSet.GenerateMaterialSet, GenerateMaterialSetResult>> logger)
        : base(authorizer, validator, executor, unitOfWork, logger)
    {
    }
}

internal sealed class GenerateMaterialSetExecutor : IExecutor<GenerateMaterialSetAction, GenerateMaterialSetResult>
{
    private readonly ITextureGenerator[] TextureGenerators =
    [
        TextureGeneratorBuilder.New("ingot")
            .AddLayer(ResourcePath.ItemTexture("ingot"), options => options.BaseColor)
            .Build(),
        TextureGeneratorBuilder.New("screw")
            .AddLayer(ResourcePath.ItemTexture("screw"), options => options.BaseColor)
            .Build(),
        TextureGeneratorBuilder.New("dust")
            .AddLayer(ResourcePath.ItemTexture("dust"), options => options.BaseColor)
            .Build(),
        TextureGeneratorBuilder.New("bolt")
            .AddLayer(ResourcePath.ItemTexture("bolt"), options => options.BaseColor)
            .Build(),
        TextureGeneratorBuilder.New("rod")
            .AddLayer(ResourcePath.ItemTexture("rod"), options => options.BaseColor)
            .Build(),
        TextureGeneratorBuilder.New("plate")
            .AddLayer(ResourcePath.ItemTexture("plate"), options => options.BaseColor)
            .Build(),
        TextureGeneratorBuilder.New("foil")
            .AddLayer(ResourcePath.ItemTexture("foil"), options => options.BaseColor)
            .Build(),
        TextureGeneratorBuilder.New("gear")
            .AddLayer(ResourcePath.ItemTexture("gear"), options => options.BaseColor)
            .Build(),
        TextureGeneratorBuilder.New("gear_fine")
            .AddLayer(ResourcePath.ItemTexture("gear_fine"), options => options.BaseColor)
            .Build(),
        TextureGeneratorBuilder.New("saw")
            .AddLayer(ResourcePath.ItemTexture("saw"), options => options.BaseColor)
            .Build(),
        TextureGeneratorBuilder.New("spring")
            .AddLayer(ResourcePath.ItemTexture("spring"), options => options.BaseColor)
            .Build(),
        TextureGeneratorBuilder.New("rotor")
            .AddLayer(ResourcePath.ItemTexture("rotor"), options => options.BaseColor)
            .Build()
    ];
    
    public async Task<ProcessResult<GenerateMaterialSetResult>> Execute(GenerateMaterialSetAction action, CancellationToken cancellationToken)
    {
        var outputBaseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Output");

        if (!Directory.Exists(outputBaseDirectory))
        {
            Directory.CreateDirectory(outputBaseDirectory);
        }

        var generatedResources = new List<string>();

        foreach (var textureGenerator in TextureGenerators)
        {
            using var outputStream = new MemoryStream(); 
        
            var result = textureGenerator.Generate(
                new()
                {
                    Id = action.Id,
                    BaseColor = action.BaseColour
                });

            var outputPath = Path.Combine(outputBaseDirectory, result.ResourcePath);
            var outputDirectory = Path.GetDirectoryName(outputPath);

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            await File.WriteAllBytesAsync(outputPath, result.Data, cancellationToken);
            generatedResources.Add(result.ResourcePath);
        }
        
        return ProcessResult<GenerateMaterialSetResult>.Success(new()
        {
            GeneratedResources = generatedResources.ToArray()
        });
    }
}

internal sealed class GenerateMaterialSetValidator : IValidator<Contracts.GenerateMaterialSet.GenerateMaterialSet, GenerateMaterialSetAction>
{
    public Task<ProcessResult<GenerateMaterialSetAction>> Validate(Contracts.GenerateMaterialSet.GenerateMaterialSet input, CancellationToken cancellationToken)
    {
        var errors = new List<Error>();
        
        if (!Color.TryParseHex(input.BaseColour, out var baseColour))
        {
            errors.Add(new()
            {
                Type = "InvalidHexColour",
                Description = "Base colour is invalid"
            });
        }

        if (errors.Any())
        {
            return Task.FromResult(ProcessResult<GenerateMaterialSetAction>.ValidationFailure(errors));
        }

        return Task.FromResult(
            ProcessResult<GenerateMaterialSetAction>.Success(new()
            {
                Id = input.Id,
                Name = input.Name,
                BaseColour = baseColour,
            }));
    }
}

internal sealed class GenerateMaterialSetAuthorizer : IAuthorizer<Contracts.GenerateMaterialSet.GenerateMaterialSet>
{
    public Task<ProcessResult<Void>> Authorize(Contracts.GenerateMaterialSet.GenerateMaterialSet command, CancellationToken cancellationToken)
    {
        return Task.FromResult(ProcessResult.Success());
    }
}