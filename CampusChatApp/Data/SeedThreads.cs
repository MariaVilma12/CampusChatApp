using CampusChatApp.Models;

namespace CampusChatApp.Data;

public static class SeedThreads
{
    public static List<CampusChatApp.Models.Thread> Threads { get; } = new()
    {
        new CampusChatApp.Models.Thread
        {
            Id = 1,
            Title = "Campus event planning",
            Description = "Share ideas for upcoming events and student activities.",
            Category = "Events",
            IsPrivate = false,
            Members = SeedUsers.Users,
            Posts = new List<Post>()
        },
        new CampusChatApp.Models.Thread
        {
            Id = 2,
            Title = "Study group",
            Description = "Find classmates for collaborative study sessions.",
            Category = "Academics",
            IsPrivate = false,
            Members = SeedUsers.Users.Take(2).ToList(),
            Posts = new List<Post>()
        }
    };

    public static void Initialize() { }
}
