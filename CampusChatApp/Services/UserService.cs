using CampusChat.Data;
using CampusChat.Models;

namespace CampusChat.Services;

public class UserService
{
    /// <summary>
    /// Returns all users.
    /// </summary>
    public IReadOnlyList<User> GetAllUsers()
    {
        return SeedData.Users;
    }

    /// <summary>
    /// Returns a user by ID.
    /// Returns null if no matching user exists.
    /// </summary>
    public User? GetUser(int id)
    {
        return SeedData.Users.FirstOrDefault(u => u.Id == id);
    }
}