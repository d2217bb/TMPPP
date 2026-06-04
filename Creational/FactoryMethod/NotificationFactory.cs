namespace TMPP.Creational.FactoryMethod;

public interface INotification
{
    string Send(string message);
}

public class EmailNotification : INotification
{
    public string Send(string message) => $"[Email Sent] {message}";
}

public class SMSNotification : INotification
{
    public string Send(string message) => $"[SMS Sent] {message}";
}

public class NotificationFactory
{
    public INotification CreateNotification(string type)
    {
        return type.ToUpper() switch
        {
            "EMAIL" => new EmailNotification(),
            "SMS" => new SMSNotification(),
            _ => throw new ArgumentException("Invalid notification type")
        };
    }
}
