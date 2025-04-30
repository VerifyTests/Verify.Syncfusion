using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIORenderer;

namespace VerifyTests;

public static partial class VerifySyncfusion
{
    static ConversionResult ConvertDocx(string? name, Stream stream, IReadOnlyDictionary<string, object> settings) =>
        Convert(name, stream, FormatType.Docx);

    static ConversionResult ConvertDoc(string? name, Stream stream, IReadOnlyDictionary<string, object> settings) =>
        Convert(name, stream, FormatType.Doc);

    static ConversionResult Convert(string? name, Stream stream, FormatType formatType)
    {
        var document = new WordDocument(stream, formatType);
        document.UpdateWordCount();
        document.UpdateDocumentFields();
        return ConvertWord(name, document);
    }

    static ConversionResult ConvertWord(string? name, WordDocument document) =>
        new(GetInfo(document), GetWordStreams(name, document).ToList());

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
            properties.PageCount,
            properties.ApplicationName,
            properties.CreateDate,
            properties.Keywords,
            properties.RevisionNumber,
        };
    }

    static IEnumerable<Target> GetWordStreams(string? name, WordDocument document)
    {
        using var stream = new MemoryStream();
        document.SaveTxt(stream, Encoding.UTF8);
        yield return new("txt", stream.ReadAsString());

        using var render = new DocIORenderer();
        var images = document.RenderAsImages();

        foreach (var image in images)
        {
            yield return new("png", image, name);
        }
    }
}