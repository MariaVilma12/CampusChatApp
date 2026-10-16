using CampusChatApp.Models;

namespace CampusChatApp.Services;

public class UserService
{
    private readonly List<User> _users = new();

    public IReadOnlyList<User> GetUsers() => _users;

    public User AddUser(User user)
    {
        user.Id = _users.Count == 0 ? 1 : _users.Max(u => u.Id) + 1;
        _users.Add(user);
        return user;
    }
}
