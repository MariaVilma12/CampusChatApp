namespace CampusChatApp.Models;

public class DirectChat
{
    public int Id { get; set; }

    // First participant
    public int User1Id { get; set; }

    // Second participant
    public int User2Id { get; set; }

    // Conversation messages
    public List<ChatMessage> Messages { get; set; } = new();
}