namespace CampusChat.Models;

public class Post
{
    public int Id { get; set; }

    public string Text { get; set; } = string.Empty;

    public int AuthorId { get; set; }

    public string AuthorName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // Replies to this post
    public List<Reply> Replies { get; set; } = new();
}
