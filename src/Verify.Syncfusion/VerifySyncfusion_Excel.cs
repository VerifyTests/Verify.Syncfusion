using System.Globalization;
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

    static (string value, bool replaceCellValue) GetCellValue(IRange cell, Counter counter)
    {
        if (!cell.HasValue())
        {
            return (string.Empty, false);
        }

        switch (cell.Type)
        {
            case CellValueType.IsNumeric:
                var value = cell.DoubleValue;
                if (cell.GetStyle().Custom.Contains('%'))
                {
                    // Percentage
                    return (value.ToString("P", CultureInfo.InvariantCulture), false);
                }

                return (value.ToString(CultureInfo.InvariantCulture), false);

            case CellValueType.IsBool:
                return (cell.BoolValue.ToString(), false);

            case CellValueType.IsDateTime:
                var date = cell.DateTimeValue;
                if (counter.TryConvert(date, out var dateResult))
                {
                    return (dateResult, true);
                }

                return (DateFormatter.Convert(date), false);

            case CellValueType.IsError:
                return (cell.Value.ToString()!, false);

            case CellValueType.IsNull:
                return ("", false);

            default:
                var text = cell.StringValue;
                if (counter.TryConvert(text, out var result))
                {
                    return (result, true);
                }

                return (text, false);
        }
    }
    static List<Target> GetExcelStreams(string? targetName, IWorkbook book)
    {
        foreach (var sheet in book.Worksheets)
        {
            ScrubCells(sheet);
        }
        using var sourceStream = new MemoryStream();
        book.SaveAs(sourceStream, ExcelSaveType.SaveAsXLS);
        var resultStream = DeterministicPackage.Convert(sourceStream);

        List<Target> targets = [new("xlsx", resultStream, performConversion: false)];
        foreach (var sheet in book.Worksheets)
        {
            targets.Add(GetSheetStreams(targetName, sheet));
        }

        return targets;
    }

    private static void ScrubCells(IWorksheet sheet)
    {
        var counter = Counter.Current;
        IRange usedRange = sheet.UsedRange;

        for (int row = 1; row <= usedRange.LastRow; row++)
        {
            for (int col = 1; col <= usedRange.LastColumn; col++)
            {
                IRange cell = sheet[row, col];

                var (value, replaceCellValue) = GetCellValue(cell, counter);
                if (replaceCellValue)
                {
                    cell.Value = value;
                }
            }
        }

        foreach (var range in sheet.Cells)
        {
            foreach (var cell in range.Cells)
            {

            }
        }
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