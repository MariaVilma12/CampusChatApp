# Safety note

**1. Personal data**

The app holds profile details, posts, replies, direct messages, thread memberships, and unread post IDs. It does not record when a user joined or last read a thread. Do not enter real personal or sensitive data.

**2. Chat retention**

Chats exist only in server memory and disappear on restart. A possible future policy is deletion one year after the last message; this is not implemented.

**3. A student leaves**

Profile deletion and post anonymisation are not implemented. A future option is to delete the profile and retain posts under “Former student”, subject to an agreed retention policy.

**4. Abuse**

Authors can delete their own posts; there is no reporting, moderator removal, or evidence-preservation feature. A production system needs a reporting and review process that protects evidence.

**5. Direct chat privacy**

The chat page checks whether a selected profile is a participant or the auditor. These are demo checks, not a security boundary: anyone can select any profile, including the seeded Community Reviewer. The app does not notify participants of auditor access, and messages are not encrypted in memory. Do not expose it to untrusted networks.

**6. Logging**

Message text and private-thread codes should not be logged. The application has no explicit policy enforcing this across framework and hosting logs; review logging configuration before deployment.

**7. Private-thread codes**

Codes are stored in plain text on the in-memory thread model; the seeded example is also in source code. There is no attempt limit. Never reuse the demo code or enter real secrets; a production system needs protected code storage, rate limiting, authentication, and server-side authorization.
