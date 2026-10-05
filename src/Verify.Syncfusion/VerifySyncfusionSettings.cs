using Syncfusion.EJ2.PdfViewer;

namespace VerifyTests;

public static class VerifySyncfusionSettings
{
    public static void PdfPngDevice(this VerifySettings settings, Func<PdfDocumentBase, PdfRenderer> func) =>
        settings.Context["VerifySyncfusionPdfPngDevice"] = func;

    public static SettingsTask PdfPngDevice(this SettingsTask settings, Func<PdfDocumentBase, PdfRenderer> func)
    {
        settings.CurrentSettings.PdfPngDevice(func);
        return settings;
    }

    internal static PdfRenderer GetPdfPngDevice(this IReadOnlyDictionary<string, object> settings, PdfDocumentBase document)
    {
        if (!settings.TryGetValue("VerifySyncfusionPdfPngDevice", out var value))
        {
            return new();
        }

        var func = (Func<PdfDocumentBase, PdfRenderer>) value;
        return func(document);
    }
}