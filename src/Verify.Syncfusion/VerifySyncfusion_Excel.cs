using Syncfusion.XlsIO;
using Syncfusion.XlsIORenderer;

namespace VerifyTests;

public static partial class VerifySyncfusion
{
    static ConversionResult ConvertExcel(string? name, Stream stream, IReadOnlyDictionary<string, object> settings)
    {
        var engine = new ExcelEngine
        {
            Excel =
            {
                XlsIORenderer = new XlsIORenderer()
            }
        };
        var workbook = engine.Excel.Workbooks.Open(stream);
        return ConvertExcel(name, workbook);
    }

    static ConversionResult ConvertExcel(string? name, IWorkbook book)
    {
        var info = GetInfo(book);
        return new(info, GetExcelStreams(name, book).ToList());
    }

    static object GetInfo(IWorkbook book) =>
        new
        {
            book.CodeName,
            book.Date1904,
            book.HasMacros,
            book.DisableMacrosStart,
            book.DetectDateTimeInValue,
            book.ArgumentsSeparator,
            book.DisplayWorkbookTabs,
            book.DisplayedTab,
            book.ActiveSheetIndex,
            book.IsRightToLeft,
            book.IsWindowProtection,
            book.Version,
            book.IsCellProtection,
            book.ReadOnly,
            book.ReadOnlyRecommended,
            book.StandardFont,
            book.StandardFontSize,
        };

    static IEnumerable<Target> GetExcelStreams(string? name, IWorkbook book)
    {
        foreach (var sheet in book.Worksheets)
        {
            using var stream = new MemoryStream();
            sheet.SaveAs(stream, ", ", Encoding.UTF8);
            var stringData = ReadNonEmptyLines(stream);
            yield return new("csv", stringData, name);
        }
    }

    static string ReadNonEmptyLines(MemoryStream stream)
    {
        stream.Position = 0;
        var builder = new StringBuilder();
        using (var writer = new StringWriter(builder))
        using (var reader = new StreamReader(stream))
        {
            while (reader.ReadLine() is { } line)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    writer.WriteLine(line);
                }
            }
        }

        return builder.ToString();
    }
}