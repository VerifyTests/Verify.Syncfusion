using System.IO.Compression;
using System.Text.RegularExpressions;

namespace VerifyTests;

public static partial class VerifySyncfusion
{
    // Matches the lang attribute of DrawingML run properties (a:rPr / a:endParaRPr).
    static readonly Regex langAttribute = new("(<a:(?:rPr|endParaRPr)\\b[^>]*?\\blang=)\"[^\"]*\"", RegexOptions.Compiled);

    // Syncfusion stamps the text runs it adds to drawings (eg the trial watermark) with the
    // OS locale, not CurrentCulture, so the package differs between machines. Replace it
    // with a fixed placeholder before deterministic packaging.
    static void ScrubLanguage(MemoryStream stream, Func<string, bool> includeEntry) =>
        ScrubEntries(
            stream,
            includeEntry,
            _ => langAttribute.Replace(_, "$1\"DeterministicLang\""));

    static void ScrubEntries(MemoryStream stream, Func<string, bool> includeEntry, Func<string, string> scrub)
    {
        using var archive = new ZipArchive(stream, ZipArchiveMode.Update, leaveOpen: true);
        foreach (var entry in archive.Entries)
        {
            if (!includeEntry(entry.FullName))
            {
                continue;
            }

            string content;
            using (var reader = new StreamReader(entry.Open()))
            {
                content = reader.ReadToEnd();
            }

            var scrubbed = scrub(content);
            if (scrubbed == content)
            {
                continue;
            }

            using var entryStream = entry.Open();
            entryStream.SetLength(0);
            using var writer = new StreamWriter(entryStream);
            writer.Write(scrubbed);
        }
    }
}
