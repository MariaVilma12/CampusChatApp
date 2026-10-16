namespace CampusChatApp.Models;

public class Post
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public User Author { get; set; } = new();
    public List<Reply> Replies { get; set; } = new();
}
