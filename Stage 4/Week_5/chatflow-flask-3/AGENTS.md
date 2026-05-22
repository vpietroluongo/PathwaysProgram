# AGENTS.md

This file defines implementation requirements for AI coding agents working on this Flask chat project.

## Objective

Build a complete real-time Flask chat application with:

1. Authentication (Flask-Login): register and login
2. Chat Rooms / Direct Messages using Flask-SocketIO
3. Message history loaded from database
4. Real-time typing indicator using SocketIO events
5. Sidebar, online status, and dark mode toggle

## Prompt-Based Delivery Plan

### Prompt 1 - Models & Database

Create complete SQLAlchemy models for:

- User
- Message
- Room

Requirements:

- Proper relationships between all models
- `to_dict()` method on each model for API/template use
- Python typing annotations on fields/methods

### Prompt 2 - SocketIO Routes

Build Flask-SocketIO event handlers in a Blueprint for:

- `send_message`
- `join_room`
- `typing`

Requirements:

- Correct room joining behavior
- Proper broadcasting to room participants
- Payloads compatible with frontend updates

### Prompt 3 - Main Routes + Templates

Generate Flask routes and Jinja2 templates for:

- Chat list
- Chat window
- Login

Requirements:

- Use Bootstrap 5 styling
- Keep templates modular and readable
- Ensure routes enforce authentication where required

### Prompt 4 - Tests

Write pytest tests for:

- Message sending
- User authentication
- Unread count calculation

Requirements:

- Include fixtures for a test database
- Keep tests deterministic and isolated
- Validate both success and expected failure cases

### Prompt 5 - Full Feature Integration

Integrate all parts into a complete real-time chat experience with:

- Sidebar navigation
- Message history rendering
- Online status display
- Dark mode toggle

## Implementation Constraints

- Keep changes focused and reviewable.
- Preserve existing behavior unless explicitly requested to change it.
- Do not add secrets or environment-specific credentials.
- Prefer clear, maintainable code over unnecessary abstraction.

## Quality Bar

- SocketIO events work in real time across clients.
- Message history persists and reloads correctly.
- Auth-protected pages redirect unauthenticated users.
- Tests pass for core chat/auth/unread workflows.
- UI works on desktop and mobile viewports.

## Suggested Execution Order

1. Models and migrations
2. Auth routes/forms
3. Chat routes/templates
4. SocketIO events
5. Integration polish (sidebar, online status, dark mode)
6. Tests and final verification

## Definition of Done

The task is complete when:

- All five prompt sections are implemented.
- Core features are functional end-to-end.
- Relevant pytest checks pass.
- Changes are summarized with any known limitations.
