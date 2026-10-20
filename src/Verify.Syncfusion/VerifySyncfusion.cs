using Syncfusion.DocIO.DLS;
using Syncfusion.Presentation;
using Syncfusion.XlsIO;

namespace VerifyTests;

public static partial class VerifySyncfusion
{
    public static bool Initialized { get; private set; }

    public static void Initialize()
    {
        if (Initialized)
        {
            throw new("Already Initialized");
        }

        Initialized = true;

        // By default Syncfusion names the font/graphics-state resources it adds on save
        // (eg the trial watermark) with a fresh Guid, so the saved pdf differs on every run.
        PdfDocument.EnableUniqueResourceNaming = false;

#if !NETFRAMEWORK
        // Syncfusion reads a font's widths through Windows-1252 when converting Word to pdf, and
        // .NET Core ships only the Unicode encodings unless the code page provider is registered.
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif

        // The name a stream converter is passed is not used: Verify names what a converter returns
        // relative to the target that was converted.
        VerifierSettings.RegisterStreamConverter("xlsx", (_, target, context) => ConvertExcel(target, context));
        VerifierSettings.RegisterStreamConverter("xls", (_, target, context) => ConvertExcel(target, context));
        VerifierSettings.RegisterFileConverter<IWorkbook>(ConvertExcel);

        VerifierSettings.RegisterStreamConverter("pdf", (_, target, context) => ConvertPdf(target, context));
        VerifierSettings.RegisterFileConverter<PdfDocument>(ConvertPdf);
        VerifierSettings.RegisterFileConverter<PdfLoadedDocument>(ConvertPdf);

        VerifierSettings.RegisterStreamConverter("pptx", (_, target, context) => ConvertPowerPoint(target, context));
        VerifierSettings.RegisterStreamConverter("ppt", (_, target, context) => ConvertPowerPoint(target, context));
        VerifierSettings.RegisterFileConverter<IPresentation>(ConvertPowerPoint);

        VerifierSettings.RegisterStreamConverter("docx", (_, target, context) => ConvertDocx(target, context));
        VerifierSettings.RegisterStreamConverter("doc", (_, target, context) => ConvertDoc(target, context));
        VerifierSettings.RegisterFileConverter<WordDocument>(ConvertWord);
    }
}
