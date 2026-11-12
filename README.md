# CampusChatApp

CampusChatApp is a Blazor Web App for a campus community. It supports public and private discussion threads, posts and replies, direct messaging, editable demo profiles, unread counters, and a read-only community auditor.

## Demo profiles and data

Choose one of the supplied profiles on the sign-in page. The profile chooser is for demonstrations only; it is not authentication, and anyone using the app can select any profile. Do not enter real or sensitive information.

Profiles, threads, memberships, posts, chats, messages, and unread counts are held in memory and reset when the application restarts. The app starts with demo profiles and three campus threads: **General**, **Backend C# & .NET**, and the private **Exam Prep Study Group**. Posts and direct conversations start empty.

The auditor profile can read private threads and review all direct conversations. Auditor access is read-only for private-thread posts and direct messages.

The private demo thread uses invite code `secret123`. It is only a disposable classroom example, not a real secret.

## Requirements

- .NET 10 SDK (or the version required by the course)
- JetBrains Rider or Visual Studio
- A modern web browser

No database server is required.

## Run

1. Open `CampusChatApp.sln` in Rider or Visual Studio.
2. Restore NuGet packages.
3. Run the `CampusChatApp` project.
4. Choose a demo profile when prompted.

The app is typically available at `http://localhost:5200`.

## Test

Run the test suite with:

```powershell
dotnet test CampusChatApp.sln
```

## Technologies

- ASP.NET Core Blazor Interactive Server
- C# and .NET 10
- Bootstrap

Developed as part of the **Arbeidskrav 2** coursework.
