namespace TMPP.Structural.Adapter;

public interface IInventorySystem
{
    string CheckStock(string item, int quantity);
    string OrderStock(string item, int quantity);
}

// Legacy third-party API that we cannot change
public class LegacySupplierAPI
{
    public string VerifyAvailability(string productCode) => $"Verified availability for {productCode}";
    public string PlaceBulkOrder(string productCode, int bulkAmount) => $"Ordered {bulkAmount} boxes of {productCode}";
}

// Adapter
public class InventoryAdapter : IInventorySystem
{
    private readonly LegacySupplierAPI _legacyApi;
    public InventoryAdapter(LegacySupplierAPI legacyApi) { _legacyApi = legacyApi; }

    public string CheckStock(string item, int quantity)
    {
        return $"Adapter converting: {_legacyApi.VerifyAvailability(item)} (Requested: {quantity})";
    }

    public string OrderStock(string item, int quantity)
    {
        int boxes = (int)Math.Ceiling(quantity / 10.0);
        return $"Adapter converting: {_legacyApi.PlaceBulkOrder(item, boxes)} (Total individual units: {quantity})";
    }
}
