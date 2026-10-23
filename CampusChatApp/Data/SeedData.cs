using CampusChat.Models;
using ThreadModel = CampusChat.Models.Thread;

namespace CampusChat.Data;

public static class SeedData
{
    public static List<User> Users = new()
    {
        new User
        {
            Id = 1,
            DisplayName = "John",
            About = "Second year backend student.",
            AvatarColor = "Blue",
            StudyYear = 2,
            IsAuditUser = false
        },

        new User
        {
            Id = 2,
            DisplayName = "Emma",
            About = "Loves programming and databases.",
            AvatarColor = "Green",
            StudyYear = 2,
            IsAuditUser = false
        },

        new User
        {
            Id = 3,
            DisplayName = "Alex",
            About = "Enjoys web development.",
            AvatarColor = "Red",
            StudyYear = 1,
            IsAuditUser = false
        },

        new User
        {
            Id = 4,
            DisplayName = "Teacher",
            About = "Lecturer",
            AvatarColor = "Purple",
            StudyYear = 0,
            IsAuditUser = false
        },

        new User
        {
            Id = 5,
            DisplayName = "Auditor",
            About = "College Auditor",
            AvatarColor = "Black",
            StudyYear = 0,
            IsAuditUser = true
        }
    };

    public static List<ThreadModel> Threads = new()
    {
        new ThreadModel
        {
            Id = 1,
            Name = "Programming",
            IsPrivate = false
        },

        new ThreadModel
        {
            Id = 2,
            Name = "Database",
            IsPrivate = false
        },

        new ThreadModel
        {
            Id = 3,
            Name = "Backend Development",
            IsPrivate = false
        },

        new ThreadModel
        {
            Id = 4,
            Name = "Teachers Only",
            IsPrivate = true,
            AccessCode = "teacher123"
        }
    };
}