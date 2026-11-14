# Design Decisions

## Architecture

The Blazor Interactive Server application separates pages and components from models and services:

- `Models` defines users, threads, posts, chats, and messages.
- `Data/AppStore` holds the in-memory application state.
- `Services` seeds demo profiles, manages the selected profile, and applies thread and chat operations.
- `Pages` and `Components` render the responsive interface.

Shared user, membership, thread, post, unread, and chat data lives in the store and is accessed through services and, in some pages, directly. `ThreadService` applies thread operations and unread tracking; `UserService` manages profiles; and `ChatService` manages conversations. The shared `CampusChatStateService` signals changes to subscribed components. These shared services let different browser sessions see the same demo data without requiring each component to own a separate copy.

`SessionService` is scoped to the current Blazor circuit and uses protected browser session storage to remember the selected profile ID. Transient interface state, such as search text, draft posts, entered codes, and open chat windows, stays in the corresponding component because it belongs to that view rather than all users.

## Data and demo profiles

`AppStateBootstrap` adds four demo profiles and the General, Backend C# & .NET, and private Exam Prep Study Group threads to `AppStore`. Posts and conversations start empty. Users can also create student demo profiles.

Profiles, memberships, posts, unread state, and conversations are shared in server memory while the process runs and are lost on restart. This keeps the demonstration self-contained without a database. Browser session storage holds only the selected profile ID, not the application data.

The profile chooser is intentionally for demonstrations only. It allows users to select any profile, including the read-only reviewer, and does not authenticate a real identity. Private-thread codes and the in-app access rules are not a security boundary; do not use the app for sensitive information.

## Layout and routes

The shared `MainLayout` provides a sidebar and top bar across the application. Separate screens keep profile selection, browsing, and individual conversations focused while preserving consistent navigation. The main routes are:

- `/` and `/welcome`: welcome page; `/signin`: select a demo profile; `/signup` and `/profile/create`: create one.
- `/home`: signed-in overview; `/people`: find profiles; `/profile` and `/profile/edit`: view and edit the selected profile; `/profiles/{id}` and `/profiles/{id}/threads`: another profile and its joined threads.
- `/threads`, `/threads/create`, `/threads/{id}`, and `/threads/private`: browse, create, read, and join threads.
- `/chats`, `/chats/start`, and `/chats/{id}`: browse people, start a conversation, and view a chat; `/audit`: review conversations.
- `/forgot-password` explains that demo profiles have no password; `/error` and `/not-found` handle error pages. `/counter` and `/weather` are template example routes.

Names use short resource-based paths and IDs for individual items, making a profile, thread, or chat directly addressable. The welcome and profile-creation aliases offer alternative entry points.

Bootstrap cards, badges, and responsive columns keep the interface familiar at different window sizes. Profile initials and chosen avatar colours distinguish people without image uploads. A signed-in user can search the People page or follow a post author or chat recipient to a profile and see that person's joined threads, without needing to know an ID.

## Threads, posts, and counts

A thread has a name, an optional description, a last-updated time for ordering, an optional private access code, and a collection of posts. The seeded threads have descriptions; the create-thread form asks for a name and access type, not a description. There is no separate topic field, thread colour, or owner: the name and available description convey the topic, and avoiding ownership keeps joining and posting independent of who created the thread.

In addition to text, author, and time, a post has an ID and a list of replies. The ID identifies the post for replies and deletion; replies keep discussion together under the original post. Replies have their own ID, author, text, and time. Only the author can delete their post.

All thread names are visible in the thread list. The **total post count** is also shown for every listed thread, including unjoined and private threads, to give a sense of activity. The count reveals activity volume but not post text; private thread content remains behind the in-app access check. Profile pages also show joined thread names.

The **unread count** is different: it is shown only for threads the selected profile has joined, including when it is zero, because unread posts are personal membership state rather than a global activity measure. Each profile tracks unread post IDs per thread. New posts by other profiles count as unread for members; the author's own posts and replies do not. Opening a joined thread clears its count; posts received while it is open are marked read, and deleting an unread post removes it from the count. Leaving a thread discards its unread state. The home page and thread list update through the shared change notification.

Ordinary profiles can read public threads without joining and can post and reply there; private threads require a code to join, after which members can read and post. The auditor can read private threads without joining but cannot post or reply there. This exception supports review while distinguishing it from participation; it does not make the entire auditor profile read-only in public threads.

## Direct conversations and updates

`ChatService` stores one conversation per pair of users, reusing an existing chat instead of creating duplicates. The Messages page opens multiple small chat windows, so users can follow several conversations without leaving the people list. The auditor's review page lists conversations and opens them read-only; ordinary users see their own conversations.

Changes made through the interface notify subscribed components via `CampusChatStateService`, so open thread and chat views can refresh without a manual reload. This is an in-process notification mechanism, not durable messaging or a guarantee across multiple server instances.

## Future improvements

For production use, replace demo profile selection and in-memory state with real authentication, authorization, and persistent storage. Additional improvements could include more robust multi-user synchronization, moderation tools, and accessibility testing.
