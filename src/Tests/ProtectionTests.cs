using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;

// Guards the protection-salt scrubbing in VerifySyncfusion_Scrub.cs. Syncfusion writes a fresh
// random cryptographic salt (and derived hash) into document protection on every save, so without
// scrubbing the exported package differs on every run and the snapshot can never match. The
// committed baseline pins the scrubbed placeholders; if the scrub regresses, the salt becomes
// random again and this test fails on the next run.
//
// Word uses the legacy w:salt / w:hash attribute names (unlike Excel's saltValue / hashValue),
// so this also covers the prefixed-attribute and legacy-name branches of the scrub regex, and the
// baseline confirms the neighbouring w:cryptAlgorithmClass="hash" value is left untouched.
[TestFixture]
public class ProtectionTests
{
    static MemoryStream ProtectedWord()
    {
        var document = new WordDocument("sample.docx", FormatType.Docx);
        document.Protect(ProtectionType.AllowOnlyReading, "password");
        var stream = new MemoryStream();
        document.Save(stream, FormatType.Docx);
        document.Close();
        stream.Position = 0;
        return stream;
    }

    [Test]
    public Task VerifyWordProtected() =>
        Verify(ProtectedWord(), "docx");
}
