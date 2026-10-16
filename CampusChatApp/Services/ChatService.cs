using CampusChatApp.Models;

namespace CampusChatApp.Services;

public class ChatService
{
    private readonly List<DirectChat> _chats = new();

    public IReadOnlyList<DirectChat> GetChats() => _chats;

    public DirectChat AddChat(DirectChat chat)
    {
        chat.Id = _chats.Count == 0 ? 1 : _chats.Max(c => c.Id) + 1;
        _chats.Add(chat);
        return chat;
    }

    public void AddMessage(int chatId, ChatMessage message)
    {
        var chat = _chats.FirstOrDefault(c => c.Id == chatId);
        if (chat is null) return;

        chat.Messages.Add(message);
        chat.UpdatedAt = DateTime.UtcNow;
    }
}
