# CampusChatApp

CampusChatApp is a Blazor Web App for a campus community. It supports public and private discussion threads, posts and replies, direct messaging, editable demo profiles, unread counters, and a community auditor.

## Demo profiles and data

Choose one of the supplied profiles on the sign-in page, or create a student demo profile from the welcome page. Demo profiles include students, lecturer Milo Jensen, and a community auditor.

The app starts with three campus threads: **General**, **Backend C# & .NET**, and the private **Exam Prep Study Group**. Posts and direct conversations start empty.

The auditor profile can read private threads and review all direct conversations. Auditor access is read-only for private-thread posts and direct messages.

The private demo thread uses invite code `secret123`. It is only a disposable classroom example, not a real secret.

Use **People** in the navigation to search for another profile and see its joined
threads. Private thread names and memberships are visible on profiles, as they
are in the thread list; viewing their posts still requires access to the thread.

The home and thread-list pages show unread post counts for joined threads,
including zero. Opening a thread clears its count; new posts from other profiles
increase it until they are read. Replies are not counted as posts.

## Requirements

- .NET 10 SDK and a modern web browser.
- Rider or Visual Studio is optional; the .NET CLI can also run the app.
- No database server is required.

## Run

From the repository root, run:

```powershell
dotnet run --project CampusChatApp --launch-profile http
```

Open `http://localhost:5200`, then choose or create a demo profile. Alternatively, open `CampusChatApp.sln` in Rider or Visual Studio and run the `CampusChatApp` project.

## Limitations

- **No real authentication or password recovery:** anyone can select any demo profile, including the auditor. Thread codes and profile selection are not a security boundary. Do not enter real or sensitive information.
- **No persistent storage:** profiles, conversations, threads, and unread counts are lost when the app restarts.
- Profile deletion and post anonymisation, abuse reporting, moderation, and evidence preservation are not implemented.
- The create-thread form does not provide a description field. `/counter` and `/weather` are placeholders, not functional features.
- Live updates use in-process notifications and are not designed for multiple server instances.

## Test

Run the test suite with:

```powershell
dotnet test CampusChatApp.sln
```
