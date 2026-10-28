using CampusChatApp.Data;
using CampusChatApp.Models;

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

    public User CreateProfile(string displayName, string about, string courseStudying, int studyYear)
    {
        var trimmedName = (displayName ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(trimmedName))
            throw new ArgumentException("Display name is required.", nameof(displayName));

        var user = new User
        {
            Id = SeedData.Users.Count == 0 ? 1 : SeedData.Users.Max(u => u.Id) + 1,
            DisplayName = trimmedName,
            About = about?.Trim() ?? string.Empty,
            CourseStudying = courseStudying?.Trim() ?? string.Empty,
            StudyYear = studyYear,
            JoinedThreadIds = new List<int>(),
            UnreadThreadCounts = new Dictionary<int, int>()
        };

        SeedData.Users.Add(user);
        return user;
    }

    public User UpdateProfile(User user, string displayName, string about, string courseStudying, int studyYear)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user));

        user.DisplayName = (displayName ?? string.Empty).Trim();
        user.About = about?.Trim() ?? string.Empty;
        user.CourseStudying = courseStudying?.Trim() ?? string.Empty;
        user.StudyYear = studyYear;

        if (string.IsNullOrWhiteSpace(user.DisplayName))
            throw new ArgumentException("Display name is required.", nameof(displayName));

        return user;
    }
}