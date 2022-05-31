using Syncfusion.Presentation;
using Syncfusion.PresentationRenderer;

namespace VerifyTests;

public static partial class VerifySyncfusion
{
    static ConversionResult ConvertPowerPoint(Stream stream, IReadOnlyDictionary<string, object> settings)
    {
        using var document = Presentation.Open(stream);
        return ConvertPowerPoint(document, settings);
    }

    static ConversionResult ConvertPowerPoint(IPresentation document, IReadOnlyDictionary<string, object> settings) =>
        new(document.BuiltInDocumentProperties, GetPowerPointStreams(document, settings).ToList());

    static IEnumerable<Target> GetPowerPointStreams(IPresentation document, IReadOnlyDictionary<string, object> settings)
    {
        var renderer = new PresentationRenderer();

        document.PresentationRenderer = renderer;
        var pagesToInclude = settings.GetPagesToInclude(document.Slides.Count);
        for (var index = 0; index < pagesToInclude; index++)
        {
            var slide = document.Slides[index];
            var image = slide.ConvertToImage(ExportImageFormat.Png);
            yield return new("png", image);
        }
    }
}