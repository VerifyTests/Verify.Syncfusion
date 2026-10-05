# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Verify.Syncfusion is a [Verify](https://github.com/VerifyTests/Verify) extension that enables snapshot/approval testing of Syncfusion documents (PDF, Word, Excel, PowerPoint). It converts documents to PNG images, text, and CSV for verification.

## Build and Test Commands

```bash
# Build
dotnet build src --configuration Release

# Run all tests
dotnet test src --configuration Release

# Run a single test
dotnet test src --configuration Release --filter "VerifyPdf"
```

**Syncfusion license required:** Tests expect a `SyncfusionLicense` environment variable. Tests will throw if it's missing.

## Architecture

All library code lives in `src/Verify.Syncfusion/` under the `VerifyTests` namespace. The main class `VerifySyncfusion` is a partial class split across files by document type:

- `VerifySyncfusion.cs` — Entry point. `Initialize()` registers stream/file converters with Verify for each supported format.
- `VerifySyncfusion_Pdf.cs` — PDF → the normalized pdf + a PNG and the extracted text of each page
- `VerifySyncfusion_Excel.cs` — XLS/XLSX → the deterministic xlsx + a CSV per sheet
- `VerifySyncfusion_Word.cs` — DOC/DOCX → the deterministic docx + the text of the document + a PNG per page
- `VerifySyncfusion_PowerPoint.cs` — PPT/PPTX → the deterministic pptx + a PNG per slide
- `VerifySyncfusionSettings.cs` — Fluent API extension (`PdfPngDevice`) that stores config in Verify's context dictionary

The pdf, Word and PowerPoint converters build their `ConversionResult` with Verify's `PagedConversion`: the document as the `Source`, its properties as `Info`, and each page (or slide) with its png and text. `PagedConversion` names the page files (`page_0001`), places the text, and writes the info file in the shape every paged document has (`Document`, `PageCount`, `Text`, `Pages`). A workbook has no pages, so the Excel converter uses the `ConversionResult(info, source, derived)` constructor directly, with a csv per sheet that is always named by the sheet. Naming the source is what ties the pages, sheets and info file to the document for comparison (derived targets skip their comparers when the document differs) and for the diff tool, and what stops the document being converted again.

What is produced is decided by Verify's settings, not by an option of this plugin: `PageText` (in the info file, a file per page, or none), `PagesToInclude`, `ExcludeDerivedTargets("png")` / `ExcludeDerivedTargets("csv")`, and `ExcludeTargets("docx")` for the document. The converters ask first (`IncludeImages`, `IncludeText`, `Pages`, `IsTargetExcluded`, `IsDerivedTargetExcluded`), so nothing left out is rendered, read or built. They ignore the `name` a stream converter is passed: Verify names what a converter returns relative to the target that was converted.

Where Syncfusion limits that:
- DocIO reads a document as one text, so the Word converter passes it to `PagedConversion.Text`: it is `Text` in the info file, or `#text.verified.txt` under `PageTextPlacement.PerPage`, never per page.
- DocIO renders every page at once, so the Word converter uses `AddImages`: pages `PagesToInclude` leaves out are rendered and then dropped, and `PageCount` is in the info file only when pages are rendered. The built in `PageCount` document property is deliberately not part of the Word info: it is stored metadata, and can differ from the rendered count.
- PowerPoint has no text output, only a png per slide.

## Key Conventions

- **Multi-targeting:** Library targets net48, net8.0, net9.0. Tests target net9.0 only.
- **Central package management:** All package versions are in `src/Directory.Packages.props`. Never put version attributes in csproj files.
- **TreatWarningsAsErrors** and **EnforceCodeStyleInBuild** are enabled in `src/Directory.Build.props`.
- **C# preview language features** are enabled (`LangVersion: preview`).
- **MarkdownSnippets:** README code samples are pulled from `#region` blocks in test code. The `<!-- snippet: -->` / `<!-- endSnippet -->` markers in `readme.md` are auto-generated — edit the source regions in `src/Tests/`, not the readme directly.
- **Verified files:** Test outputs (`.verified.txt`, `.verified.png`, `.verified.csv`, `.verified.xlsx`) are committed and should be updated via `dotnet test` when converter output changes.
- **Global Verify settings:** `src/StaticSettingsTests/` is a separate test project because a setting on `VerifierSettings` (there, `ExcludeDerivedTargets("png")`) applies to every test in the process.
- **xlsx snapshots depend on test order:** the trial watermark Syncfusion adds when a workbook is saved is a text box numbered by a process wide counter (`TextBox 1`, `TextBox 2`, ...), so the bytes of a `.verified.xlsx` depend on how many workbooks were saved before it in the test run. A new test that saves an xlsx shifts the xlsx snapshot of every test that runs after it (NUnit runs a fixture's tests in name order). `Samples.ExcludeCsv` excludes the xlsx for that reason.
- **CI:** GitHub Actions builds from `.github/workflows/build.yml`. On failure, `*.received.*` files are uploaded as artifacts for diff inspection.
