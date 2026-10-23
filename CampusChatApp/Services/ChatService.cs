using CampusChatApp.Data;
using CampusChatApp.Models;

namespace CampusChatApp.Services;

public class ChatService
{
    public IReadOnlyList<DirectChat> GetAllChats()
    {
        return SeedData.Chats;
    }

    public DirectChat? GetChat(int id)
    {
        return SeedData.Chats.FirstOrDefault(c => c.Id == id);
    }

    public DirectChat AddChat(DirectChat chat)
    {
        chat.Id = SeedData.Chats.Count == 0 ? 1 : SeedData.Chats.Max(c => c.Id) + 1;
        SeedData.Chats.Add(chat);
        return chat;
    }

    public void AddMessage(int chatId, ChatMessage message)
    {
        var chat = SeedData.Chats.FirstOrDefault(c => c.Id == chatId);
        if (chat is null) return;
        chat.Messages.Add(message);
    }
}
