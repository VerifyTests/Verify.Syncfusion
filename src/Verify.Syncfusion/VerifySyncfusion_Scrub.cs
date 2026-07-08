using System.IO.Compression;
using System.Text.RegularExpressions;

namespace VerifyTests;

public static partial class VerifySyncfusion
{
    // Matches the salt/hash attributes of protection elements, capturing the attribute
    // name so it can be preserved in the replacement. Covers all four forms seen across
    // Office documents:
    //   xlsx sheetProtection → saltValue / hashValue
    //   docx documentProtection → w:salt / w:hash (Syncfusion emits the legacy names)
    // The word-boundary anchors the attribute name and keeps the w: prefix (which sits
    // before the match) intact, while distinguishing salt= from saltValue=.
    static readonly Regex saltAttribute = new("\\b(salt|saltValue)=\"[^\"]*\"", RegexOptions.Compiled);
    static readonly Regex hashAttribute = new("\\b(hash|hashValue)=\"[^\"]*\"", RegexOptions.Compiled);

    // Office documents embed protection (Excel sheet protection, Word document/write
    // protection) with a fresh random cryptographic salt on every save, plus a hash
    // derived from it. That makes the exported package non-deterministic — the salt and
    // hash differ on every run — so the snapshot can never match. Replace them with fixed
    // placeholders before deterministic packaging.
    static void ScrubProtection(MemoryStream stream, Func<string, bool> includeEntry)
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

            var scrubbed = saltAttribute.Replace(content, "$1=\"DeterministicSalt\"");
            scrubbed = hashAttribute.Replace(scrubbed, "$1=\"DeterministicHash\"");
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
