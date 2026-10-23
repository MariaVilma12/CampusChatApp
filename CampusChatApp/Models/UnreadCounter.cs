namespace CampusChatApp.Models;

public class UnreadCounter
{
    public int UserId { get; set; }

    public int ThreadId { get; set; }

    // Number of unread posts
    public int UnreadPosts { get; set; }

    // Number of unread direct messages
    public int UnreadMessages { get; set; }
}
