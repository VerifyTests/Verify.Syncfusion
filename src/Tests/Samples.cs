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
        VerifyFile("sample.pdf")
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
}