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

    public void Dispose()
    {
        AppStore.Users.Clear();
        AppStore.Threads.Clear();
        AppStore.Chats.Clear();
    }
}
