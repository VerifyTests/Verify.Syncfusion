public static class ModuleInitializer
{
    #region enable

    [ModuleInitializer]
    public static void Initialize()
    {
        VerifySyncfusion.Initialize();

        #endregion

        VerifyDiffPlex.Initialize();
    }
}