from __future__ import annotations

from werkzeug.security import generate_password_hash

from models import Message, Room, User, db


def test_unread_count_calculation(app_context):
    room = Room(name="Unread Room")
    alice = User(username="alice_u", password_hash=generate_password_hash("pass123"))
    bob = User(username="bob_u", password_hash=generate_password_hash("pass123"))
    room.members.extend([alice, bob])

    db.session.add_all([room, alice, bob])
    db.session.commit()

    message_1 = Message(room_id=room.id, sender_id=alice.id, content="first", is_read=False)
    message_2 = Message(room_id=room.id, sender_id=alice.id, content="second", is_read=False)
    message_3 = Message(room_id=room.id, sender_id=bob.id, content="mine", is_read=False)
    db.session.add_all([message_1, message_2, message_3])
    db.session.commit()

    assert bob.unread_count(room.id) == 2
    assert alice.unread_count(room.id) == 1
