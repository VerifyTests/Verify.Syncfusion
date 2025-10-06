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
        ApplySyncfusionLicense();
        VerifyDiffPlex.Initialize(OutputType.Compact);
        VerifierSettings.InitializePlugins();
    }

    static void ApplySyncfusionLicense()
    {
        var license = Environment.GetEnvironmentVariable("SyncfusionLicense");
        if (license == null)
        {
            throw new("Expected a `SyncfusionLicense` environment variable");
        }

        SyncfusionLicenseProvider.RegisterLicense(license);
    }
}