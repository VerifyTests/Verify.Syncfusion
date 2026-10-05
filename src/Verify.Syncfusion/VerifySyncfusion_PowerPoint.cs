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

    // A slide is a page: PagedConversion names its png, places its text, and writes the number of
    // slides to the info file as the page count, so a PagesToInclude filter remains unambiguous in
    // the info snapshot.
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
        var includeImages = conversion.IncludeImages;
        var includeText = conversion.IncludeText;
        if (includeImages)
        {
            document.PresentationRenderer = new PresentationRenderer();
        }

        foreach (var number in conversion.Pages(slides.Count))
        {
            var slide = slides[number - 1];

            Stream? image = null;
            if (includeImages)
            {
                image = slide.ConvertToImage(ExportImageFormat.Png);
            }

            string? text = null;
            if (includeText)
            {
                text = GetSlideText(slide);
            }

            conversion.AddPage(number, image, text);
        }

        return conversion.Build();
    }

    // The text of a slide: a line for each paragraph that has any, in the order of its shapes.
    static string GetSlideText(ISlide slide)
    {
        var builder = new StringBuilder();
        AppendText(builder, slide.Shapes);
        return builder.ToString();
    }

    static void AppendText(StringBuilder builder, IShapes shapes)
    {
        foreach (var item in shapes)
        {
            if (item is IGroupShape group)
            {
                AppendText(builder, group.Shapes);
                continue;
            }

            if (item is ITable table)
            {
                foreach (var row in table.Rows)
                {
                    foreach (var cell in row.Cells)
                    {
                        AppendText(builder, cell.TextBody);
                    }
                }

                continue;
            }

            if (item is IShape shape)
            {
                AppendText(builder, shape.TextBody);
            }
        }
    }

    static void AppendText(StringBuilder builder, ITextBody? body)
    {
        if (body is null)
        {
            return;
        }

        foreach (var paragraph in body.Paragraphs)
        {
            var text = paragraph.Text;
            if (!string.IsNullOrEmpty(text))
            {
                builder.AppendLine(text);
            }
        }
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
