using CampusChatApp.Data;
using CampusChatApp.Models;
using ThreadModel = CampusChatApp.Models.Thread;

namespace CampusChatApp.Services;

public class ThreadService
{
    public IReadOnlyList<ThreadModel> GetAllThreads()
    {
        return SeedData.Threads;
    }

    public ThreadModel? GetThread(int id)
    {
        return SeedData.Threads.FirstOrDefault(t => t.Id == id);
    }

    public IReadOnlyList<ThreadModel> SearchThreads(string searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
            return SeedData.Threads;

        return SeedData.Threads
            .Where(t => t.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public bool CanAccessThread(ThreadModel? thread, User? user, string? suppliedCode = null)
    {
        if (thread is null)
            return false;

        if (user?.IsAuditUser == true)
            return true;

        if (!thread.IsPrivate)
            return true;

        if (string.IsNullOrWhiteSpace(thread.AccessCode))
            return false;

        var code = suppliedCode ?? string.Empty;
        return user != null && (user.JoinedThreadIds.Contains(thread.Id) || thread.AccessCode == code);
    }

    public bool TryJoinPrivateThread(int threadId, string submittedCode, User user)
    {
        var thread = GetThread(threadId);
        if (thread is null || !thread.IsPrivate)
            return false;

        if (thread.AccessCode != submittedCode)
            return false;

        if (!user.JoinedThreadIds.Contains(threadId))
            user.JoinedThreadIds.Add(threadId);

        return true;
    }

    public void AddPost(int threadId, Post post)
    {
        var thread = GetThread(threadId);
        if (thread is null)
            return;

        thread.Posts.Add(post);
    }

    public void MarkThreadAsOpened(User user, int threadId)
    {
        if (user.UnreadThreadCounts.ContainsKey(threadId))
            user.UnreadThreadCounts[threadId] = 0;
    }

    public void NotifyUnreadThread(User user, int threadId)
    {
        if (!user.JoinedThreadIds.Contains(threadId))
            return;

        if (!user.UnreadThreadCounts.ContainsKey(threadId))
            user.UnreadThreadCounts[threadId] = 0;

        user.UnreadThreadCounts[threadId] += 1;
    }
}
