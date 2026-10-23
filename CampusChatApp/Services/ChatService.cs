using CampusChat.Models;

namespace CampusChat.Services;

public class ChatService
{
    private readonly List<DirectChat> _chats = new();

    /// <summary>
    /// Returns every direct chat.
    /// Returns an empty list if there are no chats.
    /// </summary>
    public IReadOnlyList<DirectChat> GetAllChats()
    {
        return _chats;
    }

    /// <summary>
    /// Returns a chat by ID.
    /// Returns null if it does not exist.
    /// </summary>
    public DirectChat? GetChat(int id)
    {
        return _chats.FirstOrDefault(c => c.Id == id);
    }
}
