namespace TMPP.Behavioral.Command;

public class ShoppingCart
{
    public List<TMPP.Models.Product> Items { get; private set; } = new();
    
    public string AddItem(TMPP.Models.Product item)
    {
        Items.Add(item);
        return $"Added {item.Name} to cart. Total items: {Items.Count}";
    }
    
    public string RemoveItem(TMPP.Models.Product item)
    {
        Items.Remove(item);
        return $"Removed {item.Name} from cart. Total items: {Items.Count}";
    }
}

public interface ICommand
{
    string Execute();
    string Undo();
}

public class AddToCartCommand : ICommand
{
    private ShoppingCart _cart;
    private TMPP.Models.Product _item;
    
    public AddToCartCommand(ShoppingCart cart, TMPP.Models.Product item)
    {
        _cart = cart;
        _item = item;
    }
    
    public string Execute() => _cart.AddItem(_item);
    public string Undo() => _cart.RemoveItem(_item);
}

public class CartInvoker
{
    private Stack<ICommand> _history = new();
    
    public string ExecuteCommand(ICommand cmd)
    {
        _history.Push(cmd);
        return cmd.Execute();
    }
    
    public string UndoLastCommand()
    {
        if (_history.Count > 0)
        {
            var cmd = _history.Pop();
            return cmd.Undo();
        }
        return "Nothing to undo.";
    }
}
