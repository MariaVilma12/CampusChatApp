# Safety Considerations

## 1. How do we prevent users from accessing information they should not see?

The application controls access through user roles. Normal users can access public threads and their own conversations. Private threads require an access code before joining. The Auditor user has special permissions to view all threads and direct chats.

---

## 2. How do we handle user input safely?

All user input is validated before being stored or displayed. Empty posts, replies, and messages are prevented. Input validation reduces the risk of invalid data being added to the application.

---

## 3. How do we protect user sessions?

The selected user session is stored using Protected Session Storage. This prevents the user selection from being lost when navigating between pages during the same browser session.

---

## 4. How do we prevent unauthorized changes?

Users can only modify their own information and content. Service classes control access to application data instead of allowing pages to directly modify stored data.

---

## 5. How do we handle private threads?

Private threads are marked separately from public threads. Users must provide the correct access code before they can view the content. This prevents unauthorized users from joining restricted discussions.

---

## 6. How do we protect application data?

The current version uses in-memory seed data, so no external database is exposed. If a database is added later, authentication, authorization, and secure database access should be implemented.

---

## 7. What improvements could be made in the future?

Future security improvements could include ASP.NET Core Identity authentication, password management, role-based authorization, database security, encrypted communication, and better audit logging.

---

## Conclusion

Security has been considered throughout the design by separating responsibilities, validating input, controlling access, and managing user sessions safely. The current solution provides basic protection suitable for the scope of this assignment.