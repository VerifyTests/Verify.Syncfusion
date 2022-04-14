public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Initialize() =>
        VerifySyncfusion.Initialize();
}