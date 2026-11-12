# Safety Considerations

## Demo identity and access

The sign-in screen selects a preloaded demo profile; it does not verify a real identity. Anyone who can reach the app can choose any profile, including the auditor profile. The auditor view and private-thread checks are classroom-demo behavior, not a security boundary.

Do not expose this app to untrusted networks or use real personal, confidential, or sensitive information.

## Data and private threads

Profiles, threads, access codes, posts, chats, messages, memberships, and unread counts are stored in memory and disappear when the application restarts. The seeded private thread uses the public demo code `secret123`; it is kept in memory as plain text. Never reuse this code or enter real secrets.

## Input handling

The app validates required thread, post, reply, and message fields and enforces length limits. Blazor encodes displayed text by default. These checks improve application behavior but do not replace production-grade security controls.

## Production requirements

Before production use, replace profile selection with real authentication and server-side authorization, persist data with appropriate access controls, hash private-thread secrets, and add audit logging and abuse protections.
