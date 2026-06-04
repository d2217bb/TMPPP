namespace TMPP.Models;

public static class Database
{
    public static List<Product> Products { get; set; } = new()
    {
        new Product { Id = "P1", Name = "Paracetamol 500mg", Price = 15.0, StockQuantity = 100, RequiresColdStorage = false },
        new Product { Id = "P2", Name = "Insulin Glargine", Price = 120.0, StockQuantity = 10, RequiresColdStorage = true },
        new Product { Id = "P3", Name = "Ozempic", Price = 350.0, StockQuantity = 0, RequiresColdStorage = true },
        new Product { Id = "P4", Name = "Aspirin Cardio", Price = 20.0, StockQuantity = 50, RequiresColdStorage = false }
    };
    
    // Global Cart (Command pattern)
    public static TMPP.Behavioral.Command.ShoppingCart Cart { get; set; } = new();
    public static TMPP.Behavioral.Command.CartInvoker CartInvoker { get; set; } = new();
    
    // Global Order State Tracking (State pattern)
    public static TMPP.Behavioral.State.OrderContext LastOrder { get; set; } = new();

    // Observers for out-of-stock items
    public static Dictionary<string, TMPP.Behavioral.Observer.StockNotifier> Notifiers { get; set; } = new();
    
    static Database()
    {
        Notifiers["P3"] = new TMPP.Behavioral.Observer.StockNotifier("Ozempic");
    }
}
