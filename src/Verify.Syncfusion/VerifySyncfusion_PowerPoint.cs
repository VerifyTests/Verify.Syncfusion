using DeterministicIoPackaging;
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

    static ConversionResult ConvertPowerPoint(string? name, IPresentation document, IReadOnlyDictionary<string, object> settings)
    {
        List<Target> targets = [BuildPptxTarget(document)];
        targets.AddRange(GetPowerPointStreams(name, document, settings));
        return new(document.BuiltInDocumentProperties, targets);
    }

    static Target BuildPptxTarget(IPresentation document)
    {
        using var source = new MemoryStream();
        document.Save(source);
        var resultStream = DeterministicPackage.Convert(source);

        return new("pptx", resultStream, performConversion: false)
        {
            BypassComparersForSubsequentOnDifference = true
        };
    }

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