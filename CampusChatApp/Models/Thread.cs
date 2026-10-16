namespace CampusChatApp.Models;

public class Thread
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public bool IsPrivate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<Post> Posts { get; set; } = new();
    public List<User> Members { get; set; } = new();
}
