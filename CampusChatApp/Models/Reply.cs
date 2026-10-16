namespace CampusChat.Models;

public class Reply
{
    public int Id { get; set; }

    // The post this reply belongs to
    public int PostId { get; set; }

    public int AuthorId { get; set; }

    public string AuthorName { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
