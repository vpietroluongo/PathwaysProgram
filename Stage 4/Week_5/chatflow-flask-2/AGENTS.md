## Task
Implement a real-time, multi-user, multi-room chat application using Python
 
## Context
- Follow application factory app pattern
- We use Python, Flask routes, Blueprints, Jinja2 templates, SQLAlchemy models with relationships, Flask-WTF, Flask-Login, Flask-SocketIO, Pytest
-Use Bootstrap 5 for styling
 
## Acceptance criteria
- [ ] Flask routes and Jinja2 templates used for chat list, chat window, and login
- [ ] Flask-SocketIO event handlers used for 'send_message', 'join_room', and 'typing' in a Blueprint. Include proper broadcasting.
- [ ] Integration tests cover all paths
- [ ] Tests included for message sending, user authentication, and unread count calculation, including fixtures for test database
- [ ] SQLAlchemy models for a Flask chat app include User, Message, and Room. Include proper relationships and a to_dict() method. Use Python typing.
- [ ] Full feature has a sidebar, message history, online status, and dark mode toggle.
- [ ] Register users and login users using Flask-Login
- [ ] Chat rooms and direct messages use Flask-SocketIO
- [ ] Message history is loaded from database
- [ ] SocketIO events are used for a real-time typing indicator
 
## Don't
- Don't add new packages
