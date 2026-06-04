namespace TMPP.Structural.Proxy;

public interface IInventoryAccess
{
    string AddStock(string item, int quantity);
}

public class RealInventoryAccess : IInventoryAccess
{
    public string AddStock(string item, int quantity) => $"Added {quantity} of {item} to the inventory database.";
}

public class InventoryAccessProxy : IInventoryAccess
{
    private RealInventoryAccess? _realAccess;
    private string _userRole;

    public InventoryAccessProxy(string userRole)
    {
        _userRole = userRole;
    }

    public string AddStock(string item, int quantity)
    {
        if (_userRole != "Pharmacist" && _userRole != "Admin")
        {
            return $"Access Denied: User role '{_userRole}' does not have permission to add stock.";
        }
        
        if (_realAccess == null)
        {
            _realAccess = new RealInventoryAccess();
        }
        
        return _realAccess.AddStock(item, quantity);
    }
}
