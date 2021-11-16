static class Extensions
{
    public static bool HasValue(this object? input)
    {
        if (input is ValueType)
        {
            var obj = Activator.CreateInstance(input.GetType());
            return !obj!.Equals(input);
        }

        if (input is string s)
        {
            return !string.IsNullOrEmpty(s);
        }

        return input is not null;
    }

    public static void MoveToStart(this Stream stream)
    {
        if (stream.CanSeek)
        {
            stream.Position = 0;
        }
    }

    public static string ReadAsString(this Stream stream)
    {
        stream.MoveToStart();
        using StreamReader reader = new(stream);
        return reader.ReadToEnd();
    }
}