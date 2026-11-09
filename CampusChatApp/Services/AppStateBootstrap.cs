using CampusChatApp.Data;

namespace CampusChatApp.Services;

public static class AppStateBootstrap
{
    public static void Initialize()
    {
        _ = AppStore.Users;
        _ = AppStore.Threads;
        _ = AppStore.Chats;

        if (AppStore.Users.Count == 0)
        {
            AppStore.Users.Add(new CampusChatApp.Models.User
            {
                Id = 1,
                DisplayName = "Maya Reyes",
                About = "Computer Science student focused on software engineering and web apps.",
                CourseStudying = "Computer Science",
                StudyYear = 2,
                AvatarColor = "#5B8DEF",
                IsAuditUser = false,
                JoinedThreadIds = new List<int>(),
                UnreadThreadCounts = new Dictionary<int, int>()
            });

            AppStore.Users.Add(new CampusChatApp.Models.User
            {
                Id = 2,
                DisplayName = "Ethan Cole",
                About = "Interested in data systems, cloud tools, and backend services.",
                CourseStudying = "Data Engineering",
                StudyYear = 3,
                AvatarColor = "#2FBF71",
                IsAuditUser = false,
                JoinedThreadIds = new List<int>(),
                UnreadThreadCounts = new Dictionary<int, int>()
            });

            AppStore.Users.Add(new CampusChatApp.Models.User
            {
                Id = 3,
                DisplayName = "Dr. Sofia Grant",
                About = "Lecturer in software architecture and systems design.",
                CourseStudying = "Software Architecture",
                StudyYear = 0,
                AvatarColor = "#9B6BFF",
                IsAuditUser = false,
                JoinedThreadIds = new List<int>(),
                UnreadThreadCounts = new Dictionary<int, int>()
            });

            AppStore.Users.Add(new CampusChatApp.Models.User
            {
                Id = 4,
                DisplayName = "Campus Auditor",
                About = "Reviews platform compliance, safety, and message auditing.",
                CourseStudying = "Audit & Compliance",
                StudyYear = 0,
                AvatarColor = "#EF5A5A",
                IsAuditUser = true,
                JoinedThreadIds = new List<int>(),
                UnreadThreadCounts = new Dictionary<int, int>()
            });
        }

        if (AppStore.Threads.Count == 0)
        {
            AppStore.Threads.Add(new CampusChatApp.Models.Thread
            {
                Id = 1,
                Name = "General",
                Description = "General discussion for all campus students.",
                LastUpdatedAt = DateTime.UtcNow,
                IsPrivate = false,
                AccessCode = null,
                Posts = new List<CampusChatApp.Models.Post>()
            });

            AppStore.Threads.Add(new CampusChatApp.Models.Thread
            {
                Id = 2,
                Name = "Backend C# & .NET",
                Description = "Help and collaboration for C# and ASP.NET assignments.",
                LastUpdatedAt = DateTime.UtcNow.AddMinutes(-15),
                IsPrivate = false,
                AccessCode = null,
                Posts = new List<CampusChatApp.Models.Post>()
            });

            AppStore.Threads.Add(new CampusChatApp.Models.Thread
            {
                Id = 3,
                Name = "Exam Prep Study Group🔒",
                Description = "Private study group for final preparations.",
                LastUpdatedAt = DateTime.UtcNow.AddHours(-1),
                IsPrivate = true,
                AccessCode = "secret123",
                Posts = new List<CampusChatApp.Models.Post>()
            });
        }
    }
}
