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

        VerifierSettings.RegisterFileConverter("xlsx", ConvertExcel);
        VerifierSettings.RegisterFileConverter("xls", ConvertExcel);
        VerifierSettings.RegisterFileConverter<IWorkbook>(ConvertExcel);

        VerifierSettings.RegisterFileConverter("pdf", ConvertPdf);
        VerifierSettings.RegisterFileConverter<PdfDocument>(ConvertPdf);
        VerifierSettings.RegisterFileConverter<PdfLoadedDocument>(ConvertPdf);

        VerifierSettings.RegisterFileConverter("pptx", ConvertPowerPoint);
        VerifierSettings.RegisterFileConverter("ppt", ConvertPowerPoint);
        VerifierSettings.RegisterFileConverter<IPresentation>(ConvertPowerPoint);

        VerifierSettings.RegisterFileConverter("docx", ConvertDocx);
        VerifierSettings.RegisterFileConverter("doc", ConvertDoc);
        VerifierSettings.RegisterFileConverter<WordDocument>(ConvertWord);
    }
}