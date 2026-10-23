using CampusChatApp.Models;
using ThreadModel = CampusChatApp.Models.Thread;

namespace CampusChatApp.Data;

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
            IsAuditUser = false,
            JoinedThreadIds = new List<int> { 1, 2 }
        },

        new User
        {
            Id = 2,
            DisplayName = "Emma",
            About = "Loves programming and databases.",
            AvatarColor = "Green",
            StudyYear = 2,
            IsAuditUser = false,
            JoinedThreadIds = new List<int> { 1, 2, 4 }
        },

        new User
        {
            Id = 3,
            DisplayName = "Alex",
            About = "Enjoys web development.",
            AvatarColor = "Red",
            StudyYear = 1,
            IsAuditUser = false,
            JoinedThreadIds = new List<int> { 1 }
        },

        new User
        {
            Id = 4,
            DisplayName = "Teacher",
            About = "Lecturer",
            AvatarColor = "Purple",
            StudyYear = 0,
            IsAuditUser = false,
            JoinedThreadIds = new List<int> { 4 }
        },

        new User
        {
            Id = 5,
            DisplayName = "Auditor",
            About = "College Auditor",
            AvatarColor = "Black",
            StudyYear = 0,
            IsAuditUser = true,
            JoinedThreadIds = new List<int> { 1, 2, 3, 4 }
        }
    };

    public static List<ThreadModel> Threads = new()
    {
        new ThreadModel
        {
            Id = 1,
            Name = "Programming",
            IsPrivate = false,
            Posts = new List<Post>()
        },

        new ThreadModel
        {
            Id = 2,
            Name = "Database",
            IsPrivate = false,
            Posts = new List<Post>()
        },

        new ThreadModel
        {
            Id = 3,
            Name = "Backend Development",
            IsPrivate = false,
            Posts = new List<Post>()
        },

        new ThreadModel
        {
            Id = 4,
            Name = "Teachers Only",
            IsPrivate = true,
            AccessCode = "teacher123",
            Posts = new List<Post>()
        }
    };

    public static List<DirectChat> Chats = new()
    {
        new DirectChat
        {
            Id = 1,
            User1Id = 1,
            User2Id = 2,
            Messages = new List<ChatMessage>
            {
                new ChatMessage { Id = 1, SenderId = 1, SenderName = "John", Text = "Are we meeting for lab review?", SentAt = DateTime.Now.AddMinutes(-15) },
                new ChatMessage { Id = 2, SenderId = 2, SenderName = "Emma", Text = "Yes, let’s do it after class.", SentAt = DateTime.Now.AddMinutes(-10) }
            }
        },
        new DirectChat
        {
            Id = 2,
            User1Id = 1,
            User2Id = 5,
            Messages = new List<ChatMessage>
            {
                new ChatMessage { Id = 1, SenderId = 5, SenderName = "Auditor", Text = "I can review access logs if needed.", SentAt = DateTime.Now.AddMinutes(-5) }
            }
        }
    };
}