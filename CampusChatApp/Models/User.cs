namespace CampusChatApp.Models;

public class User
{
    public int Id { get; set; }

    public string DisplayName { get; set; } = string.Empty;

    public string About { get; set; } = string.Empty;

    // Example: Blue, Green, Red
    public string AvatarColor { get; set; } = "Blue";

    public int StudyYear { get; set; }

    // True only for the Auditor account
    public bool IsAuditUser { get; set; }

    // Stores the IDs of threads the user has joined
    public List<int> JoinedThreadIds { get; set; } = new();

    // Unread post counts per joined thread
    public Dictionary<int, int> UnreadThreadCounts { get; set; } = new();
}