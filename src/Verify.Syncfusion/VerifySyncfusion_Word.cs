using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;

namespace VerifyTests;

public static partial class VerifySyncfusion
{
    static ConversionResult ConvertDocx(Stream stream, IReadOnlyDictionary<string, object> settings) =>
        Convert(stream, settings, FormatType.Docx);

    static ConversionResult ConvertDoc(Stream stream, IReadOnlyDictionary<string, object> settings) =>
        Convert(stream, settings, FormatType.Doc);

    static ConversionResult Convert(Stream stream, IReadOnlyDictionary<string, object> settings, FormatType formatType)
    {
        var document = new WordDocument(stream, formatType);
        document.UpdateWordCount();
        document.UpdateDocumentFields();
        return ConvertWord(document, settings);
    }

    static ConversionResult ConvertWord(WordDocument document, IReadOnlyDictionary<string, object> settings) =>
        new(GetInfo(document), GetWordStreams(document).ToList());

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

    static IEnumerable<Target> GetWordStreams(WordDocument document)
    {
        using var stream = new MemoryStream();
        document.SaveTxt(stream, Encoding.UTF8);
        yield return new("txt", stream.ReadAsString(), null);
    }
}