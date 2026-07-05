#if DEBUG

[TestFixture]
public class PowerPointPagesToIncludeTests
{
    // PagesToInclude trims only the rendered slide pngs. The pptx snapshot stays the full
    // presentation, and the info's SlideCount (from BuiltInDocumentProperties) still reports all
    // three slides, so including a single slide remains unambiguous.
    [Test]
    public Task PagesToIncludeTrimsRenderedSlidesOnly()
    {
        var stream = new MemoryStream(File.ReadAllBytes("sample.pptx"));
        return Verify(stream, "pptx")
            .PagesToInclude(1);
    }
}

#endif
