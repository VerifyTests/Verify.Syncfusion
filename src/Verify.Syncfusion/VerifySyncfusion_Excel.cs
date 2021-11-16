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

    static ConversionResult ConvertExcel(IWorkbook book, IReadOnlyDictionary<string, object> settings)
    {
        var info = GetInfo(book);
        return new(info, GetExcelStreams(book).ToList());
    }

    static object GetInfo(IWorkbook book)
    {
        return new
        {
            book.Author
        };
    }

    static IEnumerable<Target> GetExcelStreams(IWorkbook book)
    {
        foreach (var sheet in book.Worksheets)
        {
            var setup = sheet.PageSetup;
            setup.PrintGridlines = true;
            setup.LeftMargin = 0;
            setup.TopMargin = 0;
            setup.RightMargin = 0;
            setup.BottomMargin = 0;

            var firstRow = sheet.UsedRange.Row;
            var firstColumn = sheet.UsedRange.Column;
            var lastRow = sheet.UsedRange.LastRow;
            var lastColumn = sheet.UsedRange.LastColumn;
            var stream = new MemoryStream();
            var stopwatch = Stopwatch.StartNew();
            sheet.ConvertToImage(firstRow, firstColumn, lastRow, lastColumn, new() { ImageFormat = ExportImageFormat.Png }, stream);
            stopwatch.Stop();
            yield return new("png", stream);
        }
    }
}