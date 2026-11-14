using CampusChatApp.Data;
using CampusChatApp.Models;
using CampusChatApp.Services;
using Xunit;

namespace CampusChatApp.Tests;

public sealed class AppStateBootstrapTests : IDisposable
{
    public AppStateBootstrapTests()
    {
        AppStore.Users.Clear();
        AppStore.Threads.Clear();
        AppStore.Chats.Clear();
    }

    [Fact]
    public void SeedsAlternativeDemoProfilesAndCampusThreadsOnce()
    {
        AppStateBootstrap.Initialize();

        Assert.Equal(4, AppStore.Users.Count);
        Assert.Contains(AppStore.Users, user => user.DisplayName == "Avery Patel");
        var lecturer = Assert.Single(AppStore.Users, user => user.DisplayName == "Milo Jensen");
        Assert.Equal(UserRole.Lecturer, lecturer.Role);
        Assert.Contains(AppStore.Users, user => user.DisplayName == "Nia Okafor");
        Assert.Single(AppStore.Users, user => user.IsAuditUser);
        Assert.Contains(AppStore.Users, user => user.IsAuditUser && user.Role == UserRole.Auditor);
        Assert.Equal(
            ["General", "Backend C# & .NET", "Exam Prep Study Group🔒"],
            AppStore.Threads.Select(thread => thread.Name));
        Assert.Equal("General discussion for all campus students.", AppStore.Threads[0].Description);
        Assert.Equal("Help and collaboration for C# and ASP.NET assignments.", AppStore.Threads[1].Description);
        Assert.Equal("Private study group for final preparations.", AppStore.Threads[2].Description);
        Assert.True(AppStore.Threads[2].IsPrivate);
        Assert.Equal("secret123", AppStore.Threads[2].AccessCode);

        AppStateBootstrap.Initialize();
        Assert.Equal(4, AppStore.Users.Count);
        Assert.Equal(3, AppStore.Threads.Count);
    }

    [Fact]
    public void CreatesValidatedStudentProfileWithUniqueId()
    {
        AppStateBootstrap.Initialize();
        var service = new UserService();

        var profile = service.CreateProfile("  Demo Student  ", " About ", " Computing ", 2, "#123456");

        Assert.Equal(5, profile.Id);
        Assert.Equal("Demo Student", profile.DisplayName);
        Assert.Equal("About", profile.About);
        Assert.Equal("Computing", profile.CourseStudying);
        Assert.Equal(UserRole.Student, profile.Role);
        Assert.False(profile.IsAuditUser);
        Assert.Same(profile, service.GetUser(profile.Id));
        Assert.Throws<ArgumentException>(() => service.CreateProfile("  ", "", "", null, "#123456"));
        Assert.Throws<ArgumentException>(() => service.CreateProfile("Student", "", "", 6, "#123456"));
        Assert.Equal(5, AppStore.Users.Count);
    }

    [Fact]
    public void TracksUnreadPostsPerMemberAndThreadAndRemovesDeletedPosts()
    {
        AppStateBootstrap.Initialize();
        var reader = AppStore.Users[0];
        var author = AppStore.Users[2];
        reader.JoinedThreadIds.AddRange([1, 2]);
        author.JoinedThreadIds.Add(1);
        var first = new Post { AuthorId = author.Id, Text = "First" };
        var second = new Post { AuthorId = author.Id, Text = "Second" };
        var otherThread = new Post { AuthorId = author.Id, Text = "Other thread" };

        ThreadService.AddPost(1, first);
        ThreadService.AddPost(1, second);
        ThreadService.AddPost(2, otherThread);
        Assert.Equal(2, ThreadService.GetUnreadCount(reader, 1));
        Assert.Equal(1, ThreadService.GetUnreadCount(reader, 2));
        Assert.Equal(0, ThreadService.GetUnreadCount(author, 1));
        Assert.Equal(0, ThreadService.GetUnreadCount(AppStore.Users[1], 1));

        Assert.True(new ThreadService().DeletePost(1, first.Id, author));
        Assert.Equal(1, ThreadService.GetUnreadCount(reader, 1));
        Assert.True(ThreadService.MarkThreadAsOpened(reader, 1));
        Assert.Equal(0, ThreadService.GetUnreadCount(reader, 1));
        Assert.Equal(1, ThreadService.GetUnreadCount(reader, 2));
        Assert.False(ThreadService.MarkThreadAsOpened(reader, 1));
        Assert.True(ThreadService.LeaveThread(2, reader));
        Assert.Equal(0, ThreadService.GetUnreadCount(reader, 2));
    }

    public void Dispose()
    {
        AppStore.Users.Clear();
        AppStore.Threads.Clear();
        AppStore.Chats.Clear();
    }
}
