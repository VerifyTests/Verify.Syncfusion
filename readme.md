# <img src="https://raw.githubusercontent.com/SimonCropp/Verify.Syncfusion/main/src/icon.png" height="30px"> Verify.Syncfusion

[![Discussions](https://img.shields.io/badge/Verify-Discussions-yellow?svg=true&label=)](https://github.com/orgs/VerifyTests/discussions)
[![Build status](https://ci.appveyor.com/api/projects/status/hkr80o3jgok632nw?svg=true)](https://ci.appveyor.com/project/SimonCropp/Verify-Syncfusion)
[![NuGet Status](https://img.shields.io/nuget/v/Verify.Syncfusion.svg)](https://www.nuget.org/packages/Verify.Syncfusion/)

Extends [Verify](https://github.com/VerifyTests/Verify) to allow verification of documents via [Syncfusion File Formats](https://help.syncfusion.com/file-formats/introduction/).

Converts documents (pdf, docx, xlsx, and pptx) to png/csv/text for verification.

**See [Milestones](../../milestones?state=closed) for release notes.**

An [Syncfusion License](https://www.syncfusion.com/sales/licensing) is required to use this tool.


## Sponsors

include: zzz


## NuGet

 * https://nuget.org/packages/Verify.Syncfusion


## Usage

<!-- snippet: enable -->
<a id='snippet-enable'></a>
```cs
[ModuleInitializer]
public static void Initialize() =>
    VerifySyncfusion.Initialize();
```
<sup><a href='/src/Tests/ModuleInitializer.cs#L3-L9' title='Snippet source file'>snippet source</a> | <a href='#snippet-enable' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### PDF


#### Verify a file

<!-- snippet: VerifyPdf -->
<a id='snippet-VerifyPdf'></a>
```cs
[Test]
public Task VerifyPdf() =>
    VerifyFile("sample.pdf");
```
<sup><a href='/src/Tests/Samples.cs#L4-L10' title='Snippet source file'>snippet source</a> | <a href='#snippet-VerifyPdf' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


#### Verify a Stream

<!-- snippet: VerifyPdfStream -->
<a id='snippet-VerifyPdfStream'></a>
```cs
[Test]
public Task VerifyPdfStream()
{
    var stream = new MemoryStream(File.ReadAllBytes("sample.pdf"));
    return Verify(stream, "pdf");
}
```
<sup><a href='/src/Tests/Samples.cs#L21-L30' title='Snippet source file'>snippet source</a> | <a href='#snippet-VerifyPdfStream' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


#### Result

<!-- snippet: Samples.VerifyPdf#00.verified.txt -->
<a id='snippet-Samples.VerifyPdf#00.verified.txt'></a>
```txt
{
  PageCount: 2,
  Author: ,
  CreationDate: DateTime_1,
  Creator: RAD PDF,
  CustomMetadata: [],
  Keywords: ,
  ModificationDate: DateTime_2,
  Producer: RAD PDF 3.9.0.0 - http://www.radpdf.com,
  Subject: ,
  Title: 
}
```
<sup><a href='/src/Tests/Samples.VerifyPdf#00.verified.txt#L1-L12' title='Snippet source file'>snippet source</a> | <a href='#snippet-Samples.VerifyPdf#00.verified.txt' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

[Samples.VerifyPdf#01.verified.png](src/Tests/Samples.VerifyPdf%2301.verified.png):

<img src="https://raw.githubusercontent.com/SimonCropp/Verify.Syncfusion/main/src/Tests/Samples.VerifyPdf%2301.verified.png" width="200px">


### Excel


#### Verify a file

<!-- snippet: VerifyExcel -->
<a id='snippet-VerifyExcel'></a>
```cs
[Test]
public Task VerifyExcel() =>
    VerifyFile("sample.xlsx");
```
<sup><a href='/src/Tests/Samples.cs#L55-L61' title='Snippet source file'>snippet source</a> | <a href='#snippet-VerifyExcel' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


#### Verify a Stream

<!-- snippet: VerifyExcelStream -->
<a id='snippet-VerifyExcelStream'></a>
```cs
[Test]
public Task VerifyExcelStream()
{
    var stream = new MemoryStream(File.ReadAllBytes("sample.xlsx"));
    return Verify(stream, "xlsx");
}
```
<sup><a href='/src/Tests/Samples.cs#L63-L72' title='Snippet source file'>snippet source</a> | <a href='#snippet-VerifyExcelStream' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


#### Result

<!-- snippet: Samples.VerifyExcel.verified.txt -->
<a id='snippet-Samples.VerifyExcel.verified.txt'></a>
```txt
{
  CodeName: ThisWorkbook,
  Date1904: false,
  HasMacros: false,
  DisableMacrosStart: false,
  DetectDateTimeInValue: true,
  ArgumentsSeparator: ,,
  DisplayWorkbookTabs: true,
  IsRightToLeft: false,
  IsWindowProtection: false,
  Version: Xlsx,
  IsCellProtection: false,
  ReadOnly: false,
  ReadOnlyRecommended: false,
  StandardFont: Arial,
  StandardFontSize: 10.0
}
```
<sup><a href='/src/Tests/Samples.VerifyExcel.verified.txt#L1-L17' title='Snippet source file'>snippet source</a> | <a href='#snippet-Samples.VerifyExcel.verified.txt' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

<!-- snippet: Samples.VerifyExcel.verified.csv -->
<a id='snippet-Samples.VerifyExcel.verified.csv'></a>
```csv
Created with a trial version of Syncfusion Excel library or registered the wrong key in your application. Go to www.syncfusion.com/account/claim-license-key to obtain the valid key.
0, First Name, Last Name, Gender, Country, Age, Date, Id
1, Dulce, Abril, Female, United States, 32, 15/10/2017, 1562
2, Mara, Hashimoto, Female, Great Britain, 25, 16/08/2016, 1582
3, Philip, Gent, Male, France, 36, 21/05/2015, 2587
4, Kathleen, Hanner, Female, United States, 25, 15/10/2017, 3549
5, Nereida, Magwood, Female, United States, 58, 16/08/2016, 2468
6, Gaston, Brumm, Male, United States, 24, 21/05/2015, 2554
Created with a trial version of Syncfusion Excel library or registered the wrong key in your application. Go to www.syncfusion.com/account/claim-license-key to obtain the valid key.
```
<sup><a href='/src/Tests/Samples.VerifyExcel.verified.csv#L1-L9' title='Snippet source file'>snippet source</a> | <a href='#snippet-Samples.VerifyExcel.verified.csv' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### Word

When verifying a Word file or stream, both the textual content of the Word file as well as a png export of the pages in the Word file are verified.

#### Verify a file

<!-- snippet: VerifyWord -->
<a id='snippet-VerifyWord'></a>
```cs
[Test]
public Task VerifyWord() =>
    VerifyFile("sample.docx");
```
<sup><a href='/src/Tests/Samples.cs#L74-L80' title='Snippet source file'>snippet source</a> | <a href='#snippet-VerifyWord' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


#### Verify a Stream

<!-- snippet: VerifyWordStream -->
<a id='snippet-VerifyWordStream'></a>
```cs
[Test]
public Task VerifyWordStream()
{
    var stream = new MemoryStream(File.ReadAllBytes("sample.docx"));
    return Verify(stream, "docx");
}
```
<sup><a href='/src/Tests/Samples.cs#L82-L91' title='Snippet source file'>snippet source</a> | <a href='#snippet-VerifyWordStream' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


#### Result

<!-- snippet: Samples.VerifyWord#00.verified.txt -->
<a id='snippet-Samples.VerifyWord#00.verified.txt'></a>
```txt
{
  LastAuthor: Simon Cropp,
  Company: ,
  LinesCount: 9,
  ParagraphCount: 10,
  WordCount: 178,
  PageCount: 1,
  ApplicationName: Microsoft Office Word,
  CreateDate: DateTime_1,
  RevisionNumber: 3
}
```
<sup><a href='/src/Tests/Samples.VerifyWord#00.verified.txt#L1-L11' title='Snippet source file'>snippet source</a> | <a href='#snippet-Samples.VerifyWord#00.verified.txt' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

<!-- snippet: Samples.VerifyWord#01.verified.txt -->
<a id='snippet-Samples.VerifyWord#01.verified.txt'></a>
```txt
Created with a trial version of Syncfusion Word library or registered the wrong key in your application. Go to "www.syncfusion.com/account/claim-license-key" to obtain the valid key.
Lorem ipsum 

  Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc ac faucibus odio. 

Vestibulum neque massa, scelerisque sit amet ligula eu, congue molestie mi. Praesent ut varius sem. Nullam at porttitor arcu, nec lacinia nisi. Ut ac dolor vitae odio interdum condimentum. Vivamus dapibus sodales ex, vitae malesuada ipsum cursus convallis. Maecenas sed egestas nulla, ac condimentum orci. Mauris diam felis, vulputate ac suscipit et, iaculis non est. Curabitur semper arcu ac ligula semper, nec luctus nisl blandit. Integer lacinia ante ac libero lobortis imperdiet. Nullam mollis convallis ipsum, ac accumsan nunc vehicula vitae. Nulla eget justo in felis tristique fringilla. Morbi sit amet tortor quis risus auctor condimentum. Morbi in ullamcorper elit. Nulla iaculis tellus sit amet mauris tempus fringilla.
Maecenas mauris lectus, lobortis et purus mattis, blandit dictum tellus.
* Maecenas non lorem quis tellus placerat varius. 
* Nulla facilisi. 
* Aenean congue fringilla justo ut aliquam. 
* Mauris id ex erat. Nunc vulputate neque vitae justo facilisis, non condimentum ante sagittis. 
* Morbi viverra semper lorem nec molestie. 
* Maecenas tincidunt est efficitur ligula euismod, sit amet ornare est vulputate.

Created with a trial version of Syncfusion Word library or registered the wrong key in your application. Go to "www.syncfusion.com/account/claim-license-key" to obtain the valid key.
```
<sup><a href='/src/Tests/Samples.VerifyWord#01.verified.txt#L1-L15' title='Snippet source file'>snippet source</a> | <a href='#snippet-Samples.VerifyWord#01.verified.txt' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


[Samples.VerifyWord#00.verified.png](src/Tests/Samples.VerifyWord%2300.verified.png):
<img src="https://raw.githubusercontent.com/SimonCropp/Verify.Syncfusion/main/src/Tests/Samples.VerifyWord%2300.verified.png" width="200px">

[Samples.VerifyWord#01.verified.png](src/Tests/Samples.VerifyWord%2301.verified.png):
<img src="https://raw.githubusercontent.com/SimonCropp/Verify.Syncfusion/main/src/Tests/Samples.VerifyWord%2301.verified.png" width="200px">

### PowerPoint


#### Verify a file

<!-- snippet: VerifyPowerPoint -->
<a id='snippet-VerifyPowerPoint'></a>
```cs
[Test]
public Task VerifyPowerPoint() =>
    VerifyFile("sample.pptx");
```
<sup><a href='/src/Tests/Samples.cs#L34-L40' title='Snippet source file'>snippet source</a> | <a href='#snippet-VerifyPowerPoint' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


#### Verify a Stream

<!-- snippet: VerifyPowerPointStream -->
<a id='snippet-VerifyPowerPointStream'></a>
```cs
[Test]
public Task VerifyPowerPointStream()
{
    var stream = new MemoryStream(File.ReadAllBytes("sample.pptx"));
    return Verify(stream, "pptx");
}
```
<sup><a href='/src/Tests/Samples.cs#L42-L51' title='Snippet source file'>snippet source</a> | <a href='#snippet-VerifyPowerPointStream' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


#### Result

<!-- snippet: Samples.VerifyPowerPoint.verified.txt -->
<a id='snippet-Samples.VerifyPowerPoint.verified.txt'></a>
```txt
{
  Title: Lorem ipsum,
  Subject: ,
  Author: Simon Cropp,
  Keywords: ,
  Comments: ,
  Template: ,
  LastAuthor: Simon Cropp,
  RevisionNumber: 1,
  LastPrinted: DateTime_1,
  CreationDate: DateTime_2,
  LastSaveDate: DateTime_3,
  WordCount: 231,
  PresentationTarget: Custom,
  ParagraphCount: 14,
  SlideCount: 3,
  NoteCount: 3,
  ScaleCrop: false,
  LinksDirty: false,
  ApplicationName: Microsoft Office PowerPoint
}
```
<sup><a href='/src/Tests/Samples.VerifyPowerPoint.verified.txt#L1-L21' title='Snippet source file'>snippet source</a> | <a href='#snippet-Samples.VerifyPowerPoint.verified.txt' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

[Samples.VerifyPowerPoint#01.verified.png](src/Tests/Samples.VerifyPowerPoint%2301.verified.png):

<img src="https://raw.githubusercontent.com/SimonCropp/Verify.Syncfusion/main/src/Tests/Samples.VerifyPowerPoint%2301.verified.png" width="200px">


## File Samples

http://file-examples.com/


## Icon

[Boxes](https://thenounproject.com/term/boxes/1526666/) designed by [Amelia](https://thenounproject.com/langonsivani/) from [The Noun Project](https://thenounproject.com/).
