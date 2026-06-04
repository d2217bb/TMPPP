namespace TMPP.Structural.Facade;

public class InventorySubsystem { public string DeductStock(string item, int qty) => $"Deducted {qty} of {item} from stock."; }
public class PaymentSubsystem { public string ChargeCard(string card, double amount) => $"Charged ${amount} to card {card}."; }
public class DeliverySubsystem { public string ScheduleDelivery(string item) => $"Scheduled delivery for {item}."; }

public class CheckoutFacade
{
    private InventorySubsystem _inventory = new();
    private PaymentSubsystem _payment = new();
    private DeliverySubsystem _delivery = new();

    public List<string> ProcessOrder(string item, int quantity, string cardNumber, double amount)
    {
        var logs = new List<string>();
        logs.Add("Facade starting complex checkout process...");
        logs.Add(_inventory.DeductStock(item, quantity));
        logs.Add(_payment.ChargeCard(cardNumber, amount));
        logs.Add(_delivery.ScheduleDelivery(item));
        logs.Add("Facade finished checkout successfully.");
        return logs;
    }
}
