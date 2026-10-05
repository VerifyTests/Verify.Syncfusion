public static class ModuleInitializer
{
    #region InitializeOutputs

    [ModuleInitializer]
    public static void Initialize()
    {
        VerifySyncfusion.Initialize();

        // For every test: pages are not rendered, so no png is verified
        VerifierSettings.ExcludeDerivedTargets("png");
    }

    #endregion

    [ModuleInitializer]
    public static void InitializeOther() =>
        VerifierSettings.UseSsimForPng(.7);
}
