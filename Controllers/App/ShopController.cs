using Microsoft.AspNetCore.Mvc;
using TMPP.Models;
using TMPP.Behavioral.Command;
using TMPP.Behavioral.Observer;

namespace TMPP.Controllers.App;

public class ShopController : Controller
{
    [HttpGet("/")]
    public IActionResult Index()
    {
        ViewData["Title"] = "Pharmacy Shop";
        return View("~/Views/Shop/Index.cshtml", Database.Products);
    }

    [HttpPost("/Shop/AddToCart")]
    public IActionResult AddToCart(string productId)
    {
        var product = Database.Products.FirstOrDefault(p => p.Id == productId);
        if (product != null && product.StockQuantity > 0)
        {
            var command = new AddToCartCommand(Database.Cart, product);
            Database.CartInvoker.ExecuteCommand(command);
            TempData["Message"] = $"Added {product.Name} to cart.";
        }
        else if (product != null && product.StockQuantity == 0)
        {
            TempData["Error"] = $"{product.Name} is out of stock.";
        }
        return RedirectToAction("Index");
    }

    [HttpPost("/Shop/UndoCart")]
    public IActionResult UndoCart()
    {
        var result = Database.CartInvoker.UndoLastCommand();
        TempData["Message"] = result;
        return RedirectToAction("Index");
    }

    [HttpPost("/Shop/Subscribe")]
    public IActionResult Subscribe(string productId, string customerName)
    {
        if (Database.Notifiers.TryGetValue(productId, out var notifier))
        {
            notifier.Attach(new CustomerObserver(customerName));
            TempData["Message"] = $"Subscribed {customerName} to stock alerts for {productId}.";
        }
        return RedirectToAction("Index");
    }
}
