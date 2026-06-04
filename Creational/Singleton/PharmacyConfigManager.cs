namespace TMPP.Creational.Singleton;

public sealed class PharmacyConfigManager
{
    private static readonly Lazy<PharmacyConfigManager> _instance = new(() => new PharmacyConfigManager());
    
    public static PharmacyConfigManager Instance => _instance.Value;
    
    public string PharmacyName { get; set; } = "Default Pharma";
    public string OpeningHour { get; set; } = "08:00 - 20:00";
    
    private PharmacyConfigManager() { }
}
