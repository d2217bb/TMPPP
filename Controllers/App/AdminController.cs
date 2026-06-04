using Microsoft.AspNetCore.Mvc;
using TMPP.Models;
using TMPP.Structural.Proxy;
using TMPP.Structural.Adapter;
using TMPP.Creational.Singleton;

namespace TMPP.Controllers.App;

public class AdminController : Controller
{
    [HttpGet("/Admin")]
    public IActionResult Index()
    {
        ViewData["Title"] = "Pharmacy Admin";
        ViewData["Config"] = PharmacyConfigManager.Instance;
        return View("~/Views/Admin/Index.cshtml", Database.Products);
    }

    [HttpPost("/Admin/AddStock")]
    public IActionResult AddStock(string role, string productId, int quantity)
    {
        var logs = new List<string>();
        IInventoryAccess proxy = new InventoryAccessProxy(role);
        string result = proxy.AddStock(productId, quantity);
        logs.Add($"[{role}] AddStock attempt: {result}");
        
        if (result.StartsWith("Added")) {
            var product = Database.Products.FirstOrDefault(p => p.Id == productId);
            if (product != null) product.StockQuantity += quantity;
        }

        TempData["AdminLogs"] = logs.ToArray();
        return RedirectToAction("Index");
    }

    [HttpPost("/Admin/OrderLegacy")]
    public IActionResult OrderLegacy(string productId, int quantity)
    {
        var logs = new List<string>();
        IInventorySystem inventory = new InventoryAdapter(new LegacySupplierAPI());
        logs.Add(inventory.OrderStock(productId, quantity));
        
        // Simulate immediate delivery for demo
        var product = Database.Products.FirstOrDefault(p => p.Id == productId);
        if (product != null) product.StockQuantity += quantity;
        
        TempData["AdminLogs"] = logs.ToArray();
        return RedirectToAction("Index");
    }
    [HttpPost("/Admin/AddProduct")]
    public IActionResult AddProduct(string name, double price, int stock, bool cold)
    {
        var logs = new List<string>();
        string id = "P" + (Database.Products.Count + 1);
        var product = new Product {
            Id = id,
            Name = name,
            Price = price,
            StockQuantity = stock,
            RequiresColdStorage = cold
        };
        Database.Products.Add(product);
        logs.Add($"New product '{name}' added with ID {id}.");
        
        TempData["AdminLogs"] = logs.ToArray();
        return RedirectToAction("Index");
    }
    
    [HttpPost("/Admin/SaveConfig")]
    public IActionResult SaveConfig(string name, string hours)
    {
        var config = PharmacyConfigManager.Instance;
        config.PharmacyName = name;
        config.OpeningHour = hours;
        return RedirectToAction("Index");
    }
}
