from __future__ import annotations

from werkzeug.security import generate_password_hash

from models import Message, Room, User, db


def test_message_sending_persists_to_database(app_context):
    room = Room(name="Socket Room")
    sender = User(username="sender", password_hash=generate_password_hash("pass123"))
    receiver = User(username="receiver", password_hash=generate_password_hash("pass123"))
    room.members.extend([sender, receiver])
    db.session.add_all([room, sender, receiver])
    db.session.commit()

    message = Message(room_id=room.id, sender_id=sender.id, content="hello world")
    db.session.add(message)
    db.session.commit()

    saved = Message.query.filter_by(room_id=room.id, sender_id=sender.id).first()
    assert saved is not None
    assert saved.content == "hello world"
