using CampusChatApp.Models;

namespace CampusChatApp.Data;

public static class SeedChats
{
    public static List<DirectChat> Chats { get; } = new()
    {
        new DirectChat
        {
            Id = 1,
            ParticipantOne = SeedUsers.Users[0],
            ParticipantTwo = SeedUsers.Users[1],
            Messages = new List<ChatMessage>
            {
                new ChatMessage { Id = 1, Content = "Are you free for the student mixer?", Sender = SeedUsers.Users[0], IsRead = true },
                new ChatMessage { Id = 2, Content = "Yes! I would love to join.", Sender = SeedUsers.Users[1], IsRead = true }
            }
        }
    };

    public static void Initialize() { }
}
