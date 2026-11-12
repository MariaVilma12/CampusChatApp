using CampusChatApp.Data;
using CampusChatApp.Models;

namespace CampusChatApp.Services;

public class ChatService
{
    public IReadOnlyList<DirectChat> GetAllChats() => AppStore.Chats;

    public DirectChat? GetChat(int id) => AppStore.Chats.FirstOrDefault(chat => chat.Id == id);

    public DirectChat AddChat(DirectChat chat)
    {
        ArgumentNullException.ThrowIfNull(chat);

        var existing = AppStore.Chats.FirstOrDefault(candidate =>
            (candidate.User1Id == chat.User1Id && candidate.User2Id == chat.User2Id) ||
            (candidate.User1Id == chat.User2Id && candidate.User2Id == chat.User1Id));
        if (existing is not null)
            return existing;

        chat.Id = AppStore.Chats.Count == 0 ? 1 : AppStore.Chats.Max(candidate => candidate.Id) + 1;
        AppStore.Chats.Add(chat);
        return chat;
    }

    public static void AddMessage(int chatId, ChatMessage message)
    {
        ArgumentNullException.ThrowIfNull(message);

        var chat = AppStore.Chats.FirstOrDefault(candidate => candidate.Id == chatId);
        if (chat is null)
            throw new InvalidOperationException("The chat could not be found.");

        chat.Messages.Add(message);
    }

    public IReadOnlyList<DirectChat> GetChatsForUser(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        return AppStore.Chats
            .Where(chat => chat.User1Id == user.Id || chat.User2Id == user.Id)
            .OrderByDescending(chat => chat.Messages.LastOrDefault()?.SentAt ?? DateTime.MinValue)
            .ToList();
    }
}
