using CampusChatApp.Data;
using CampusChatApp.Models;

namespace CampusChatApp.Services;

public class ChatService
{
    public static IReadOnlyList<DirectChat> GetAllChats()
    {
        return AppStore.Chats;
    }

    public static DirectChat? GetChat(int id)
    {
        return AppStore.Chats.FirstOrDefault(c => c.Id == id);
    }

    public static DirectChat AddChat(DirectChat chat)
    {
        chat.Id = AppStore.Chats.Count == 0 ? 1 : AppStore.Chats.Max(c => c.Id) + 1;
        AppStore.Chats.Add(chat);
        return chat;
    }

    public static void AddMessage(int chatId, ChatMessage message)
    {
        var chat = AppStore.Chats.FirstOrDefault(c => c.Id == chatId);
        if (chat is null) return;
        chat.Messages.Add(message);
    }

    public static IReadOnlyList<DirectChat> GetChatsForUser(User user)
    {
        return AppStore.Chats
            .Where(c => c.User1Id == user.Id || c.User2Id == user.Id)
            .OrderByDescending(c => c.Messages.LastOrDefault()?.SentAt ?? DateTime.MinValue)
            .ToList();
    }
}
