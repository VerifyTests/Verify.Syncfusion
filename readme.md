# <img src="/src/icon.png" height="30px"> Verify.Syncfusion

[![Build status](https://ci.appveyor.com/api/projects/status/hkr80o3jgok632nw?svg=true)](https://ci.appveyor.com/project/SimonCropp/Verify-Syncfusion)
[![NuGet Status](https://img.shields.io/nuget/v/Verify.Syncfusion.svg)](https://www.nuget.org/packages/Verify.Syncfusion/)

Extends [Verify](https://github.com/VerifyTests/Verify) to allow verification of documents via [Syncfusion File Formats](https://help.syncfusion.com/file-formats/introduction/).

Converts documents (pdf, docx, xslx, and pptx) to png/csv/text for verification.

An [Syncfusion License](https://www.syncfusion.com/sales/licensing) is required to use this tool.

<a href='https://dotnetfoundation.org' alt='Part of the .NET Foundation'><img src='https://raw.githubusercontent.com/VerifyTests/Verify/master/docs/dotNetFoundation.svg' height='30px'></a><br>
Part of the <a href='https://dotnetfoundation.org' alt=''>.NET Foundation</a>


## NuGet package

https://nuget.org/packages/Verify.Syncfusion/


## Usage


### Enable Verify.Syncfusion

<!-- snippet: ModuleInitializer.cs -->
<a id='snippet-ModuleInitializer.cs'></a>
```cs
using VerifyTests;

public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Initialize()
    {
        VerifySyncfusion.Initialize();
    }
}
```
<sup><a href='/src/Tests/ModuleInitializer.cs#L1-L10' title='Snippet source file'>snippet source</a> | <a href='#snippet-ModuleInitializer.cs' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### PDF


#### Verify a file

<!-- snippet: VerifyPdf -->
<a id='snippet-verifypdf'></a>
```cs
[Test]
public Task VerifyPdf()
{
    return Verifier.VerifyFile("sample.pdf");
}
```
<sup><a href='/src/Tests/Samples.cs#L8-L16' title='Snippet source file'>snippet source</a> | <a href='#snippet-verifypdf' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


#### Verify a Stream

<!-- snippet: VerifyPdfStream -->
<a id='snippet-verifypdfstream'></a>
```cs
[Test]
public Task VerifyPdfStream()
{
    return Verifier.Verify(File.OpenRead("sample.pdf"))
        .UseExtension("pdf");
}
```
<sup><a href='/src/Tests/Samples.cs#L25-L34' title='Snippet source file'>snippet source</a> | <a href='#snippet-verifypdfstream' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


#### Result

<!-- snippet: Samples.VerifyPdf.00.verified.txt -->
<a id='snippet-Samples.VerifyPdf.00.verified.txt'></a>
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
<sup><a href='/src/Tests/Samples.VerifyPdf.00.verified.txt#L1-L12' title='Snippet source file'>snippet source</a> | <a href='#snippet-Samples.VerifyPdf.00.verified.txt' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

[Samples.VerifyPdf.01.verified.png](/src/Tests/Samples.VerifyPdf.01.verified.png):

<img src="https://raw.githubusercontent.com/SimonCropp/Verify.Syncfusion/main/src/Tests/Samples.VerifyPdf.01.verified.png" width="200px">


### Excel


#### Verify a file

<!-- snippet: VerifyExcel -->
<a id='snippet-verifyexcel'></a>
```cs
[Test]
public Task VerifyExcel()
{
    return Verifier.VerifyFile("sample.xlsx");
}
```
<sup><a href='/src/Tests/Samples.cs#L61-L69' title='Snippet source file'>snippet source</a> | <a href='#snippet-verifyexcel' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


#### Verify a Stream

<!-- snippet: VerifyExcelStream -->
<a id='snippet-verifyexcelstream'></a>
```cs
[Test]
public Task VerifyExcelStream()
{
    return Verifier.Verify(File.OpenRead("sample.xlsx"))
        .UseExtension("xlsx");
}
```
<sup><a href='/src/Tests/Samples.cs#L71-L80' title='Snippet source file'>snippet source</a> | <a href='#snippet-verifyexcelstream' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


#### Result

<!-- snippet: Samples.VerifyExcel.00.verified.txt -->
<a id='snippet-Samples.VerifyExcel.00.verified.txt'></a>
```txt
{
  CodeName: ThisWorkbook,
  DetectDateTimeInValue: true,
  ArgumentsSeparator: ,,
  DisplayWorkbookTabs: true,
  Version: Xlsx,
  StandardFont: Arial,
  StandardFontSize: 10.0
}
```
<sup><a href='/src/Tests/Samples.VerifyExcel.00.verified.txt#L1-L9' title='Snippet source file'>snippet source</a> | <a href='#snippet-Samples.VerifyExcel.00.verified.txt' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

<!-- snippet: Samples.VerifyExcel.01.verified.csv -->
<a id='snippet-Samples.VerifyExcel.01.verified.csv'></a>
```csv
0, First Name, Last Name, Gender, Country, Age, Date, Id
1, Dulce, Abril, Female, United States, 32, 15/10/2017, 1562
2, Mara, Hashimoto, Female, Great Britain, 25, 16/08/2016, 1582
3, Philip, Gent, Male, France, 36, 21/05/2015, 2587
4, Kathleen, Hanner, Female, United States, 25, 15/10/2017, 3549
5, Nereida, Magwood, Female, United States, 58, 16/08/2016, 2468
6, Gaston, Brumm, Male, United States, 24, 21/05/2015, 2554
```
<sup><a href='/src/Tests/Samples.VerifyExcel.01.verified.csv#L1-L849' title='Snippet source file'>snippet source</a> | <a href='#snippet-Samples.VerifyExcel.01.verified.csv' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### Word


#### Verify a file

<!-- snippet: VerifyWord -->
<a id='snippet-verifyword'></a>
```cs
[Test]
public Task VerifyWord()
{
    return Verifier.VerifyFile("sample.docx");
}
```
<sup><a href='/src/Tests/Samples.cs#L82-L90' title='Snippet source file'>snippet source</a> | <a href='#snippet-verifyword' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


#### Verify a Stream

<!-- snippet: VerifyWordStream -->
<a id='snippet-verifywordstream'></a>
```cs
[Test]
public Task VerifyWordStream()
{
    return Verifier.Verify(File.OpenRead("sample.docx"))
        .UseExtension("docx");
}
```
<sup><a href='/src/Tests/Samples.cs#L92-L101' title='Snippet source file'>snippet source</a> | <a href='#snippet-verifywordstream' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


#### Result

<!-- snippet: Samples.VerifyWord.00.verified.txt -->
<a id='snippet-Samples.VerifyWord.00.verified.txt'></a>
```txt
{
  LastAuthor: Simon Cropp,
  Company: ,
  LinesCount: 9,
  ParagraphCount: 10,
  WordCount: 180,
  PageCount: 1,
  ApplicationName: Microsoft Office Word,
  CreateDate: DateTime_1,
  RevisionNumber: 3
}
```
<sup><a href='/src/Tests/Samples.VerifyWord.00.verified.txt#L1-L11' title='Snippet source file'>snippet source</a> | <a href='#snippet-Samples.VerifyWord.00.verified.txt' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

<!-- snippet: Samples.VerifyWord.01.verified.txt -->
<a id='snippet-Samples.VerifyWord.01.verified.txt'></a>
```txt
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
```
<sup><a href='/src/Tests/Samples.VerifyWord.01.verified.txt#L1-L12' title='Snippet source file'>snippet source</a> | <a href='#snippet-Samples.VerifyWord.01.verified.txt' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### PowerPoint


#### Verify a file

<!-- snippet: VerifyPowerPoint -->
<a id='snippet-verifypowerpoint'></a>
```cs
[Test]
public Task VerifyPowerPoint()
{
    return Verifier.VerifyFile("sample.pptx");
}
```
<sup><a href='/src/Tests/Samples.cs#L38-L46' title='Snippet source file'>snippet source</a> | <a href='#snippet-verifypowerpoint' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


#### Verify a Stream

<!-- snippet: VerifyPowerPointStream -->
<a id='snippet-verifypowerpointstream'></a>
```cs
[Test]
public Task VerifyPowerPointStream()
{
    return Verifier.Verify(File.OpenRead("sample.pptx"))
        .UseExtension("pptx");
}
```
<sup><a href='/src/Tests/Samples.cs#L48-L57' title='Snippet source file'>snippet source</a> | <a href='#snippet-verifypowerpointstream' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


#### Result

<!-- snippet: Samples.VerifyPowerPoint.00.verified.txt -->
<a id='snippet-Samples.VerifyPowerPoint.00.verified.txt'></a>
```txt
{
  Title: Lorem ipsum,
  Subject: ,
  Author: simon,
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
  ApplicationName: Microsoft Office PowerPoint
}
```
<sup><a href='/src/Tests/Samples.VerifyPowerPoint.00.verified.txt#L1-L19' title='Snippet source file'>snippet source</a> | <a href='#snippet-Samples.VerifyPowerPoint.00.verified.txt' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

[Samples.VerifyPowerPoint.01.verified.png](/src/Tests/Samples.VerifyPowerPoint.01.verified.png):

<img src="https://raw.githubusercontent.com/SimonCropp/Verify.Syncfusion/main/src/Tests/Samples.VerifyPowerPoint.01.verified.png" width="200px">


## File Samples

http://file-examples.com/



## Icon

[Boxes](https://thenounproject.com/term/boxes/1526666/) designed by [Amelia](https://thenounproject.com/langonsivani/) from [The Noun Project](https://thenounproject.com/).
