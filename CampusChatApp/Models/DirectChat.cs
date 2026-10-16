namespace CampusChatApp.Models;

public class DirectChat
{
    public int Id { get; set; }
    public User ParticipantOne { get; set; } = new();
    public User ParticipantTwo { get; set; } = new();
    public List<ChatMessage> Messages { get; set; } = new();
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
