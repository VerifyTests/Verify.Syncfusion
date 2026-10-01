// Png is excluded in ModuleInitializer, so no png targets are produced.
[TestFixture]
public class OutputsTests
{
    [Test]
    public Task PdfWithoutPng() =>
        VerifyFile("sample.pdf");

    [Test]
    public Task WordWithoutPng() =>
        VerifyFile("sample.docx");

    [Test]
    public Task ExcelWithCsv() =>
        VerifyFile("sample.xlsx");
}
