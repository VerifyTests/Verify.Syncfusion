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
- `VerifySyncfusion_Pdf.cs` — PDF → PNG pages + extracted text
- `VerifySyncfusion_Excel.cs` — XLS/XLSX → CSV per sheet + XLSX export
- `VerifySyncfusion_Word.cs` — DOC/DOCX → text + PNG pages
- `VerifySyncfusion_PowerPoint.cs` — PPT/PPTX → PNG per slide
- `VerifySyncfusionSettings.cs` — Fluent API extensions (`PagesToInclude`, `PdfPngDevice`) that store config in Verify's context dictionary

Each converter returns a `ConversionResult` containing metadata (info object) and converted targets (streams for images/text/CSV).

## Key Conventions

- **Multi-targeting:** Library targets net48, net8.0, net9.0. Tests target net9.0 only.
- **Central package management:** All package versions are in `src/Directory.Packages.props`. Never put version attributes in csproj files.
- **TreatWarningsAsErrors** and **EnforceCodeStyleInBuild** are enabled in `src/Directory.Build.props`.
- **C# preview language features** are enabled (`LangVersion: preview`).
- **MarkdownSnippets:** README code samples are pulled from `#region` blocks in test code. The `<!-- snippet: -->` / `<!-- endSnippet -->` markers in `readme.md` are auto-generated — edit the source regions in `src/Tests/`, not the readme directly.
- **Verified files:** Test outputs (`.verified.txt`, `.verified.png`, `.verified.csv`, `.verified.xlsx`) are committed and should be updated via `dotnet test` when converter output changes.
- **CI:** AppVeyor builds from `src/appveyor.yml`. On failure, `*.received.*` files are uploaded as artifacts for diff inspection.
