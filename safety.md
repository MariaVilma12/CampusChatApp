# Safety note

**1. Personal data**

The app holds each profile's ID, display name, role, About text, course, study year, avatar colour, and audit-role flag. It also holds thread memberships and unread post IDs; post and reply text, author IDs and names, and timestamps; and direct-chat participant IDs, message text, sender IDs and names, and timestamps. The selected profile ID is kept in protected browser session storage. The app does not record when a user joined or last read a thread. Do not enter real personal or sensitive data.

**2. Chat retention**

The proposed product policy is to delete a direct chat 12 months after its last message. A year gives participants time to refer back to recent conversations while limiting indefinite retention. This policy is not implemented: in the demo, chats remain in server memory only until the app process stops or restarts.

**3. A student leaves**

Leaving college does not trigger any action in this app. Profile deletion and post anonymisation are not implemented, so the profile and its posts remain in server memory until the app process stops or restarts. A production policy must decide whether to delete or anonymise them and how long to retain them.

**4. Abuse**

In the current app, only the post's author can remove it; there is no reporting or moderation feature. Under the proposed product policy, a designated college moderator decides whether reported content violates the rules and should be removed. That review and moderator role are not implemented yet; a production service needs a clear process that protects evidence.

**5. Direct chat privacy**

The chat page checks whether the selected profile is a participant or the auditor. These are demo checks, not a security boundary: anyone can select any profile, including the seeded **Community Reviewer** auditor profile. The app does not notify the two participants when the auditor reads their chat, and messages are not encrypted in memory. Do not expose it to untrusted networks.

**6. Logging**

Message text and private-thread codes should not be logged. The application has no explicit policy enforcing this across framework and hosting logs; review logging configuration before deployment.

**7. Private-thread codes**

Codes are stored in plain text on the in-memory thread model; the seeded example is also in source code. There is no attempt limit. Never reuse the demo code or enter real secrets; a production system needs protected code storage, rate limiting, authentication, and server-side authorization.
