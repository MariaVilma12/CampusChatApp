using CampusChatApp.Data;
using CampusChatApp.Helpers;
using CampusChatApp.Models;

namespace CampusChatApp.Services;

public class UserService
{
    public static IReadOnlyList<User> GetAllUsers() => AppStore.Users;

    public User? GetUser(int id) => AppStore.Users.FirstOrDefault(u => u.Id == id);

    public static User CreateProfile(string displayName, string about, string courseStudying, int studyYear, string avatarColor = "#4f46e5", bool isAuditUser = false)
    {
        var profile = ValidateAndNormalizeProfile(displayName, about, courseStudying, studyYear, avatarColor);

        var user = new User
        {
            Id = GetNextUserId(),
            DisplayName = profile.displayName,
            About = profile.about,
            CourseStudying = profile.courseStudying,
            StudyYear = profile.studyYear,
            AvatarColor = profile.avatarColor,
            IsAuditUser = isAuditUser,
            JoinedThreadIds = new List<int>(),
            UnreadThreadCounts = new Dictionary<int, int>()
        };

        AppStore.Users.Add(user);
        return user;
    }

    public static User UpdateProfile(User user, string displayName, string about, string courseStudying, int studyYear, string avatarColor = "#4f46e5", bool? isAuditUser = null)
    {
        ArgumentNullException.ThrowIfNull(user);

        var profile = ValidateAndNormalizeProfile(displayName, about, courseStudying, studyYear, avatarColor);

        user.DisplayName = profile.displayName;
        user.About = profile.about;
        user.CourseStudying = profile.courseStudying;
        user.StudyYear = profile.studyYear;
        user.AvatarColor = profile.avatarColor;

        if (isAuditUser.HasValue)
            user.IsAuditUser = isAuditUser.Value;

        return user;
    }

    private static int GetNextUserId() => AppStore.Users.Count == 0 ? 1 : AppStore.Users.Max(u => u.Id) + 1;

    private static (string displayName, string about, string courseStudying, int studyYear, string avatarColor) ValidateAndNormalizeProfile(
        string? displayName,
        string? about,
        string? courseStudying,
        int studyYear,
        string? avatarColor)
    {
        var normalizedName = (displayName ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(normalizedName))
            throw new ArgumentException("Display name is required.", nameof(displayName));

        if (normalizedName.Length > Constants.MaxDisplayNameLength)
            throw new ArgumentException($"Display name must be {Constants.MaxDisplayNameLength} characters or fewer.", nameof(displayName));

        var safeColor = string.IsNullOrWhiteSpace(avatarColor)
            ? "#4f46e5"
            : avatarColor.Trim();

        return (
            normalizedName,
            about?.Trim() ?? string.Empty,
            courseStudying?.Trim() ?? string.Empty,
            studyYear,
            safeColor);
    }
}