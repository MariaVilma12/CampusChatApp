namespace CampusChatApp.Models;

public class Reply
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public User Author { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
