using System.Security.Cryptography;
using System.Text;
using CampusChatApp.Data;
using CampusChatApp.Models;

namespace CampusChatApp.Services;

public class ChatService
{
    public record Profile
    {
        public string Id { get; init; } = string.Empty;
        public string DisplayName { get; init; } = string.Empty;
        public string Bio { get; init; } = string.Empty;
        public string AvatarColor { get; init; } = "#3B82F6";
        public int? StudyYear { get; init; }
        public bool IsAuditRole { get; init; }
    }

    public record ChatThread
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string? Topic { get; init; }
        public string OwnerId { get; init; } = string.Empty;
        public bool IsPrivate { get; init; }
    }

    public record Post
    {
        public Guid Id { get; init; }
        public Guid ThreadId { get; init; }
        public string AuthorId { get; init; } = string.Empty;
        public Guid? ParentPostId { get; init; }
        public string Content { get; init; } = string.Empty;
        public DateTime CreatedAtUtc { get; init; } = DateTime.UtcNow;
        public bool IsDeleted { get; init; }
    }

    public record DirectMessage
    {
        public Guid Id { get; init; }
        public string SenderId { get; init; } = string.Empty;
        public string RecipientId { get; init; } = string.Empty;
        public string Content { get; init; } = string.Empty;
        public DateTime CreatedAtUtc { get; init; } = DateTime.UtcNow;
    }

    public record Membership
    {
        public Guid ThreadId { get; init; }
        public string ProfileId { get; init; } = string.Empty;
        public DateTime JoinedAtUtc { get; init; } = DateTime.UtcNow;
    }

    public record Bookmark
    {
        public Guid ThreadId { get; init; }
        public string ProfileId { get; init; } = string.Empty;
        public DateTime LastReadUtc { get; init; } = DateTime.UtcNow;
    }

    public record DirectChatSummary
    {
        public string OtherUserId { get; init; } = string.Empty;
        public DateTime LastMessageUtc { get; init; }
    }

    public class CampusChatValidationException : Exception
    {
        public CampusChatValidationException(string message) : base(message)
        {
        }
    }

    public static readonly IReadOnlyList<string> AvatarColors = new List<string>
    {
        "#5B8DEF",
        "#2FBF71",
        "#9B6BFF",
        "#EF5A5A",
        "#F4B942",
        "#F06CB0"
    };

    private readonly List<Profile> _profiles = new()
    {
        new Profile
        {
            Id = "student1",
            DisplayName = "Maya",
            Bio = "Computer Science student focused on software engineering and web apps.",
            AvatarColor = "#5B8DEF",
            StudyYear = 2
        },
        new Profile
        {
            Id = "student2",
            DisplayName = "Ethan",
            Bio = "Interested in data systems, cloud tools, and backend services.",
            AvatarColor = "#2FBF71",
            StudyYear = 3
        },
        new Profile
        {
            Id = "lecturer1",
            DisplayName = "Dr. Sofia Grant",
            Bio = "Lecturer in software architecture and systems design.",
            AvatarColor = "#9B6BFF",
            StudyYear = null
        },
        new Profile
        {
            Id = "auditor1",
            DisplayName = "Campus Auditor",
            Bio = "Reviews platform compliance, safety, and message auditing.",
            AvatarColor = "#EF5A5A",
            StudyYear = null,
            IsAuditRole = true
        }
    };

    private readonly List<ChatThread> _threads = new();
    private readonly Dictionary<Guid, string> _privateThreadCodes = new();

    private readonly List<Post> _posts = new();
    private readonly object _postsLock = new();

    private readonly List<Membership> _memberships = new();
    private readonly object _membershipsLock = new();

    private readonly List<Bookmark> _bookmarks = new();
    private readonly object _bookmarksLock = new();

    private readonly List<DirectMessage> _directMessages = new();
    private readonly object _directMessagesLock = new();

    private readonly object _profilesLock = new();

    public event Action<Guid>? DataChanged;
    public event Action<string, string>? DirectMessageSent;

    public ChatService()
    {
        CreateThread(
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            "Campus Hub",
            "General campus announcements, events, and conversations for students.",
            "lecturer1",
            null
        );

        CreateThread(
            Guid.Parse("22222222-2222-2222-2222-222222222222"),
            "C# & ASP.NET Studio",
            "Help and collaboration for software engineering and .NET coursework.",
            "student1",
            null
        );

        CreateThread(
            Guid.Parse("33333333-3333-3333-3333-333333333333"),
            "Final Revision Circle",
            "Private study group for final-year revision and exam prep.",
            "lecturer1",
            "campus2026"
        );
    }

    private static string HashCode(string code)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(code.Trim()));
        return Convert.ToHexString(bytes);
    }

    private bool IsAuditor(string? userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            return false;
        }

        lock (_profilesLock)
        {
            var profile = _profiles.FirstOrDefault(p =>
                string.Equals(p.Id, userId, StringComparison.OrdinalIgnoreCase));
            return profile?.IsAuditRole == true;
        }
    }

    private bool CanReadThread(Guid threadId, string? userId)
    {
        return IsAuditor(userId) || IsPublicOrMember(threadId, userId);
    }

    private bool CanPostToThread(Guid threadId, string? userId)
    {
        return !string.IsNullOrEmpty(userId) && IsPublicOrMember(threadId, userId);
    }

    private bool IsPublicOrMember(Guid threadId, string? userId)
    {
        var thread = _threads.FirstOrDefault(t => t.Id == threadId);
        if (thread is null || !thread.IsPrivate)
        {
            return true;
        }

        if (string.IsNullOrEmpty(userId))
        {
            return false;
        }

        lock (_membershipsLock)
        {
            return _memberships.Any(m =>
                m.ThreadId == threadId &&
                string.Equals(m.ProfileId, userId, StringComparison.OrdinalIgnoreCase));
        }
    }

    public Task<IReadOnlyList<Profile>> GetProfilesAsync()
    {
        lock (_profilesLock)
        {
            return Task.FromResult<IReadOnlyList<Profile>>(_profiles.ToList());
        }
    }

    public Task<Profile?> GetProfileByIdAsync(string id)
    {
        lock (_profilesLock)
        {
            var profile = _profiles.FirstOrDefault(p =>
                string.Equals(p.Id, id, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(profile);
        }
    }

    public Task UpdateProfileAsync(string profileId, string requesterId, string displayName, string? bio,
        string avatarColor, int? studyYear)
    {
        if (string.IsNullOrWhiteSpace(displayName))
        {
            throw new CampusChatValidationException("Display name cannot be empty.");
        }

        if (!AvatarColors.Contains(avatarColor))
        {
            throw new CampusChatValidationException("Please pick one of the available colours.");
        }

        if (studyYear.HasValue && (studyYear.Value < 1 || studyYear.Value > 5))
        {
            throw new CampusChatValidationException("Study year must be between 1 and 5.");
        }

        lock (_profilesLock)
        {
            var profile = _profiles.FirstOrDefault(p =>
                string.Equals(p.Id, profileId, StringComparison.OrdinalIgnoreCase));
            if (profile is null)
            {
                throw new ArgumentException("The specified user profile does not exist.", nameof(profileId));
            }

            if (!string.Equals(profile.Id, requesterId, StringComparison.OrdinalIgnoreCase))
            {
                throw new CampusChatValidationException("You can only edit your own profile.");
            }

            var updated = profile with
            {
                DisplayName = displayName.Trim(),
                Bio = bio?.Trim() ?? string.Empty,
                AvatarColor = avatarColor,
                StudyYear = studyYear
            };

            var index = _profiles.IndexOf(profile);
            _profiles[index] = updated;
        }

        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<ChatThread>> GetThreadsAsync()
    {
        IReadOnlyList<ChatThread> list = _threads.AsReadOnly();
        return Task.FromResult(list);
    }

    public Task<ChatThread?> GetThreadByIdAsync(Guid id)
    {
        var thread = _threads.FirstOrDefault(t => t.Id == id);
        return Task.FromResult(thread);
    }

    public Task<IReadOnlyList<Post>> GetPostsAsync(Guid threadId, string? requesterId = null)
    {
        if (!CanReadThread(threadId, requesterId))
        {
            return Task.FromResult<IReadOnlyList<Post>>(Array.Empty<Post>());
        }

        lock (_postsLock)
        {
            var posts = _posts
                .Where(p => p.ThreadId == threadId)
                .OrderBy(p => p.CreatedAtUtc)
                .ToList();

            return Task.FromResult<IReadOnlyList<Post>>(posts);
        }
    }

    public Task AddPostAsync(Guid threadId, string authorId, string content, Guid? parentPostId = null)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new CampusChatValidationException("Post content cannot be empty.");
        }

        Profile? profile;
        lock (_profilesLock)
        {
            profile = _profiles.FirstOrDefault(p =>
                string.Equals(p.Id, authorId, StringComparison.OrdinalIgnoreCase));
        }

        if (profile is null)
        {
            throw new ArgumentException("The specified author profile does not exist.", nameof(authorId));
        }

        if (!CanPostToThread(threadId, profile.Id))
        {
            throw new CampusChatValidationException("You need the access code to post in this thread.");
        }

        lock (_postsLock)
        {
            if (!_threads.Any(t => t.Id == threadId))
            {
                throw new ArgumentException("The specified chat thread does not exist.", nameof(threadId));
            }

            if (parentPostId.HasValue)
            {
                var parentPost = _posts.FirstOrDefault(p => p.Id == parentPostId.Value);
                if (parentPost is null || parentPost.ThreadId != threadId)
                {
                    throw new ArgumentException("The specified parent post does not exist within this thread.",
                        nameof(parentPostId));
                }
            }

            var newPost = new Post
            {
                Id = Guid.NewGuid(),
                ThreadId = threadId,
                AuthorId = profile.Id,
                ParentPostId = parentPostId,
                Content = content.Trim(),
                CreatedAtUtc = DateTime.UtcNow,
                IsDeleted = false
            };

            _posts.Add(newPost);
        }

        DataChanged?.Invoke(threadId);
        return Task.CompletedTask;
    }

    public Task DeletePostAsync(Guid postId, string requesterId)
    {
        Post? post;
        lock (_postsLock)
        {
            post = _posts.FirstOrDefault(p => p.Id == postId);
            if (post is null)
            {
                throw new ArgumentException("Post not found.", nameof(postId));
            }

            if (!string.Equals(post.AuthorId, requesterId, StringComparison.OrdinalIgnoreCase))
            {
                throw new CampusChatValidationException("You can only delete your own posts.");
            }

            var deleted = post with { IsDeleted = true, Content = string.Empty };
            var index = _posts.IndexOf(post);
            _posts[index] = deleted;
        }

        DataChanged?.Invoke(post.ThreadId);
        return Task.CompletedTask;
    }

    public Task JoinThreadAsync(Guid threadId, string userId, string? accessCode = null)
    {
        var thread = _threads.FirstOrDefault(t => t.Id == threadId);
        if (thread is null)
        {
            throw new ArgumentException("The specified chat thread does not exist.", nameof(threadId));
        }

        if (thread.IsPrivate)
        {
            if (string.IsNullOrWhiteSpace(accessCode) ||
                !_privateThreadCodes.TryGetValue(threadId, out var storedHash) ||
                !string.Equals(storedHash, HashCode(accessCode), StringComparison.OrdinalIgnoreCase))
            {
                throw new CampusChatValidationException("Wrong access code.");
            }
        }

        Profile? profile;
        lock (_profilesLock)
        {
            profile = _profiles.FirstOrDefault(p =>
                string.Equals(p.Id, userId, StringComparison.OrdinalIgnoreCase));
        }

        if (profile is null)
        {
            throw new ArgumentException("The specified user profile does not exist.", nameof(userId));
        }

        lock (_membershipsLock)
        {
            var alreadyMember = _memberships.Any(m =>
                m.ThreadId == threadId &&
                string.Equals(m.ProfileId, profile.Id, StringComparison.OrdinalIgnoreCase));

            if (alreadyMember)
            {
                return Task.CompletedTask;
            }

            _memberships.Add(new Membership
            {
                ProfileId = profile.Id,
                ThreadId = threadId,
                JoinedAtUtc = DateTime.UtcNow
            });
        }

        DataChanged?.Invoke(threadId);
        return Task.CompletedTask;
    }

    public Task LeaveThreadAsync(Guid threadId, string userId)
    {
        bool removed = false;
        lock (_membershipsLock)
        {
            var membership = _memberships.FirstOrDefault(m =>
                m.ThreadId == threadId &&
                string.Equals(m.ProfileId, userId, StringComparison.OrdinalIgnoreCase));

            if (membership is not null)
            {
                _memberships.Remove(membership);
                removed = true;
            }
        }

        if (removed)
        {
            DataChanged?.Invoke(threadId);
        }

        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<Guid>> GetJoinedThreadIdsAsync(string userId)
    {
        lock (_membershipsLock)
        {
            var joinedIds = _memberships
                .Where(m => string.Equals(m.ProfileId, userId, StringComparison.OrdinalIgnoreCase))
                .Select(m => m.ThreadId)
                .ToList();

            return Task.FromResult<IReadOnlyList<Guid>>(joinedIds);
        }
    }

    public Task MarkThreadReadAsync(Guid threadId, string userId)
    {
        lock (_bookmarksLock)
        {
            _bookmarks.RemoveAll(b =>
                b.ThreadId == threadId &&
                string.Equals(b.ProfileId, userId, StringComparison.OrdinalIgnoreCase));

            _bookmarks.Add(new Bookmark
            {
                ProfileId = userId,
                ThreadId = threadId,
                LastReadUtc = DateTime.UtcNow
            });
        }

        return Task.CompletedTask;
    }

    public Task<int> GetUnreadCountAsync(Guid threadId, string userId)
    {
        DateTime? baselineUtc = null;

        lock (_bookmarksLock)
        {
            var bookmark = _bookmarks.FirstOrDefault(b =>
                b.ThreadId == threadId &&
                string.Equals(b.ProfileId, userId, StringComparison.OrdinalIgnoreCase));

            if (bookmark is not null)
            {
                baselineUtc = bookmark.LastReadUtc;
            }
        }

        if (baselineUtc is null)
        {
            lock (_membershipsLock)
            {
                var membership = _memberships.FirstOrDefault(m =>
                    m.ThreadId == threadId &&
                    string.Equals(m.ProfileId, userId, StringComparison.OrdinalIgnoreCase));

                baselineUtc = membership?.JoinedAtUtc ?? DateTime.MinValue;
            }
        }

        int unreadCount;
        lock (_postsLock)
        {
            unreadCount = _posts.Count(p =>
                p.ThreadId == threadId &&
                !p.IsDeleted &&
                !string.Equals(p.AuthorId, userId, StringComparison.OrdinalIgnoreCase) &&
                p.CreatedAtUtc > baselineUtc.Value);
        }

        return Task.FromResult(unreadCount);
    }

    public Task SendDirectMessageAsync(string senderId, string recipientId, string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new CampusChatValidationException("Message content cannot be empty.");
        }

        if (string.Equals(senderId, recipientId, StringComparison.OrdinalIgnoreCase))
        {
            throw new CampusChatValidationException("You cannot message yourself.");
        }

        Profile? senderProfile;
        Profile? recipientProfile;
        lock (_profilesLock)
        {
            senderProfile = _profiles.FirstOrDefault(p =>
                string.Equals(p.Id, senderId, StringComparison.OrdinalIgnoreCase));
            recipientProfile = _profiles.FirstOrDefault(p =>
                string.Equals(p.Id, recipientId, StringComparison.OrdinalIgnoreCase));
        }

        if (senderProfile is null)
        {
            throw new ArgumentException("The specified sender profile does not exist.", nameof(senderId));
        }

        if (recipientProfile is null)
        {
            throw new ArgumentException("The specified recipient profile does not exist.", nameof(recipientId));
        }

        lock (_directMessagesLock)
        {
            _directMessages.Add(new DirectMessage
            {
                Id = Guid.NewGuid(),
                SenderId = senderProfile.Id,
                RecipientId = recipientProfile.Id,
                Content = content.Trim(),
                CreatedAtUtc = DateTime.UtcNow
            });
        }

        DirectMessageSent?.Invoke(senderProfile.Id, recipientProfile.Id);
        return Task.CompletedTask;
    }

    public IReadOnlyList<DirectChat> GetAllChats()
    {
        return AppStore.Chats;
    }

    public Task<IReadOnlyList<DirectChat>> GetAllChatsAsync()
    {
        return Task.FromResult(GetAllChats());
    }

    public DirectChat? GetChat(int id)
    {
        return AppStore.Chats.FirstOrDefault(c => c.Id == id);
    }

    public DirectChat AddChat(DirectChat chat)
    {
        chat.Id = AppStore.Chats.Count == 0 ? 1 : AppStore.Chats.Max(c => c.Id) + 1;
        AppStore.Chats.Add(chat);
        return chat;
    }

    public static void AddMessage(int chatId, ChatMessage message)
    {
        var chat = AppStore.Chats.FirstOrDefault(c => c.Id == chatId);
        if (chat is null)
        {
            return;
        }

        chat.Messages.Add(message);
    }

    public IReadOnlyList<DirectChat> GetChatsForUser(User user)
    {
        return AppStore.Chats
            .Where(c => c.User1Id == user.Id || c.User2Id == user.Id)
            .OrderByDescending(c => c.Messages.LastOrDefault()?.SentAt ?? DateTime.MinValue)
            .ToList();
    }

    public Task<IReadOnlyList<DirectMessage>> GetConversationAsync(string userId, string otherUserId)
    {
        lock (_directMessagesLock)
        {
            var messages = _directMessages
                .Where(m =>
                    (string.Equals(m.SenderId, userId, StringComparison.OrdinalIgnoreCase) &&
                        string.Equals(m.RecipientId, otherUserId, StringComparison.OrdinalIgnoreCase)) ||
                    (string.Equals(m.SenderId, otherUserId, StringComparison.OrdinalIgnoreCase) &&
                        string.Equals(m.RecipientId, userId, StringComparison.OrdinalIgnoreCase)))
                .OrderBy(m => m.CreatedAtUtc)
                .ToList();

            return Task.FromResult<IReadOnlyList<DirectMessage>>(messages);
        }
    }

    public Task<IReadOnlyList<DirectChatSummary>> GetDirectChatsAsync(string userId)
    {
        lock (_directMessagesLock)
        {
            var summaries = _directMessages
                .Where(m =>
                    string.Equals(m.SenderId, userId, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(m.RecipientId, userId, StringComparison.OrdinalIgnoreCase))
                .GroupBy(m => string.Equals(m.SenderId, userId, StringComparison.OrdinalIgnoreCase)
                    ? m.RecipientId
                    : m.SenderId, StringComparer.OrdinalIgnoreCase)
                .Select(g => new DirectChatSummary
                {
                    OtherUserId = g.Key,
                    LastMessageUtc = g.Max(m => m.CreatedAtUtc)
                })
                .OrderByDescending(s => s.LastMessageUtc)
                .ToList();

            return Task.FromResult<IReadOnlyList<DirectChatSummary>>(summaries);
        }
    }

    public Task<bool> CanReadThreadAsync(Guid threadId, string? userId)
    {
        return Task.FromResult(CanReadThread(threadId, userId));
    }

    public Task<bool> CanPostToThreadAsync(Guid threadId, string? userId)
    {
        return Task.FromResult(CanPostToThread(threadId, userId));
    }

    public Task<bool> IsAuditorAsync(string? userId)
    {
        return Task.FromResult(IsAuditor(userId));
    }

    public Task<IReadOnlyList<(string UserA, string UserB)>> GetAllConversationPairsAsync(string requesterId)
    {
        if (!IsAuditor(requesterId))
        {
            throw new CampusChatValidationException("Only the audit role can view all conversations.");
        }

        lock (_directMessagesLock)
        {
            var pairs = _directMessages
                .Select(m => string.CompareOrdinal(m.SenderId, m.RecipientId) < 0
                    ? (UserA: m.SenderId, UserB: m.RecipientId)
                    : (UserA: m.RecipientId, UserB: m.SenderId))
                .Distinct()
                .ToList();

            return Task.FromResult<IReadOnlyList<(string UserA, string UserB)>>(pairs);
        }
    }

    public Task<IReadOnlyList<DirectMessage>> GetConversationForAuditAsync(string requesterId, string userA, string userB)
    {
        if (!IsAuditor(requesterId))
        {
            throw new CampusChatValidationException("Only the audit role can view all conversations.");
        }

        return GetConversationAsync(userA, userB);
    }

    private void CreateThread(Guid id, string name, string? topic, string ownerId, string? accessCode)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Thread name cannot be empty.", nameof(name));
        }

        bool isPrivate = !string.IsNullOrWhiteSpace(accessCode);

        if (isPrivate)
        {
            _privateThreadCodes[id] = HashCode(accessCode!);
        }

        var thread = new ChatThread
        {
            Id = id,
            Name = name.Trim(),
            Topic = topic?.Trim(),
            OwnerId = ownerId,
            IsPrivate = isPrivate
        };

        _threads.Add(thread);
    }
}
