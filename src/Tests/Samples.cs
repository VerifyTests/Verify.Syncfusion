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
            .PdfPngDevice(_ => new(){ScaleFactor = 4});

    #region VerifyPdfStream

    [Test]
    public Task VerifyPdfStream() =>
        Verify(File.OpenRead("sample.pdf"))
            .UseExtension("pdf");

    #endregion

#if DEBUG

    #region VerifyPowerPoint

    [Test]
    public Task VerifyPowerPoint() =>
        VerifyFile("sample.pptx");

    #endregion

    #region VerifyPowerPointStream

    [Test]
    public Task VerifyPowerPointStream() =>
        Verify(File.OpenRead("sample.pptx"))
            .UseExtension("pptx");

    #endregion

#endif

    #region VerifyExcel

    [Test]
    public Task VerifyExcel() =>
        VerifyFile("sample.xlsx");

    #endregion

    #region VerifyExcelStream

    [Test]
    public Task VerifyExcelStream() =>
        Verify(File.OpenRead("sample.xlsx"))
            .UseExtension("xlsx");

    #endregion

    #region VerifyWord

    [Test]
    public Task VerifyWord() =>
        VerifyFile("sample.docx");

    #endregion

    #region VerifyWordStream

    [Test]
    public Task VerifyWordStream() =>
        Verify(File.OpenRead("sample.docx"))
            .UseExtension("docx");

    #endregion
}