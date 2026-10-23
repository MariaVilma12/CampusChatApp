using CampusChat.Models;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace CampusChat.Services;

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

    public async Task LogoutAsync()
    {
        CurrentUser = null;
        await _storage.DeleteAsync(SessionKey);
    }
}