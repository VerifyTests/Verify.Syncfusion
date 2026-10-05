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

        // DocIO reads a document as one text, and cannot say which page a part of it is on.
        if (conversion.IncludeText)
        {
            using var stream = new MemoryStream();
            document.SaveTxt(stream, Encoding.UTF8);
            conversion.Text(stream.ReadAsString());
        }

        // DocIO renders every page at once, so AddImages drops the pages PagesToInclude leaves out
        // once they are rendered. It also records the page count, which only rendering gives.
        if (conversion.IncludeImages)
        {
            using var render = new DocIORenderer();
            conversion.AddImages(document.RenderAsImages());
        }

        // Read once the document has been saved and rendered, as it was before PagedConversion.
        conversion.Info = GetInfo(document);
        return conversion.Build();
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
