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
        var culture = CultureInfo.GetCultureInfo("en-US");
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;
        VerifierSettings.UseSsimForPng(.7);
        VerifierSettings.InitializePlugins();
    }
}
