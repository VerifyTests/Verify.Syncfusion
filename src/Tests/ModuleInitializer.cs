using Syncfusion.Licensing;
using VerifyTests;

public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Initialize()
    {
        var key = Environment.GetEnvironmentVariable("SyncfusionLicense");
        if (key != null)
        {
            SyncfusionLicenseProvider.RegisterLicense(key);
        }
        
        VerifySyncfusion.Initialize();
    }
}