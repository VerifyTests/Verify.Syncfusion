public static class ModuleInitializer
{
    #region InitializeOutputs

    [ModuleInitializer]
    public static void Initialize() =>
        VerifySyncfusion.Initialize(SyncfusionOutputs.Text | SyncfusionOutputs.Csv);

    #endregion

    [ModuleInitializer]
    public static void InitializeOther() =>
        VerifierSettings.UseSsimForPng(.7);
}
