using Syncfusion.DocIO.DLS;
using Syncfusion.Presentation;
using Syncfusion.XlsIO;

namespace VerifyTests;

public static partial class VerifySyncfusion
{
    public static bool Initialized { get; private set; }

    static SyncfusionOutputs outputs = SyncfusionOutputs.All;

    /// <param name="outputs">Which output kinds documents are split into. Defaults to <see cref="SyncfusionOutputs.All"/>.</param>
    public static void Initialize(SyncfusionOutputs outputs = SyncfusionOutputs.All)
    {
        if (Initialized)
        {
            throw new("Already Initialized");
        }

        Initialized = true;
        VerifySyncfusion.outputs = outputs;

        // By default Syncfusion names the font/graphics-state resources it adds on save
        // (eg the trial watermark) with a fresh Guid, so the saved pdf differs on every run.
        PdfDocument.EnableUniqueResourceNaming = false;

        VerifierSettings.RegisterStreamConverter("xlsx", ConvertExcel);
        VerifierSettings.RegisterStreamConverter("xls", ConvertExcel);
        VerifierSettings.RegisterFileConverter<IWorkbook>((target, context) => ConvertExcel(null, target, context));

        VerifierSettings.RegisterStreamConverter("pdf", ConvertPdf);
        VerifierSettings.RegisterFileConverter<PdfDocument>((target, context) => ConvertPdf(null, target, context));
        VerifierSettings.RegisterFileConverter<PdfLoadedDocument>((target, context) => ConvertPdf(null, target, context));

        VerifierSettings.RegisterStreamConverter("pptx", ConvertPowerPoint);
        VerifierSettings.RegisterStreamConverter("ppt", ConvertPowerPoint);
        VerifierSettings.RegisterFileConverter<IPresentation>((target, context) => ConvertPowerPoint(null, target, context));

        VerifierSettings.RegisterStreamConverter("docx", ConvertDocx);
        VerifierSettings.RegisterStreamConverter("doc", ConvertDoc);
        VerifierSettings.RegisterFileConverter<WordDocument>((target, context) => ConvertWord(null, target, context));
    }
}