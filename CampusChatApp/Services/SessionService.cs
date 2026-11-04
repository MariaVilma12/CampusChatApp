
using CampusChatApp.Models;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using ThreadModel = CampusChatApp.Models.Thread;

namespace CampusChatApp.Services;

public class SessionService(
    ProtectedSessionStorage sessionStorage,
    UserService userService)
{
    private const string CurrentUserKey = "campus-chat-current-user";

    private User? CurrentUser { get; set; }

    public async Task SetCurrentUserAsync(User user)
    {
        CurrentUser = user;
        await sessionStorage.SetAsync(CurrentUserKey, user.Id);
    }

    public async Task<User?> GetCurrentUserAsync()
    {
        var result = await sessionStorage.GetAsync<int>(CurrentUserKey);

        if (!result.Success || result.Value == 0)
        {
            CurrentUser = null;
            return null;
        }

        CurrentUser = userService.GetUser(result.Value);
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

        if (!thread.IsPrivate)
            return true;

        return currentUser != null && currentUser.JoinedThreadIds.Contains(thread.Id);
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
        await sessionStorage.DeleteAsync(CurrentUserKey);
    }
}