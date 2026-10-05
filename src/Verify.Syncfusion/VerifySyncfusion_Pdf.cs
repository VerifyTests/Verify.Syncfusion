using DeterministicPdf;
using Syncfusion.EJ2.PdfViewer;

namespace VerifyTests;

public static partial class VerifySyncfusion
{
    static ConversionResult ConvertPdf(Stream stream, IReadOnlyDictionary<string, object> settings)
    {
        using var document = new PdfLoadedDocument(stream);

        return ConvertPdf(document, settings);
    }

    static ConversionResult ConvertPdf(PdfDocument document, IReadOnlyDictionary<string, object> settings)
    {
        var info = GetInfo(document.DocumentInformation);
        var pages = document.Pages.Cast<PdfPageBase>().ToList();
        return ConvertPdf(document, info, pages, settings);
    }

    static ConversionResult ConvertPdf(PdfLoadedDocument document, IReadOnlyDictionary<string, object> settings)
    {
        var info = GetInfo(document.DocumentInformation);
        var pages = document.Pages.Cast<PdfPageBase>().ToList();
        return ConvertPdf(document, info, pages, settings);
    }

    // The page count is not here: PagedConversion writes it to the info file.
    static object GetInfo(PdfDocumentInformation info)
    {
        if (info.Title == "Syncfusion" ||
            info.Subject == "Syncfusion" ||
            info.Author == "Syncfusion")
        {
            throw new("The default value of 'Syncfusion' for Title, Subject, or Author is not allowed.");
        }

        return new
        {
            info.Author,
            info.CreationDate,
            info.Creator,
            info.CustomMetadata,
            info.Keywords,
            info.Language,
            info.ModificationDate,
            info.Producer,
            info.Subject,
            info.Title,
        };
    }

    static ConversionResult ConvertPdf(
        PdfDocumentBase document,
        object info,
        List<PdfPageBase> pages,
        IReadOnlyDictionary<string, object> settings)
    {
        // Names the pages, places their text, and says which pages and which of their outputs the
        // verification wants, so a page that is not wanted is neither rendered nor read.
        var conversion = new PagedConversion(settings)
        {
            Info = info
        };

        var pdfStream = new MemoryStream();
        document.Save(pdfStream);
        pdfStream.Position = 0;

        // The document is already saved here to feed the renderer, so the pdf snapshot costs only the
        // neutralizing pass. It is always the full document, regardless of PagesToInclude, which
        // trims the pages below. Mirrors the xlsx/docx/pptx targets in the sibling formats.
        if (!settings.IsTargetExcluded("pdf"))
        {
            conversion.Source(new("pdf", PdfNormalizer.Normalize(pdfStream)));
            pdfStream.Position = 0;
        }

        var includeText = conversion.IncludeText;
        PdfRenderer? pngDevice = null;
        if (conversion.IncludeImages)
        {
            pngDevice = settings.GetPdfPngDevice(document);
            pngDevice.Load(pdfStream);
        }

        foreach (var number in conversion.Pages(pages.Count))
        {
            var index = number - 1;

            string? text = null;
            if (includeText)
            {
                text = pages[index].ExtractText();
            }

            Stream? png = null;
            if (pngDevice is not null)
            {
                png = RenderPage(pngDevice, index);
            }

            conversion.AddPage(number, png, text);
        }

        return conversion.Build();
    }

    static MemoryStream RenderPage(PdfRenderer pngDevice, int index)
    {
        var pngStream = new MemoryStream();
        var image = pngDevice.ExportAsImage(index);
        var skData = image.Encode(SKEncodedImageFormat.Png,100);
        skData.SaveTo(pngStream);
        return pngStream;
    }
}