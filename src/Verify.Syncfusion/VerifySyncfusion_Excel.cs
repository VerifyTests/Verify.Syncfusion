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

    // A sheet is a page, hidden or not: PagedConversion names its png, and says which sheets and
    // which outputs the verification wants, so what is left out is neither drawn nor exported.
    static ConversionResult ConvertExcel(IWorkbook book, IReadOnlyDictionary<string, object> settings)
    {
        if (book.Version == ExcelVersion.Excel97to2003)
        {
            throw new("Excel97to2003 not supported");
        }

        var conversion = new PagedConversion(settings)
        {
            Info = GetInfo(book)
        };

        var includeCsv = !settings.IsDerivedTargetExcluded("csv");
        var includeImages = conversion.IncludeImages;
        if (includeImages)
        {
            // A workbook that was passed in may come from an engine that was not given one
            book.Application.XlsIORenderer ??= new XlsIORenderer();
        }

        var sheets = book.Worksheets;
        foreach (var number in conversion.Pages(sheets.Count))
        {
            var sheet = sheets[number - 1];

            // Not the text of the page, which would put it in the info file: a csv is a file of
            // its own, named by the sheet.
            if (includeCsv)
            {
                conversion.AddDerived(GetSheetTarget(sheet));
            }

            Stream? image = null;
            if (includeImages)
            {
                image = RenderSheet(sheet);
            }

            conversion.AddPage(number, image);
        }

        // Saved last, once the sheets are read and drawn. An unlicensed save adds a sheet with its
        // evaluation warning to the workbook, which is then in the xlsx and is not a page of it.
        // Building the deterministic xlsx is expensive, so skip it when the xlsx target is excluded.
        if (!settings.IsTargetExcluded("xlsx"))
        {
            conversion.Source(BuildXlsxTarget(book));
        }

        return conversion.Build();
    }

    // The sheet as the one image, from its first cell to the last that holds a value. Not its used
    // range, which takes in every cell that is only formatted: a column formatted to its end is a
    // thousand rows of nothing. Null for a sheet with no values, which has nothing to draw.
    static MemoryStream? RenderSheet(IWorksheet sheet)
    {
        var lastRow = 0;
        var lastColumn = 0;
        foreach (var cell in sheet.UsedCells)
        {
            if (cell.IsBlank)
            {
                continue;
            }

            lastRow = Math.Max(lastRow, cell.LastRow);
            lastColumn = Math.Max(lastColumn, cell.LastColumn);
        }

        if (lastRow == 0)
        {
            return null;
        }

        var stream = new MemoryStream();
        sheet.ConvertToImage(
            1,
            1,
            lastRow,
            lastColumn,
            new ExportImageOptions
            {
                ImageFormat = ExportImageFormat.Png,
                // Best is three times the size each way, which for a sheet is an image too large
                // to review
                ScalingMode = ScalingMode.Normal
            },
            stream);
        return stream;
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
            HiddenSheets = HiddenSheets(book),
        };

    // A hidden sheet has a csv as any other, so this is what says which are hidden. Null, so left
    // out, for a workbook with none.
    static List<string>? HiddenSheets(IWorkbook book)
    {
        var hidden = book.Worksheets
            .Where(_ => _.Visibility != WorksheetVisibility.Visible)
            .Select(_ => _.Name)
            .ToList();
        if (hidden.Count == 0)
        {
            return null;
        }

        return hidden;
    }

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