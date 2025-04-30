using Syncfusion.Presentation;
using Syncfusion.PresentationRenderer;

namespace VerifyTests;

public static partial class VerifySyncfusion
{
    static ConversionResult ConvertPowerPoint(string? name, Stream stream, IReadOnlyDictionary<string, object> settings)
    {
        using var document = Presentation.Open(stream);
        return ConvertPowerPoint(name, document, settings);
    }

    static ConversionResult ConvertPowerPoint(string? name, IPresentation document, IReadOnlyDictionary<string, object> settings) =>
        new(document.BuiltInDocumentProperties, GetPowerPointStreams(name, document, settings).ToList());

    static IEnumerable<Target> GetPowerPointStreams(string? name, IPresentation document, IReadOnlyDictionary<string, object> settings)
    {
        var renderer = new PresentationRenderer();

        document.PresentationRenderer = renderer;
        var slides = document.Slides;
        var pagesToInclude = settings.GetPagesToInclude(slides.Count);
        for (var index = 0; index < pagesToInclude; index++)
        {
            var slide = slides[index];
            var image = slide.ConvertToImage(ExportImageFormat.Png);
            yield return new("png", image, name);
        }
    }
}