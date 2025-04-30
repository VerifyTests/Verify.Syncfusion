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

        VerifierSettings.RegisterStreamConverter("xlsx", ConvertExcel);
        VerifierSettings.RegisterStreamConverter("xls", ConvertExcel);
        VerifierSettings.RegisterFileConverter<IWorkbook>((target, _) => ConvertExcel(null, target));

        VerifierSettings.RegisterStreamConverter("pdf", ConvertPdf);
        VerifierSettings.RegisterFileConverter<PdfDocument>((target, context) => ConvertPdf(null, target, context));
        VerifierSettings.RegisterFileConverter<PdfLoadedDocument>((target, context) => ConvertPdf(null, target, context));

        VerifierSettings.RegisterStreamConverter("pptx", ConvertPowerPoint);
        VerifierSettings.RegisterStreamConverter("ppt", ConvertPowerPoint);
        VerifierSettings.RegisterFileConverter<IPresentation>((target, context) => ConvertPowerPoint(null, target, context));

        VerifierSettings.RegisterStreamConverter("docx", ConvertDocx);
        VerifierSettings.RegisterStreamConverter("doc", ConvertDoc);
        VerifierSettings.RegisterFileConverter<WordDocument>((target, _) => ConvertWord(null, target));
    }
}