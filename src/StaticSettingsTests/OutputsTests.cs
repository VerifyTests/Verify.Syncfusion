// Png is excluded in ModuleInitializer, so no png targets are produced.
[TestFixture]
public class OutputsTests
{
    [Test]
    public Task PdfWithoutPng() =>
        VerifyFile(ProjectFiles.sample_pdf.Path);

    [Test]
    public Task WordWithoutPng() =>
        VerifyFile(ProjectFiles.sample_docx.Path);

    [Test]
    public Task ExcelWithCsv() =>
        VerifyFile(ProjectFiles.sample_xlsx.Path);
}
