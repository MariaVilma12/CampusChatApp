namespace CampusChat.Models;

public class Thread
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    // True if the thread is private
    public bool IsPrivate { get; set; }

    // Required only for private threads
    public string? AccessCode { get; set; }

    // All posts inside the thread
    public List<Post> Posts { get; set; } = new();
}