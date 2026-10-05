[TestFixture]
public class Samples
{
    #region VerifyPdf

    [Test]
    public Task VerifyPdf() =>
        VerifyFile("sample.pdf");

    #endregion

    [Test]
    public Task VerifyPdfResolution() =>
        VerifyFile(ProjectFiles.sample_pdf.Path)
            .PdfPngDevice(
                _ => new()
                {
                    ScaleFactor = 4
                });

    #region VerifyPdfStream

    [Test]
    public Task VerifyPdfStream()
    {
        var stream = new MemoryStream(File.ReadAllBytes("sample.pdf"));
        return Verify(stream, "pdf");
    }

    #endregion

    // A document object, rather than a file or a stream, goes through the typed converter. The pdf
    // it gives back as the source must not then be converted again by the stream converter for pdf.
    [Test]
    public async Task VerifyPdfDocument()
    {
        using var document = new PdfLoadedDocument(File.ReadAllBytes("sample.pdf"));
        await Verify(document)
            .ExcludeDerivedTargets("png");
    }

    #region PageTextPerPage

    [Test]
    public Task PageTextPerPage() =>
        VerifyFile("sample.pdf")
            .PageText(PageTextPlacement.PerPage)
            .ExcludeDerivedTargets("png");

    #endregion

    #region PagesToInclude

    [Test]
    public Task PagesToInclude() =>
        VerifyFile("sample.pdf")
            .PagesToInclude(1)
            .ExcludeDerivedTargets("png");

    #endregion

#if DEBUG

    #region VerifyPowerPoint

    [Test]
    public Task VerifyPowerPoint() =>
        VerifyFile("sample.pptx");

    #endregion

    #region VerifyPowerPointStream

    [Test]
    public Task VerifyPowerPointStream()
    {
        var stream = new MemoryStream(File.ReadAllBytes("sample.pptx"));
        return Verify(stream, "pptx");
    }

    #endregion

    #region ExcludePptx

    // Excludes pptx, so the deterministic pptx target is skipped.
    [Test]
    public Task ExcludePptx() =>
        VerifyFile("sample.pptx")
            .ExcludeTargets("pptx");

    #endregion

#endif

    #region VerifyExcel

    [Test]
    public Task VerifyExcel() =>
        VerifyFile("sample.xlsx");

    #endregion

    #region VerifyExcelStream

    [Test]
    public Task VerifyExcelStream()
    {
        var stream = new MemoryStream(File.ReadAllBytes("sample.xlsx"));
        return Verify(stream, "xlsx");
    }

    #endregion

    #region ExcludeXlsx

    // Excludes xlsx, so the deterministic xlsx target is skipped.
    [Test]
    public Task ExcludeXlsx() =>
        VerifyFile("sample.xlsx")
            .ExcludeTargets("xlsx");

    #endregion

    // No csv is written for a sheet. The xlsx is excluded as well so that no workbook is saved
    // here: the trial watermark Syncfusion adds on save is numbered per process (TextBox 1, 2 ...),
    // so another save would change the xlsx snapshot of every test that runs after this one.
    [Test]
    public Task ExcludeCsv() =>
        VerifyFile("sample.xlsx")
            .ExcludeTargets("xlsx")
            .ExcludeDerivedTargets("csv");

    // A hidden sheet is verified as any other, so it has a csv. What says it is hidden is
    // HiddenSheets in the info file. The xlsx is excluded for the reason given above.
    [Test]
    public async Task HiddenSheet()
    {
        using var engine = new Syncfusion.XlsIO.ExcelEngine();
        var book = engine.Excel.Workbooks.Create(2);
        book.Version = Syncfusion.XlsIO.ExcelVersion.Xlsx;
        book.Worksheets[0].Range["A1"].Text = "First sheet";
        book.Worksheets[1].Range["A1"].Text = "Hidden sheet";
        book.Worksheets[1].Visibility = Syncfusion.XlsIO.WorksheetVisibility.Hidden;

        await Verify(book)
            .ExcludeTargets("xlsx");
    }

    // A hidden sheet is counted as any other, so here it is the first page: it is the one that is
    // drawn and exported, and the sheet that is shown is left out.
    [Test]
    public async Task HiddenSheetIsCountedByPagesToInclude()
    {
        using var engine = new Syncfusion.XlsIO.ExcelEngine();
        var book = engine.Excel.Workbooks.Create(2);
        book.Version = Syncfusion.XlsIO.ExcelVersion.Xlsx;
        book.Worksheets[0].Range["A1"].Text = "Hidden sheet";
        book.Worksheets[1].Range["A1"].Text = "Second sheet";
        book.Worksheets[1].Activate();
        book.Worksheets[0].Visibility = Syncfusion.XlsIO.WorksheetVisibility.Hidden;

        await Verify(book)
            .PagesToInclude(1)
            .ExcludeTargets("xlsx");
    }

    // A sheet that only code can unhide is verified as one Excel can
    [Test]
    public async Task VeryHiddenSheet()
    {
        using var engine = new Syncfusion.XlsIO.ExcelEngine();
        var book = engine.Excel.Workbooks.Create(2);
        book.Version = Syncfusion.XlsIO.ExcelVersion.Xlsx;
        book.Worksheets[0].Range["A1"].Text = "First sheet";
        book.Worksheets[1].Range["A1"].Text = "Very hidden sheet";
        book.Worksheets[1].Visibility = Syncfusion.XlsIO.WorksheetVisibility.StrongHidden;

        await Verify(book)
            .ExcludeTargets("xlsx");
    }

    #region VerifyWord

    [Test]
    public Task VerifyWord() =>
        VerifyFile("sample.docx");

    #endregion

    #region VerifyWordStream

    [Test]
    public Task VerifyWordStream()
    {
        var stream = new MemoryStream(File.ReadAllBytes("sample.docx"));
        return Verify(stream, "docx");
    }

    #endregion

    #region ExcludeDocx

    // Excludes docx, so the deterministic docx target is skipped.
    [Test]
    public Task ExcludeDocx() =>
        VerifyFile("sample.docx")
            .ExcludeTargets("docx");

    #endregion

    // The text of a docx is read page by page, so PerPage puts that of each page in a file of its
    // own. The second page has none worth a file once the trial watermark is all it holds.
    [Test]
    public Task WordTextPerPage() =>
        VerifyFile("sample.docx")
            .PageText(PageTextPlacement.PerPage)
            .ExcludeTargets("docx")
            .ExcludeDerivedTargets("png");

    // Every page of a docx is rendered at once, and those PagesToInclude leaves out are dropped.
    // The second page, since it is the small one: all it holds is the end of the trial watermark.
    [Test]
    public Task WordPagesToInclude() =>
        VerifyFile("sample.docx")
            .PagesToInclude(_ => _ == 2)
            .PageText(PageTextPlacement.None)
            .ExcludeTargets("docx");
}