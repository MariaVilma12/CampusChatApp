using CampusChatApp.Models;
using ThreadModel = CampusChatApp.Models.Thread;

namespace CampusChatApp.Data;

public static class AppStore
{
    static AppStore()
    {
        SeedUsers();
    }

    public static List<User> Users = new();

    public static List<ThreadModel> Threads = new();

    public static List<DirectChat> Chats = new();

    private static void SeedUsers()
    {
        if (Users.Count == 0)
        {
            Users.Add(new User
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

            Users.Add(new User
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

            Users.Add(new User
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

            Users.Add(new User
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
    }
}