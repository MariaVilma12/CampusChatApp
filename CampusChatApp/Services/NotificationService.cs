namespace CampusChatApp.Services;

public class NotificationService
{
    private readonly List<string> _notifications = new();

    public IReadOnlyList<string> GetNotifications() => _notifications;

    public void AddNotification(string message)
    {
        if (!string.IsNullOrWhiteSpace(message))
            _notifications.Add(message);
    }
}
