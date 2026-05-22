from __future__ import annotations

from flask import Flask
from flask_socketio import SocketIO

from models import Message, Room, User, db


def test_user_authentication_flow(client, app: Flask) -> None:
    response = client.get("/chat/")
    assert response.status_code == 302
    assert "/chat/login" in response.headers["Location"]

    login_response = client.post(
        "/chat/login",
        data={"username": "alice"},
        follow_redirects=False,
    )
    assert login_response.status_code == 302
    assert login_response.headers["Location"].endswith("/chat/rooms")

    with client.session_transaction() as flask_session:
        assert flask_session["_user_id"]
        assert int(flask_session["_user_id"]) > 0

    with app.app_context():
        assert User.query.filter_by(username="alice").first() is not None


def test_send_message_broadcasts_and_persists(
    app: Flask,
    client,
    socketio: SocketIO,
) -> None:
    with app.app_context():
        sender = User(username="sender")
        receiver = User(username="receiver")
        room = Room(name="general")
        room.members.extend([sender, receiver])
        db.session.add_all([sender, receiver, room])
        db.session.commit()

        sender_id = sender.id
        receiver_id = receiver.id
        room_id = room.id

    sender_socket = socketio.test_client(app, flask_test_client=client)
    receiver_socket = socketio.test_client(app, flask_test_client=client)

    sender_socket.emit("join_room", {"user_id": sender_id, "room_id": room_id})
    receiver_socket.emit("join_room", {"user_id": receiver_id, "room_id": room_id})

    sender_socket.get_received()
    receiver_socket.get_received()

    sender_socket.emit(
        "send_message",
        {
            "sender_id": sender_id,
            "room_id": room_id,
            "content": "Hello room",
        },
    )

    sender_events = sender_socket.get_received()
    receiver_events = receiver_socket.get_received()

    sender_message_events = [event for event in sender_events if event["name"] == "new_message"]
    receiver_message_events = [event for event in receiver_events if event["name"] == "new_message"]

    assert len(sender_message_events) == 1
    assert len(receiver_message_events) == 1
    assert sender_message_events[0]["args"][0]["content"] == "Hello room"
    assert receiver_message_events[0]["args"][0]["content"] == "Hello room"

    with app.app_context():
        stored = Message.query.filter_by(room_id=room_id).all()
        assert len(stored) == 1
        assert stored[0].sender_id == sender_id
        assert stored[0].content == "Hello room"


def test_unread_count_calculation(app: Flask) -> None:
    with app.app_context():
        sender = User(username="sue")
        receiver = User(username="rob")
        other_user = User(username="other")
        db.session.add_all([sender, receiver, other_user])
        db.session.commit()

        db.session.add_all(
            [
                Message(sender_id=sender.id, receiver_id=receiver.id, content="u1", is_read=False),
                Message(sender_id=sender.id, receiver_id=receiver.id, content="u2", is_read=False),
                Message(sender_id=sender.id, receiver_id=receiver.id, content="r1", is_read=True),
                Message(sender_id=sender.id, receiver_id=other_user.id, content="other", is_read=False),
            ]
        )
        db.session.commit()

        assert Message.unread_count_for_user(receiver.id) == 2
        assert Message.unread_count_for_user(other_user.id) == 1
