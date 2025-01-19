using MaterialGenerator.Application.Common;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace MaterialGenerator.Application.TextureGeneration;

public sealed record TextureOptions
{
    public required string Id { get; init; }
    public required Color BaseColor { get; init; }
}

public sealed record TextureResult
{
    public required ResourcePath ResourcePath { get; init; }
    public required byte[] Data { get; init; }
}

public interface ITextureGenerator
{
    TextureResult Generate(TextureOptions options);
}

internal sealed record TextureLayer
{
    public required string BaseTextureResourceId { get; init; }
    public required Func<TextureOptions, Color> GetColour { get; init; }
}

internal sealed class TextureGenerator : ITextureGenerator
{
    public TextureGenerator(
        string idSuffix,
        TextureLayer[] layers)
    {
        IdSuffix = idSuffix;
        Layers = layers;
    }

    private readonly string IdSuffix;
    private readonly TextureLayer[] Layers;
    
    public TextureResult Generate(TextureOptions options)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(IdSuffix);
        
        if (!Layers.Any())
        {
            throw new InvalidOperationException("No texture layers have been configured.");
        }
        
        var outputStream = new MemoryStream();

        // TODO: support multiple layers
        var layer = Layers.First();
        var blendRatio = .5f;
        
        var baseImage = Image.Load<Rgba32>(File.OpenRead(Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, $"Resources/{layer.BaseTextureResourceId}")));
        var image = baseImage.Clone<Rgba32>();
        
        var tintRgba = layer.GetColour(options).WithAlpha(0.5f).ToPixel<RgbaVector>();

        var tintMatrix = new ColorMatrix(
            blendRatio, 0, 0, 0,
            0, blendRatio, 0, 0,
            0, 0, blendRatio, 0,
            0, 0, 0, 1,
            tintRgba.R * blendRatio, tintRgba.G * blendRatio, tintRgba.B * blendRatio, 0);
        
        image.Mutate(context => context.Filter(tintMatrix));
        image.SaveAsPng(outputStream);
        
        return new()
        {
            ResourcePath = ResourcePath.ItemTexture($"{options.Id}_{IdSuffix}"),
            Data = outputStream.ToArray()
        };
    }
}

internal sealed class TextureGeneratorBuilder
{
    private TextureGeneratorBuilder(string idSuffix)
    {
        IdSuffix = idSuffix;
    }
    
    private string IdSuffix;
    private readonly List<TextureLayer> Layers = new();

    public static TextureGeneratorBuilder New(string idSuffix) => new(idSuffix);

    public TextureGeneratorBuilder AddLayer(TextureLayer layer)
    {
        Layers.Add(layer);
        return this;
    }
    
    public TextureGeneratorBuilder AddLayer(string baseTextureResourceId, Func<TextureOptions, Color> getColour) => AddLayer(new()
    {
        BaseTextureResourceId = baseTextureResourceId,
        GetColour = getColour
    });

    public TextureGenerator Build() => new(IdSuffix, Layers.ToArray());
}