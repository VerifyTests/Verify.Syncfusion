using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIORenderer;

namespace VerifyTests;

public static partial class VerifySyncfusion
{
    static ConversionResult ConvertDocx(Stream stream, IReadOnlyDictionary<string, object> settings) =>
        Convert(stream, FormatType.Docx, settings);

    static ConversionResult ConvertDoc(Stream stream, IReadOnlyDictionary<string, object> settings) =>
        Convert(stream, FormatType.Doc, settings);

    static ConversionResult Convert(Stream stream, FormatType formatType, IReadOnlyDictionary<string, object> settings)
    {
        var document = new WordDocument(stream, formatType);
        document.UpdateWordCount();
        document.UpdateDocumentFields();
        return ConvertWord(document, settings);
    }

    static ConversionResult ConvertWord(WordDocument document, IReadOnlyDictionary<string, object> settings)
    {
        var conversion = new PagedConversion(settings);

        // Building the deterministic docx is expensive, so skip it when the docx target is excluded.
        if (!settings.IsTargetExcluded("docx"))
        {
            conversion.Source(BuildDocxTarget(document));
        }

        // DocIO reads a document as one text, and cannot say which page a part of it is on. Laid out
        // as a pdf it has pages, each with its text, so PagesToInclude limits the text as it does
        // the images. That also gives the page count when no page is rendered.
        if (conversion.IncludeText)
        {
            AddPageTexts(conversion, document);
        }

        // Every page is rendered at once, and AddImages drops the pages PagesToInclude leaves out.
        // DocIO can render a range of pages, but only by index, which takes knowing how many there
        // are before any is drawn. The only count to hand is that of the pdf the text is read
        // from, and it is not the count of the images: without a license each conversion adds
        // its evaluation warning to the document, so the one that runs second can have a page more.
        // A page with text as well is added twice, once for each. They are the one page to
        // PagedConversion, which goes by the number.
        if (conversion.IncludeImages)
        {
            using var render = new DocIORenderer();
            conversion.AddImages(document.RenderAsImages());
        }

        // Read once the document has been saved and rendered, as it was before PagedConversion.
        conversion.Info = GetInfo(document);
        return conversion.Build();
    }

    static void AddPageTexts(PagedConversion conversion, WordDocument document)
    {
        using var pdfStream = new MemoryStream();
        using (var renderer = new DocIORenderer())
        using (var pdf = renderer.ConvertToPDF(document))
        {
            pdf.Save(pdfStream);
        }

        pdfStream.Position = 0;
        using var loaded = new PdfLoadedDocument(pdfStream);
        var pages = loaded.Pages;
        foreach (var number in conversion.Pages(pages.Count))
        {
            // By layout: read in the order it is written, the text of a rendered document is a
            // line for every run of it.
            conversion.AddPage(number, text: pages[number - 1].ExtractText(true));
        }
    }

    static Target BuildDocxTarget(WordDocument document)
    {
        using var source = new MemoryStream();
        document.Save(source, FormatType.Docx);
        var resultStream = DeterministicPackage.Convert(source);

        return new("docx", resultStream);
    }

    // The built in PageCount is not here. It is what the document last stored, not what it renders
    // to, and beside the page count PagedConversion writes it would be a second, different, one.
    static object GetInfo(IWordDocument document)
    {
        var properties = document.BuiltinDocumentProperties;
        return new
        {
            properties.Author,
            properties.LastAuthor,
            properties.Subject,
            properties.Category,
            properties.Company,
            properties.Manager,
            properties.LinesCount,
            properties.ParagraphCount,
            properties.WordCount,
            properties.ApplicationName,
            properties.CreateDate,
            properties.Keywords,
            properties.RevisionNumber,
        };
    }
}
