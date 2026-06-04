using Microsoft.AspNetCore.Mvc;
using TMPP.Models;
using TMPP.Structural.Decorator;
using TMPP.Behavioral.Strategy;
using TMPP.Structural.Facade;
using TMPP.Creational.AbstractFactory;
using TMPP.Creational.FactoryMethod;

namespace TMPP.Controllers.App;

public class CheckoutController : Controller
{
    [HttpGet("/Checkout")]
    public IActionResult Index()
    {
        ViewData["Title"] = "Checkout";
        return View("~/Views/Checkout/Index.cshtml");
    }

    [HttpPost("/Checkout")]
    public IActionResult ProcessCheckout(string customerType, bool expressShipping, bool insulatedPackaging, string cardNumber, string email)
    {
        var cartItems = Database.Cart.Items;
        if (cartItems.Count == 0) return RedirectToAction("Index", "Shop");

        var logs = new List<string>();
        logs.Add("Starting Unified Checkout Process...");

        // 1. Calculate Base Price
        double basePrice = cartItems.Sum(i => i.Price);
        
        // 2. Apply Strategy (Discount)
        var calculator = new DiscountCalculator();
        if (customerType == "Senior") calculator.SetStrategy(new SeniorDiscountStrategy());
        else if (customerType == "Loyalty") calculator.SetStrategy(new LoyaltyDiscountStrategy());
        
        double discountedPrice = calculator.CalculateTotal(basePrice);
        logs.Add($"Applied {customerType} discount. Price: ${discountedPrice.ToString("0.00")}");

        // 3. Apply Decorator (Shipping Options)
        string itemNames = string.Join(", ", cartItems.Select(i => i.Name));
        IOrder order = new BaseOrder(itemNames, discountedPrice);
        
        if (expressShipping) order = new ExpressShippingDecorator(order);
        if (insulatedPackaging) order = new InsulatedPackagingDecorator(order);
        
        double finalPrice = order.GetCost();
        logs.Add($"Applied Shipping Options. Final Cost: ${finalPrice.ToString("0.00")}");
        
        // 4. Abstract Factory (Packaging based on order requirements)
        bool needsCold = cartItems.Any(i => i.RequiresColdStorage);
        IOrderFactory factory = needsCold ? new UrgentOrderFactory() : new NormalOrderFactory();
        var packaging = factory.CreatePackaging();
        var courier = factory.CreateCourier();
        logs.Add($"Abstract Factory determined: {packaging.PackageItem()} and {courier.Deliver()}");

        // 5. Facade (Execute the transaction steps)
        var facade = new CheckoutFacade();
        logs.AddRange(facade.ProcessOrder("Cart Items", 1, cardNumber, finalPrice));
        
        // Deduct actual stock globally
        foreach(var item in cartItems) {
            item.StockQuantity--;
        }

        // 6. State (Move order to Processed)
        Database.LastOrder = new TMPP.Behavioral.State.OrderContext(); // Reset to Pending
        logs.Add("Order State initialized: Pending");
        logs.Add("State changed: " + Database.LastOrder.Process()); // Move to Processed

        // 7. Factory Method (Send Confirmation)
        var notificationFactory = new NotificationFactory();
        var emailNotification = notificationFactory.CreateNotification("Email");
        logs.Add(emailNotification.Send($"to {email}: Your order of ${finalPrice.ToString("0.00")} is confirmed."));

        // Clear Cart
        Database.Cart = new TMPP.Behavioral.Command.ShoppingCart();
        Database.CartInvoker = new TMPP.Behavioral.Command.CartInvoker();

        TempData["CheckoutLogs"] = logs.ToArray();
        return RedirectToAction("Success");
    }

    [HttpGet("/Checkout/Success")]
    public IActionResult Success()
    {
        ViewData["Title"] = "Order Successful";
        return View("~/Views/Checkout/Success.cshtml");
    }
}
