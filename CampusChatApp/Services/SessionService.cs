
using CampusChat.Services;
using CampusChatApp.Models;
using Microsoft.JSInterop;
using ThreadModel = CampusChatApp.Models.Thread;

namespace CampusChatApp.Services;

public class SessionService
{
    private const string BrowserStorageKeyPrefix = "campus-chat-current-user";

    private readonly IJSRuntime _jsRuntime;
    private readonly UserService _userService;

    public User? CurrentUser { get; private set; }

    public SessionService(
        IJSRuntime jsRuntime,
        UserService userService)
    {
        _jsRuntime = jsRuntime;
        _userService = userService;
    }

    private async Task<string> GetStorageKeyAsync()
    {
        try
        {
            var windowId = await _jsRuntime.InvokeAsync<string?>("eval", "window.name || ''");
            if (string.IsNullOrWhiteSpace(windowId))
            {
                windowId = Guid.NewGuid().ToString("N");
                await _jsRuntime.InvokeVoidAsync("eval", $"window.name = '{windowId}';");
            }

            return $"{BrowserStorageKeyPrefix}-{windowId}";
        }
        catch
        {
            return BrowserStorageKeyPrefix;
        }
    }

    public async Task SetCurrentUserAsync(User user)
    {
        CurrentUser = user;

        try
        {
            var key = await GetStorageKeyAsync();
            await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", key, user.Id.ToString());
        }
        catch
        {
            // Ignore JS interop failures during prerender/static setup.
        }
    }

    public async Task<User?> GetCurrentUserAsync()
    {
        try
        {
            var key = await GetStorageKeyAsync();
            var storedValue = await _jsRuntime.InvokeAsync<string?>("sessionStorage.getItem", key);
            CurrentUser = string.IsNullOrWhiteSpace(storedValue)
                ? null
                : _userService.GetUser(int.Parse(storedValue));
        }
        catch
        {
            // Ignore JS interop failures during prerender/static setup.
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

        try
        {
            var key = await GetStorageKeyAsync();
            await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", key);
        }
        catch
        {
            // Ignore JS interop failures during prerender/static setup.
        }
    }
}