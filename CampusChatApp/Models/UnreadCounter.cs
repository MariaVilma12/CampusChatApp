namespace CampusChatApp.Models;

public class UnreadCounter
{
    public int ThreadId { get; set; }
    public int UserId { get; set; }
    public int Count { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
