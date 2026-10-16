using CampusChatApp.Models;

namespace CampusChatApp.Data;

public static class SeedUsers
{
    public static List<User> Users { get; } = new()
    {
        new User { Id = 1, FirstName = "Alex", LastName = "Rivera", Email = "alex@campus.edu", Major = "Computer Science", Bio = "Loves hackathons." },
        new User { Id = 2, FirstName = "Sam", LastName = "Lee", Email = "sam@campus.edu", Major = "Psychology", Bio = "Interested in student wellbeing." },
        new User { Id = 3, FirstName = "Priya", LastName = "Nair", Email = "priya@campus.edu", Major = "Biology", Bio = "Campus event organizer." }
    };

    public static void Initialize() { }
}
