namespace CampusChat.Models;

public class ChatMessage
{
    public int Id { get; set; }

    public int SenderId { get; set; }

    public string SenderName { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;

    public DateTime SentAt { get; set; } = DateTime.Now;
}
