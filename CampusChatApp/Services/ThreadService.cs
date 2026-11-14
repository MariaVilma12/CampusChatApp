using CampusChatApp.Data;
using CampusChatApp.Helpers;
using CampusChatApp.Models;
using ThreadModel = CampusChatApp.Models.Thread;

namespace CampusChatApp.Services;

public class ThreadService
{
    private static readonly object UnreadLock = new();

    public static IReadOnlyList<ThreadModel> GetAllThreads() => AppStore.Threads;

    public static ThreadModel? GetThread(int id) => AppStore.Threads.FirstOrDefault(t => t.Id == id);

    public static IReadOnlyList<ThreadModel> SearchThreads(string searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
            return AppStore.Threads;

        return AppStore.Threads
            .Where(t => t.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public static int GetNextThreadId() => AppStore.Threads.Count == 0 ? 1 : AppStore.Threads.Max(t => t.Id) + 1;

    public static void AddThread(ThreadModel thread)
    {
        ArgumentNullException.ThrowIfNull(thread);
        if (string.IsNullOrWhiteSpace(thread.Name))
            throw new ArgumentException("Thread name is required.", nameof(thread));

        if (thread.IsPrivate)
        {
            thread.AccessCode = NormalizeAccessCode(thread.AccessCode);

            if (string.IsNullOrWhiteSpace(thread.AccessCode))
                throw new ArgumentException("Private threads require an access code.", nameof(thread));

            if (thread.AccessCode.Length > Constants.MaxAccessCodeLength)
                throw new ArgumentException($"Access code must be {Constants.MaxAccessCodeLength} characters or fewer.", nameof(thread));
        }

        if (thread.Id == 0)
            thread.Id = GetNextThreadId();

        thread.LastUpdatedAt = DateTime.UtcNow;
        AppStore.Threads.Add(thread);
    }

    public static bool CanAccessThread(ThreadModel? thread, User? user, string? suppliedCode = null)
    {
        if (thread is null)
            return false;

        if (user?.IsAuditUser == true)
            return true;

        if (!thread.IsPrivate)
            return true;

        if (user is null)
            return false;

        if (user.JoinedThreadIds.Contains(thread.Id))
            return true;

        var storedCode = NormalizeAccessCode(thread.AccessCode);
        if (string.IsNullOrWhiteSpace(storedCode))
            return false;

        var providedCode = NormalizeAccessCode(suppliedCode);
        return string.Equals(storedCode, providedCode, StringComparison.OrdinalIgnoreCase);
    }

    public static bool TryJoinPrivateThread(int threadId, string submittedCode, User user)
    {
        var thread = GetThread(threadId);
        if (thread is null || !thread.IsPrivate)
            return false;

        var storedCode = NormalizeAccessCode(thread.AccessCode);
        var enteredCode = NormalizeAccessCode(submittedCode);

        if (!string.Equals(storedCode, enteredCode, StringComparison.OrdinalIgnoreCase))
            return false;

        JoinThread(threadId, user);
        return true;
    }

    public static bool JoinThread(int threadId, User user)
    {
        lock (UnreadLock)
        {
            if (user.JoinedThreadIds.Contains(threadId))
                return false;

            user.JoinedThreadIds.Add(threadId);
            return true;
        }
    }

    public static bool LeaveThread(int threadId, User user)
    {
        lock (UnreadLock)
        {
            if (!user.JoinedThreadIds.Remove(threadId))
                return false;

            user.UnreadThreadPostIds.Remove(threadId);
            return true;
        }
    }

    public static void AddPost(int threadId, Post post)
    {
        var thread = GetThread(threadId);
        if (thread is null)
            return;

        ArgumentNullException.ThrowIfNull(post);
        if (!CanPostToThread(thread, post.AuthorId))
            throw new InvalidOperationException("You do not have permission to post in this thread.");

        lock (UnreadLock)
        {
            post.Id = thread.Posts.Count == 0 ? 1 : thread.Posts.Max(p => p.Id) + 1;
            thread.Posts.Add(post);
            thread.LastUpdatedAt = DateTime.UtcNow;

            foreach (var user in AppStore.Users.Where(u => u.Id != post.AuthorId && u.JoinedThreadIds.Contains(threadId)))
            {
                if (!user.UnreadThreadPostIds.TryGetValue(threadId, out var unread))
                    user.UnreadThreadPostIds[threadId] = unread = new HashSet<int>();

                unread.Add(post.Id);
            }
        }
    }

    private static string? NormalizeAccessCode(string? code)
    {
        return string.IsNullOrWhiteSpace(code) ? null : code.Trim();
    }

    public void AddReply(int threadId, int postId, Reply reply)
    {
        var thread = GetThread(threadId);
        if (thread is null)
            return;

        var post = thread.Posts.FirstOrDefault(p => p.Id == postId);
        if (post is null)
            return;

        if (!CanPostToThread(thread, reply.AuthorId))
            throw new InvalidOperationException("You do not have permission to reply in this thread.");

        post.Replies.Add(reply);
        thread.LastUpdatedAt = DateTime.UtcNow;
    }

    public bool DeletePost(int threadId, int postId, User currentUser)
    {
        var thread = GetThread(threadId);
        if (thread is null)
            return false;

        lock (UnreadLock)
        {
            var post = thread.Posts.FirstOrDefault(p => p.Id == postId);
            if (post is null || post.AuthorId != currentUser.Id)
                return false;

            thread.Posts.Remove(post);
            foreach (var user in AppStore.Users)
            {
                if (user.UnreadThreadPostIds.TryGetValue(threadId, out var unread))
                    unread.Remove(postId);
            }
            thread.LastUpdatedAt = DateTime.UtcNow;
        }
        return true;
    }

    public static int GetUnreadCount(User user, int threadId)
    {
        lock (UnreadLock)
        {
            return user.JoinedThreadIds.Contains(threadId) &&
                   user.UnreadThreadPostIds.TryGetValue(threadId, out var unread)
                ? unread.Count
                : 0;
        }
    }

    public static bool MarkThreadAsOpened(User user, int threadId)
    {
        lock (UnreadLock)
        {
            return user.JoinedThreadIds.Contains(threadId) &&
                   user.UnreadThreadPostIds.Remove(threadId);
        }
    }

    private static bool CanPostToThread(ThreadModel thread, int authorId)
    {
        var author = AppStore.Users.FirstOrDefault(user => user.Id == authorId);
        if (author is null || (thread.IsPrivate && author.IsAuditUser))
            return false;

        return !thread.IsPrivate || author.JoinedThreadIds.Contains(thread.Id);
    }
}
