#if DEBUG

[TestFixture]
public class PowerPointPagesToIncludeTests
{
    // PagesToInclude trims the png and the text of the slides. The pptx snapshot stays the full
    // presentation, and the info's PageCount, like the SlideCount from BuiltInDocumentProperties,
    // still reports all three slides, so including a single slide remains unambiguous.
    [Test]
    public Task PagesToIncludeTrimsSlides()
    {
        var stream = new MemoryStream(File.ReadAllBytes(ProjectFiles.sample_pptx));
        return Verify(stream, "pptx")
            .PagesToInclude(1);
    }
}

#endif
