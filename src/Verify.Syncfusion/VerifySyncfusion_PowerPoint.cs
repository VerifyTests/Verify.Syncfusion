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

    // A slide is a page: PagedConversion names its png, and writes the number of slides to the info
    // file as the page count, so a PagesToInclude filter remains unambiguous in the info snapshot.
    static ConversionResult ConvertPowerPoint(IPresentation document, IReadOnlyDictionary<string, object> settings)
    {
        var conversion = new PagedConversion(settings)
        {
            Info = document.BuiltInDocumentProperties
        };

        // Building the deterministic pptx is expensive, so skip it when the pptx target is excluded.
        if (!settings.IsTargetExcluded("pptx"))
        {
            conversion.Source(BuildPptxTarget(document));
        }

        var slides = document.Slides;
        // Set here rather than left to Pages, so the info file has it when no slide is rendered.
        conversion.PageCount = slides.Count;
        if (conversion.IncludeImages)
        {
            document.PresentationRenderer = new PresentationRenderer();
            foreach (var number in conversion.Pages(slides.Count))
            {
                var image = slides[number - 1].ConvertToImage(ExportImageFormat.Png);
                conversion.AddPage(number, image);
            }
        }

        return conversion.Build();
    }

    // The pptx snapshot is always the full presentation, regardless of PagesToInclude:
    // PagesToInclude only trims the rendered slide png pages in ConvertPowerPoint.
    static Target BuildPptxTarget(IPresentation document)
    {
        using var source = new MemoryStream();
        document.Save(source);
        var resultStream = DeterministicPackage.Convert(source);

        return new("pptx", resultStream);
    }
}
