using CampusChatApp.Data;
using CampusChatApp.Models;
using ThreadModel = CampusChatApp.Models.Thread;

namespace CampusChatApp.Services;

public static class AppStateBootstrap
{
    public static void Initialize()
    {
        if (AppStore.Users.Count == 0)
        {
            AppStore.Users.AddRange(
            [
                new User
                {
                    Id = 1,
                    DisplayName = "Avery Patel",
                    About = "Computer science student who enjoys building accessible web apps.",
                    CourseStudying = "Computer Science",
                    StudyYear = 2,
                    AvatarColor = "#0f766e"
                },
                new User
                {
                    Id = 2,
                    DisplayName = "Milo Jensen",
                    Role = UserRole.Lecturer,
                    About = "Lecturer interested in cloud platforms, data, and team projects.",
                    CourseStudying = "Information Technology",
                    AvatarColor = "#c2410c"
                },
                new User
                {
                    Id = 3,
                    DisplayName = "Nia Okafor",
                    About = "First-year software engineering student and campus volunteer.",
                    CourseStudying = "Software Engineering",
                    StudyYear = 1,
                    AvatarColor = "#6d28d9"
                },
                new User
                {
                    Id = 4,
                    DisplayName = "Community Reviewer",
                    Role = UserRole.Auditor,
                    About = "Read-only access for campus community review.",
                    CourseStudying = "Campus Services",
                    IsAuditUser = true,
                    AvatarColor = "#be123c"
                }
            ]);
        }

        if (AppStore.Threads.Count == 0)
        {
            AppStore.Threads.AddRange(
            [
                new ThreadModel
                {
                    Id = 1,
                    Name = "General",
                    Description = "General discussion for all campus students.",
                    LastUpdatedAt = DateTime.UtcNow,
                    Posts = new List<Post>()
                },
                new ThreadModel
                {
                    Id = 2,
                    Name = "Backend C# & .NET",
                    Description = "Help and collaboration for C# and ASP.NET assignments.",
                    LastUpdatedAt = DateTime.UtcNow,
                    Posts = new List<Post>()
                },
                new ThreadModel
                {
                    Id = 3,
                    Name = "Exam Prep Study Group🔒",
                    Description = "Private study group for final preparations.",
                    LastUpdatedAt = DateTime.UtcNow,
                    IsPrivate = true,
                    AccessCode = "secret123",
                    Posts = new List<Post>()
                }
            ]);
        }
    }
}
