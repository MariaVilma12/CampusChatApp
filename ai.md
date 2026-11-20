## 1.What the AI produced

During the development of CampusChatApp, I used Claude and ChatGPT as programming assistants to help understand concepts, improve code quality, debug issues, and explore possible solutions. Claude was mainly used for interactive discussions about architecture, code improvements, debugging, and explaining programming concepts. ChatGPT was used for reviewing ideas, improving documentation, and helping with explanations and reflections.
Both tools helped me understand Blazor concepts, dependency injection, service-based architecture, state management, and component communication. They provided suggestions related to areas such as `ChatService`, `ThreadService`, `UserService`, validation logic, UI components, and documentation improvements.
However, the overall application architecture, folder structure, data models, and design decisions were created by me. Claude and ChatGPT were used as supporting tools to improve my understanding and development process rather than generating the complete application.

## 2. What you changed, and what you kept.

I kept some suggestions from Claude and ChatGPT that matched the design of CampusChatApp, especially ideas related to separating responsibilities between `Models`, `Services`, `Data`, `Components`, and `Pages`. The service-based approach using classes such as `UserService`, `ChatService`, and `ThreadService` was maintained because it supported a cleaner and more maintainable structure.
I also kept useful suggestions about state updates using `CampusChatStateService`, event notifications, and managing shared application data through `AppStore`.
However, I changed or rejected suggestions that did not fit the existing application design. Any generated code was reviewed, tested, and modified before being included. I made sure that shared data, such as users, threads, posts, and chats, remained inside the application store, while temporary UI data such as search text, drafts, and open chat windows stayed inside their relevant components.

## 3. Why you made that call

I made these decisions because Claude and ChatGPT do not always have the complete context of an existing project. A solution that works in a small example may not work correctly when added to a larger application with specific requirements.
For CampusChatApp, maintaining a clear separation between shared state, services, and UI components was important. I only accepted suggestions that improved the project, passed testing, and matched the original architecture. Reviewing and understanding the suggestions before applying them helped ensure that the application remained reliable, secure, and easier to maintain.

## 4. How AI has impacted you as a programmer, good or bad.

AI has had both positive and negative impacts on my programming experience. On the positive side, it helped me learn faster, understand unfamiliar concepts, debug problems, and find better solutions. It was especially useful when working with Blazor Interactive Server, service communication, state management, and improving code structure.
On the negative side, using AI without careful review can lead to problems because generated suggestions may not always match the project's requirements or may introduce unexpected issues. This experience taught me that understanding the code is more important than simply copying a solution.
Overall, AI has improved my productivity and learning process. It works best as a programming assistant that supports my decision-making, while testing, critical thinking, and knowledge of the code remain my responsibility as a developer.