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

    // Matches the version in the claim-license-key link of the trial watermark: base64 of the
    // Syncfusion package version (eg vs=MzUuMS40MA== for 35.1.40).
    static readonly Regex licenseVersion = new("(claim-license-key\\?pl=[A-Za-z0-9+/=]*&(?:amp;)?vs=)[A-Za-z0-9+/]+=*", RegexOptions.Compiled);

    // The trial watermark links to claim-license-key with the package version, so every
    // Syncfusion bump would change every saved document. Replace it with a fixed placeholder.
    static void ScrubLicenseVersion(MemoryStream stream) =>
        ScrubEntries(
            stream,
            _ => _.EndsWith(".xml", StringComparison.Ordinal) ||
                 _.EndsWith(".rels", StringComparison.Ordinal),
            _ => licenseVersion.Replace(_, "$1DeterministicVersion"));

    // A pdf holds byte offsets (the xref table), so the version is overwritten with the same
    // number of characters rather than replaced with a placeholder of a different length.
    static void ScrubPdfLicenseVersion(MemoryStream stream)
    {
        var buffer = stream.GetBuffer();
        var length = (int) stream.Length;
        // Latin1 maps each byte to one char, so a match index is a byte index. Encoding.Latin1 is not in net48.
        var content = Encoding.GetEncoding(28591).GetString(buffer, 0, length);
        foreach (Match match in licenseVersion.Matches(content))
        {
            var prefix = match.Groups[1];
            var start = prefix.Index + prefix.Length;
            var end = match.Index + match.Length;
            for (var i = start; i < end; i++)
            {
                buffer[i] = (byte) 'X';
            }
        }
    }

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
