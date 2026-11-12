using CampusChatApp.Data;
using CampusChatApp.Helpers;
using CampusChatApp.Models;

namespace CampusChatApp.Services;

public class UserService
{
    public static IReadOnlyList<User> GetAllUsers() => AppStore.Users;

    public User? GetUser(int id) => AppStore.Users.FirstOrDefault(u => u.Id == id);

    public User UpdateProfile(User user, string displayName, string about, string courseStudying, int? studyYear, string avatarColor)
    {
        ArgumentNullException.ThrowIfNull(user);

        var profile = ValidateAndNormalizeProfile(displayName, about, courseStudying, studyYear, avatarColor);

        user.DisplayName = profile.displayName;
        user.About = profile.about;
        user.CourseStudying = profile.courseStudying;
        user.StudyYear = profile.studyYear;
        user.AvatarColor = profile.avatarColor;
        return user;
    }

    private static (string displayName, string about, string courseStudying, int? studyYear, string avatarColor) ValidateAndNormalizeProfile(
        string? displayName,
        string? about,
        string? courseStudying,
        int? studyYear,
        string? avatarColor)
    {
        var normalizedName = (displayName ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(normalizedName))
            throw new ArgumentException("Display name is required.", nameof(displayName));

        if (normalizedName.Length > Constants.MaxDisplayNameLength)
            throw new ArgumentException($"Display name must be {Constants.MaxDisplayNameLength} characters or fewer.", nameof(displayName));

        if (studyYear.HasValue && (studyYear.Value < 1 || studyYear.Value > 5))
            throw new ArgumentException("Study year must be between 1 and 5.", nameof(studyYear));

        if (string.IsNullOrWhiteSpace(avatarColor) ||
            !System.Text.RegularExpressions.Regex.IsMatch(avatarColor, "^#[0-9a-fA-F]{6}$"))
            throw new ArgumentException("Choose a valid avatar colour.", nameof(avatarColor));

        return (
            normalizedName,
            about?.Trim() ?? string.Empty,
            courseStudying?.Trim() ?? string.Empty,
            studyYear,
            avatarColor.Trim());
    }
}