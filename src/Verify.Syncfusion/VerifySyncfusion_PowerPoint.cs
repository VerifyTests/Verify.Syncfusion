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
        List<Target> targets = [];
        // Building the deterministic pptx is expensive, so skip it when the pptx target is excluded.
        if (!settings.IsTargetExcluded("pptx"))
        {
            targets.Add(BuildPptxTarget(document));
        }

        targets.AddRange(GetPowerPointStreams(name, document, settings));
        // BuiltInDocumentProperties already surfaces the full SlideCount, so a PagesToInclude filter
        // (which only trims the rendered slide pngs) remains unambiguous in the info snapshot.
        return new(document.BuiltInDocumentProperties, targets);
    }

    // The pptx snapshot is always the full presentation, regardless of PagesToInclude:
    // PagesToInclude only trims the rendered slide png pages in GetPowerPointStreams.
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