using DeterministicIoPackaging;
using Syncfusion.XlsIO;
using Syncfusion.XlsIORenderer;

namespace VerifyTests;

public static partial class VerifySyncfusion
{
    static ConversionResult ConvertExcel(string? targetName, Stream stream, IReadOnlyDictionary<string, object> settings)
    {
        var engine = new ExcelEngine
        {
            Excel =
            {
                XlsIORenderer = new XlsIORenderer()
            }
        };
        var workbook = engine.Excel.Workbooks.Open(stream);
        return ConvertExcel(targetName, workbook);
    }

    static ConversionResult ConvertExcel(string? targetName, IWorkbook book)
    {
        var info = GetInfo(book);
        return new(info, GetExcelStreams(targetName, book).ToList());
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

    static List<Target> GetExcelStreams(string? targetName, IWorkbook book)
    {
        using var sourceStream = new MemoryStream();
        if (book.Version == ExcelVersion.Excel97to2003)
        {
            throw new("Excel97to2003 not supported");
        }

        book.SaveAs(sourceStream, ExcelSaveType.SaveAsXLS);
        var resultStream = DeterministicPackage.Convert(sourceStream);

        List<Target> targets = [new("xlsx", resultStream, performConversion: false)];
        foreach (var sheet in book.Worksheets)
        {
            targets.Add(GetSheetStreams(targetName, sheet));
        }

        return targets;
    }

    static Target GetSheetStreams(string? targetName, IWorksheet sheet)
    {
        string targetAndSheet;
        if (targetName == null)
        {
            targetAndSheet = sheet.Name;
        }
        else
        {
            targetAndSheet = $"{targetName}-{sheet.Name}";
        }
        using var stream = new MemoryStream();
        sheet.SaveAs(stream, ", ", Encoding.UTF8);
        var stringData = ReadNonEmptyLines(stream);
        return new("csv", stringData, targetAndSheet);
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