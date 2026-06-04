namespace TMPP.Behavioral.State;

public interface IOrderState
{
    string Process(OrderContext context);
    string Deliver(OrderContext context);
    string Cancel(OrderContext context);
}

public class OrderContext
{
    public IOrderState State { get; set; }
    public OrderContext() { State = new PendingState(); }
    
    public string Process() => State.Process(this);
    public string Deliver() => State.Deliver(this);
    public string Cancel() => State.Cancel(this);
}

public class PendingState : IOrderState
{
    public string Process(OrderContext context)
    {
        context.State = new ProcessedState();
        return "Order verified and processed.";
    }
    public string Deliver(OrderContext context) => "Cannot deliver a pending order. Must be processed first.";
    public string Cancel(OrderContext context)
    {
        context.State = new CancelledState();
        return "Order cancelled successfully.";
    }
}

public class ProcessedState : IOrderState
{
    public string Process(OrderContext context) => "Order is already processed.";
    public string Deliver(OrderContext context)
    {
        context.State = new DeliveredState();
        return "Order handed to courier and delivered.";
    }
    public string Cancel(OrderContext context)
    {
        context.State = new CancelledState();
        return "Processed order cancelled. Stock returned.";
    }
}

public class DeliveredState : IOrderState
{
    public string Process(OrderContext context) => "Cannot process an already delivered order.";
    public string Deliver(OrderContext context) => "Order is already delivered.";
    public string Cancel(OrderContext context) => "Cannot cancel a delivered order. Initiate a return instead.";
}

public class CancelledState : IOrderState
{
    public string Process(OrderContext context) => "Cancelled order cannot be processed.";
    public string Deliver(OrderContext context) => "Cancelled order cannot be delivered.";
    public string Cancel(OrderContext context) => "Order is already cancelled.";
}
