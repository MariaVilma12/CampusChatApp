# Design Decisions

## Architecture

The application separates Blazor pages and components from its models and services:

- `Models` defines users, threads, posts, chats, and messages.
- `Data/AppStore` holds the in-memory application state.
- `Services` seeds demo profiles, manages the selected profile, and applies thread and chat operations.
- `Pages` and `Components` render the responsive interface.

The UI uses service classes for updates rather than writing to the store directly where practical.

## Data and demo profiles

`AppStateBootstrap` adds four demo profiles and the General, Backend C# & .NET, and private Exam Prep Study Group threads to the in-memory store. Posts and conversations start empty.

All application data is held in memory and is lost when the process restarts. Protected browser session storage remembers the selected demo profile for the current browser session; it does not authenticate a real identity.

The profile chooser is intentionally for local demonstrations only. It allows users to select any profile, including the read-only reviewer, and must not be treated as an authorization boundary or used with sensitive information.

## Threads and messaging

`ThreadService` handles public and private threads, membership, access-code checks, posts, replies, deletion, and unread counts. Private-thread access is limited to members, except that the auditor can read private threads. The auditor cannot post or reply in private threads.

`ChatService` manages one-to-one conversations stored in `AppStore`. Users can open multiple chat windows on the Messages page. Auditor profiles can review conversations but cannot send direct messages.

`CampusChatStateService` notifies open components when thread or message data changes so they can refresh their views.

## User interface

The shared layout provides the sidebar navigation and top bar. Bootstrap components are styled with the application's teal and mint theme. Profile avatars use initials and per-profile colors.

## Future improvements

For production use, replace demo profile selection and in-memory state with real authentication, authorization, and persistent storage. Additional improvements could include real-time multi-user updates, moderation tools, and accessibility testing.
