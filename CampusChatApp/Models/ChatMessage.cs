namespace CampusChatApp.Models;

public class ChatMessage
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public User Sender { get; set; } = new();
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; }
}
