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
        VerifierSettings.UseSsimForPng(.7);
        VerifierSettings.InitializePlugins();
    }
}
