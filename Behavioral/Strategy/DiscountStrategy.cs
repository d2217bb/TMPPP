namespace TMPP.Behavioral.Strategy;

public interface IDiscountStrategy { double ApplyDiscount(double price); }

public class NoDiscountStrategy : IDiscountStrategy { public double ApplyDiscount(double price) => price; }
public class SeniorDiscountStrategy : IDiscountStrategy { public double ApplyDiscount(double price) => price * 0.8; }
public class LoyaltyDiscountStrategy : IDiscountStrategy { public double ApplyDiscount(double price) => price * 0.9; }

public class DiscountCalculator
{
    private IDiscountStrategy _strategy;
    
    public DiscountCalculator() { _strategy = new NoDiscountStrategy(); }
    
    public void SetStrategy(IDiscountStrategy strategy) { _strategy = strategy; }
    
    public double CalculateTotal(double price) => _strategy.ApplyDiscount(price);
}
