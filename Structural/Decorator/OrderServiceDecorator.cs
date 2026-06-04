namespace TMPP.Structural.Decorator;

public interface IOrder
{
    string GetDescription();
    double GetCost();
}

public class BaseOrder : IOrder
{
    private string _item;
    private double _price;
    public BaseOrder(string item, double price) { _item = item; _price = price; }
    public string GetDescription() => $"Order items: {_item}";
    public double GetCost() => _price;
}

public abstract class OrderDecorator : IOrder
{
    protected IOrder _order;
    public OrderDecorator(IOrder order) { _order = order; }
    public virtual string GetDescription() => _order.GetDescription();
    public virtual double GetCost() => _order.GetCost();
}

public class ExpressShippingDecorator : OrderDecorator
{
    public ExpressShippingDecorator(IOrder order) : base(order) { }
    public override string GetDescription() => _order.GetDescription() + ", with Express Shipping";
    public override double GetCost() => _order.GetCost() + 5.0;
}

public class InsulatedPackagingDecorator : OrderDecorator
{
    public InsulatedPackagingDecorator(IOrder order) : base(order) { }
    public override string GetDescription() => _order.GetDescription() + ", with Insulated Packaging";
    public override double GetCost() => _order.GetCost() + 3.5;
}
