
using CampusChat.Services;
using CampusChatApp.Models;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using ThreadModel = CampusChatApp.Models.Thread;

namespace CampusChatApp.Services;

public class SessionService
{
    private const string SessionKey = "CurrentUserId";

    private readonly ProtectedSessionStorage _storage;
    private readonly UserService _userService;

    public User? CurrentUser { get; private set; }

    public SessionService(
        ProtectedSessionStorage storage,
        UserService userService)
    {
        _storage = storage;
        _userService = userService;
    }

    public async Task SetCurrentUserAsync(User user)
    {
        CurrentUser = user;
        await _storage.SetAsync(SessionKey, user.Id);
    }

    public async Task<User?> GetCurrentUserAsync()
    {
        if (CurrentUser != null)
            return CurrentUser;

        var result = await _storage.GetAsync<int>(SessionKey);

        if (result.Success)
        {
            CurrentUser = _userService.GetUser(result.Value);
        }

        return CurrentUser;
    }

    public bool IsAuditUser(User? user) => user?.IsAuditUser == true;

    public bool CanAccessThread(ThreadModel? thread, User? user = null)
    {
        if (thread is null)
            return false;

        var currentUser = user ?? CurrentUser;
        if (currentUser?.IsAuditUser == true)
            return true;

        return !thread.IsPrivate || currentUser != null && currentUser.JoinedThreadIds.Contains(thread.Id);
    }

    public bool CanAccessChat(DirectChat? chat, User? user = null)
    {
        if (chat is null)
            return false;

        var currentUser = user ?? CurrentUser;
        if (currentUser?.IsAuditUser == true)
            return true;

        return currentUser != null && (chat.User1Id == currentUser.Id || chat.User2Id == currentUser.Id);
    }

    public async Task LogoutAsync()
    {
        CurrentUser = null;
        await _storage.DeleteAsync(SessionKey);
    }
}