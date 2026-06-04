namespace TMPP.Behavioral.Observer;

public interface IObserver { string Update(string medication, bool inStock); }

public class CustomerObserver : IObserver
{
    private string _name;
    public CustomerObserver(string name) { _name = name; }
    
    public string Update(string medication, bool inStock) 
        => $"[{_name}] Notification: {medication} is now {(inStock ? "IN STOCK" : "OUT OF STOCK")}.";
}

public class StockNotifier
{
    private List<IObserver> _observers = new();
    private string _medication;
    private bool _inStock = false;
    
    public StockNotifier(string medication) { _medication = medication; }
    
    public void Attach(IObserver observer) { _observers.Add(observer); }
    public void Detach(IObserver observer) { _observers.Remove(observer); }
    
    public List<string> SetInStock(bool inStock)
    {
        _inStock = inStock;
        var logs = new List<string>();
        foreach (var observer in _observers)
        {
            logs.Add(observer.Update(_medication, _inStock));
        }
        return logs;
    }
}
