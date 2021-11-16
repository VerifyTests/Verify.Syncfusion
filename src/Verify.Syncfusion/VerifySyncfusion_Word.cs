using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIORenderer;

namespace VerifyTests;

public static partial class VerifySyncfusion
{
    static ConversionResult ConvertDocx(Stream stream, IReadOnlyDictionary<string, object> settings)
    {
        return Convert(stream, settings, FormatType.Docx);
    }

    static ConversionResult ConvertDoc(Stream stream, IReadOnlyDictionary<string, object> settings)
    {
        return Convert(stream, settings, FormatType.Doc);
    }

    static ConversionResult Convert(Stream stream, IReadOnlyDictionary<string, object> settings, FormatType formatType)
    {
        var document = new WordDocument(stream, formatType);
        return ConvertWord(document, settings);
    }

    static ConversionResult ConvertWord(WordDocument document, IReadOnlyDictionary<string, object> settings)
    {
        return new(GetInfo(document), GetWordStreams(document, settings).ToList());
    }

    static object GetInfo(IWordDocument document)
    {
        return new
        {
            document.BuiltinDocumentProperties.Author,
        };
    }

    static IEnumerable<Target> GetWordStreams(WordDocument document, IReadOnlyDictionary<string, object> settings)
    {
        using var renderer = new DocIORenderer();
        using var convertToPdf = renderer.ConvertToPDF(document);
        return GetPdfStreams(convertToPdf, settings);
    }
}