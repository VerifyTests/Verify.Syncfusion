namespace VerifyTests;

/// <summary>
/// Controls which output kinds a document is split into. Passed to <see cref="VerifySyncfusion.Initialize"/>.
/// </summary>
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
