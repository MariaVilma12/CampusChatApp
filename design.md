# Design Decisions

## Introduction

This document describes the main design decisions made during the development of the Campus Chat application. The focus was to create a simple, maintainable, and modular Blazor application that satisfies the assignment requirements while remaining easy to understand and extend.

---

# Application Architecture

The application follows a layered structure by separating models, services, data, and user interface components.

```
CampusChatApp
│
├── Components
├── Models
├── Services
├── Data
├── Helpers
└── wwwroot
```

This structure keeps the project organized and makes each part responsible for a single task.

- **Models** store the application's data.
- **Services** contain the application logic.
- **Data** provides in-memory seed data.
- **Components and Pages** display the user interface.
- **Helpers** can contain utility methods if needed later.

Keeping these responsibilities separate makes the application easier to maintain and test.

---

# Data Storage

The assignment does not require a database. Therefore, the application stores all data in memory using a `SeedData` class.

This approach has several advantages:

- Easy to implement
- No database installation required
- Fast development
- Suitable for a demonstration project

The disadvantage is that all data is lost when the application stops. This is acceptable because persistence is not required for the assignment.

---

# Service Layer

Instead of accessing the seed data directly from the pages, the application uses service classes.

The project includes:

- UserService
- ThreadService
- ChatService
- SessionService

Each page communicates with these services rather than directly modifying the data.

This provides a clear separation between the user interface and the application's logic. If a database is added in the future, only the services would need to change while the pages could remain mostly unchanged.

---

# Dependency Injection

The services are registered in `Program.cs` using ASP.NET Core Dependency Injection.

This allows Blazor components to request the services they need through constructor or property injection.

Using Dependency Injection makes the code more modular and reduces coupling between classes.

---

# User Interface Design

The interface uses Bootstrap because it is included with Blazor and provides responsive components without additional libraries.

The navigation menu contains the main sections of the application:

- Home
- Profile
- Threads
- Direct Chats
- Sign Out

This layout keeps navigation simple and allows users to move easily between features.

---

# Model Design

The application uses separate models for different types of information.

The main models are:

- User
- Thread
- Post
- Reply
- DirectChat
- ChatMessage
- UnreadCounter

Separating the models improves readability and makes each class responsible for a single concept.

For example:

- A `Thread` contains discussion posts.
- A `Post` contains replies.
- A `DirectChat` contains chat messages.
- `UnreadCounter` tracks unread posts and messages.

---

# Session Management

The currently signed-in user is managed through a session service together with Protected Session Storage.

This allows the selected user to remain signed in while navigating between pages without requiring authentication or a database.

This approach satisfies the assignment requirement for remembering the selected user during the session.

---

# Reusable Components

Where possible, the application will use reusable Blazor components instead of duplicating code.

Examples include:

- Post cards
- Chat message display
- Thread cards
- Navigation menu

Reusable components reduce duplicated code and make future maintenance easier.

---

# Error Handling

The application performs basic validation before accepting user input.

Examples include:

- Preventing empty posts
- Preventing empty replies
- Validating private thread access codes
- Checking that a user is signed in before accessing protected pages

Providing simple validation improves usability and prevents invalid data.

---

# Future Improvements

If the application were developed further, several improvements could be made:

- Store data in a SQL database
- Add ASP.NET Core Identity authentication
- Upload profile pictures
- Real-time messaging using SignalR
- Better search and filtering
- Notification system
- Responsive mobile improvements

---

# Conclusion

The design focuses on simplicity, readability, and maintainability. By separating responsibilities into models, services, data, and user interface components, the application is easier to understand and extend. The chosen architecture also aligns well with recommended ASP.NET Core Blazor development practices and satisfies the assignment requirements.