using Syncfusion.XlsIO;
using Syncfusion.XlsIORenderer;

namespace VerifyTests;

public static partial class VerifySyncfusion
{
    static ConversionResult ConvertExcel(Stream stream, IReadOnlyDictionary<string, object> settings)
    {
        var engine = new ExcelEngine
        {
            Excel =
            {
                XlsIORenderer = new XlsIORenderer()
            }
        };
        var workbook = engine.Excel.Workbooks.Open(stream);
        return ConvertExcel(workbook, settings);
    }

    // A workbook has no pages, so this says which target is the workbook and which were derived
    // from it directly, rather than through PagedConversion.
    static ConversionResult ConvertExcel(IWorkbook book, IReadOnlyDictionary<string, object> settings)
    {
        if (book.Version == ExcelVersion.Excel97to2003)
        {
            throw new("Excel97to2003 not supported");
        }

        var info = GetInfo(book);

        Target? source = null;
        // Building the deterministic xlsx is expensive, so skip it when the xlsx target is excluded.
        if (!settings.IsTargetExcluded("xlsx"))
        {
            source = BuildXlsxTarget(book);
        }

        List<Target> sheets = [];
        if (!settings.IsDerivedTargetExcluded("csv"))
        {
            foreach (var sheet in book.Worksheets)
            {
                sheets.Add(GetSheetTarget(sheet));
            }
        }

        return new(info, source, sheets);
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

    static Target BuildXlsxTarget(IWorkbook book)
    {
        using var sourceStream = new MemoryStream();
        book.SaveAs(sourceStream, ExcelSaveType.SaveAsXLS);
        ScrubLanguage(sourceStream, _ => _.StartsWith("xl/drawings/", StringComparison.Ordinal) &&
                                         _.EndsWith(".xml", StringComparison.Ordinal));
        var resultStream = DeterministicPackage.Convert(sourceStream);

        return new("xlsx", resultStream);
    }

    // Named by the sheet, always, so that a second sheet adds a file rather than renaming the first.
    static Target GetSheetTarget(IWorksheet sheet)
    {
        using var stream = new MemoryStream();
        sheet.SaveAs(stream, ", ", Encoding.UTF8);
        var stringData = ReadNonEmptyLines(stream);
        return new("csv", stringData, sheet.Name);
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