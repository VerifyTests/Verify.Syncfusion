namespace VerifyTests;

/// <summary>
/// What <c>Initialize</c> used to be given to choose the outputs a document is split into. Settings
/// of Verify choose that now, and nothing reads this: it is here so that code still naming it is
/// told what to use in its place.
/// </summary>
[Obsolete(
    "SyncfusionOutputs and the outputs argument of Initialize are replaced by settings of Verify: VerifierSettings.ExcludeDerivedTargets(\"png\") to leave out the page images, VerifierSettings.PageText(PageTextPlacement.None) to leave out the text and VerifierSettings.ExcludeDerivedTargets(\"csv\") to leave out the csv files. See https://github.com/VerifyTests/Verify.Syncfusion#migrating-from-3x",
    true)]
[Flags]
public enum SyncfusionOutputs
{
    /// <summary>
    /// No outputs. Only the source document and info are emitted.
    /// </summary>
    None = 0,

    /// <summary>
    /// Rendered png images: one per pdf page, docx page, and pptx slide.
    /// </summary>
    Png = 1,

    /// <summary>
    /// Extracted text: one txt per pdf page, and one txt for a docx.
    /// </summary>
    Text = 2,

    /// <summary>
    /// One csv per xlsx worksheet.
    /// </summary>
    Csv = 4,

    /// <summary>
    /// All output kinds.
    /// </summary>
    All = Png | Text | Csv
}
