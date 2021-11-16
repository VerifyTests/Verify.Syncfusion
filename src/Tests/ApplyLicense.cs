using Syncfusion.Licensing;

public static class ApplyLicense
{
    [ModuleInitializer]
    public static void Initialize()
    {
        var key = Environment.GetEnvironmentVariable("SyncfusionLicense");
        if (key != null)
        {
            SyncfusionLicenseProvider.RegisterLicense(key);
        }
    }
}