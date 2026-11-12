using CampusChatApp.Models;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using ThreadModel = CampusChatApp.Models.Thread;

namespace CampusChatApp.Services;

public class SessionService(
    ProtectedSessionStorage sessionStorage,
    UserService userService)
{
    private const string CurrentUserKey = "campus-chat-demo-profile-v1";

    public record SessionTicket(int UserId);

    private User? CurrentUser { get; set; }
    public event Action<User?>? AuthenticationStateChanged;

    public async Task<bool> SelectProfileAsync(int userId)
    {
        var user = userService.GetUser(userId);
        if (user is null)
            return false;

        CurrentUser = user;
        await sessionStorage.SetAsync(CurrentUserKey, new SessionTicket(userId));
        AuthenticationStateChanged?.Invoke(CurrentUser);
        return true;
    }

    public async Task<User?> GetCurrentUserAsync()
    {
        var result = await sessionStorage.GetAsync<SessionTicket>(CurrentUserKey);
        var user = result.Success && result.Value is not null
            ? userService.GetUser(result.Value.UserId)
            : null;

        if (CurrentUser != user)
        {
            CurrentUser = user;
            AuthenticationStateChanged?.Invoke(CurrentUser);
        }

        if (result.Success && user is null)
            await sessionStorage.DeleteAsync(CurrentUserKey);

        return CurrentUser;
    }

    public bool IsAuditUser(User? user) => user?.IsAuditUser == true;

    public bool CanAccessThread(ThreadModel? thread, User? user = null)
    {
        if (thread is null)
            return false;

        var currentUser = user ?? CurrentUser;
        if (currentUser?.IsAuditUser == true || !thread.IsPrivate)
            return true;

        return currentUser is not null && currentUser.JoinedThreadIds.Contains(thread.Id);
    }

    public bool CanAccessChat(DirectChat? chat, User? user = null)
    {
        if (chat is null)
            return false;

        var currentUser = user ?? CurrentUser;
        return currentUser?.IsAuditUser == true ||
            (currentUser is not null && (chat.User1Id == currentUser.Id || chat.User2Id == currentUser.Id));
    }

    public async Task LogoutAsync()
    {
        var wasSignedIn = CurrentUser is not null;
        CurrentUser = null;
        await sessionStorage.DeleteAsync(CurrentUserKey);
        if (wasSignedIn)
            AuthenticationStateChanged?.Invoke(null);
    }
}
