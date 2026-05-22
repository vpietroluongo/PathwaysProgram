from __future__ import annotations

from app.chat.routes import calculate_unread_count
from app.extensions import db
from models import Message, Room, User


def test_register_login_logout_flow(client):
    response = client.post(
        "/register",
        data={
            "username": "newuser",
            "password": "password123",
            "confirm_password": "password123",
        },
        follow_redirects=True,
    )
    assert response.status_code == 200
    assert b"Account created" in response.data

    login_response = client.post(
        "/login",
        data={"username": "newuser", "password": "password123"},
        follow_redirects=True,
    )
    assert login_response.status_code == 200
    assert b"Rooms" in login_response.data

    logout_response = client.get("/logout", follow_redirects=True)
    assert logout_response.status_code == 200
    assert b"Log in" in logout_response.data


def test_send_message_socket_event(app, seed_users, socket_clients):
    with app.app_context():
        room = db.session.get(Room, seed_users["general"])

    socket_clients["alice"].emit("join_room", {"room_id": room.id})
    socket_clients["bob"].emit("join_room", {"room_id": room.id})

    socket_clients["alice"].emit("send_message", {"room_id": room.id, "content": "hello bob"})

    received = socket_clients["bob"].get_received()
    message_events = [event for event in received if event["name"] == "new_message"]
    assert message_events, "Expected at least one new_message event"
    assert message_events[-1]["args"][0]["content"] == "hello bob"


def test_typing_indicator_event(app, seed_users, socket_clients):
    with app.app_context():
        room = db.session.get(Room, seed_users["general"])

    socket_clients["alice"].emit("join_room", {"room_id": room.id})
    socket_clients["bob"].emit("join_room", {"room_id": room.id})

    socket_clients["alice"].emit("typing", {"room_id": room.id, "is_typing": True})

    received = socket_clients["bob"].get_received()
    typing_events = [event for event in received if event["name"] == "typing"]
    assert typing_events
    payload = typing_events[-1]["args"][0]
    assert payload["is_typing"] is True
    assert payload["username"] == "alice"


def test_unread_count_calculation(app, room_with_messages):
    with app.app_context():
        room = db.session.get(Room, room_with_messages["room"])
        bob = db.session.get(User, room_with_messages["bob"])
        first = db.session.get(Message, room_with_messages["first"])

        assert calculate_unread_count(room, bob) == 2

        first.read_by.append(bob)
        db.session.commit()

        assert calculate_unread_count(room, bob) == 1


def test_message_history_loaded_from_database(app, authenticated_clients, room_with_messages):
    response = authenticated_clients["bob"].get(f"/chat/?room={room_with_messages['room']}")
    assert response.status_code == 200
    assert b"First" in response.data
    assert b"Second" in response.data
