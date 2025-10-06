using VerifyTests.DiffPlex;

public static class ModuleInitializer
{
    #region enable

    [ModuleInitializer]
    public static void Initialize() =>
        VerifySyncfusion.Initialize();

    #endregion

    [ModuleInitializer]
    public static void InitializeOther()
    {
        VerifyDiffPlex.Initialize(OutputType.Compact);
        VerifierSettings.InitializePlugins();
    }
}