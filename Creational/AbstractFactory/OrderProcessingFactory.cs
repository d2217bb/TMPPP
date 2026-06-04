namespace TMPP.Creational.AbstractFactory;

public interface IPackaging { string PackageItem(); }
public interface ICourier { string Deliver(); }

public interface IOrderFactory
{
    IPackaging CreatePackaging();
    ICourier CreateCourier();
}

public class StandardPackaging : IPackaging { public string PackageItem() => "Packed in a standard cardboard box."; }
public class StandardCourier : ICourier { public string Deliver() => "Delivered via standard post within 3 days."; }

public class NormalOrderFactory : IOrderFactory
{
    public IPackaging CreatePackaging() => new StandardPackaging();
    public ICourier CreateCourier() => new StandardCourier();
}

public class InsulatedPackaging : IPackaging { public string PackageItem() => "Packed in an insulated cold box with dry ice."; }
public class ExpressCourier : ICourier { public string Deliver() => "Delivered via express courier within 2 hours."; }

public class UrgentOrderFactory : IOrderFactory
{
    public IPackaging CreatePackaging() => new InsulatedPackaging();
    public ICourier CreateCourier() => new ExpressCourier();
}
