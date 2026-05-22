from __future__ import annotations

from werkzeug.security import generate_password_hash

from models import Room, User, db


def test_create_room_adds_creator_as_member(client, app_context):
    user = User(username="room_owner", password_hash=generate_password_hash("secret123"))
    db.session.add(user)
    db.session.commit()

    login_response = client.post(
        "/login",
        data={"username": "room_owner", "password": "secret123"},
        follow_redirects=True,
    )
    assert login_response.status_code == 200

    response = client.post(
        "/chats/create",
        data={"room_name": "Project Room"},
        follow_redirects=True,
    )

    assert response.status_code == 200
    assert b"Project Room" in response.data

    room = Room.query.filter_by(name="Project Room").first()
    assert room is not None
    assert user in room.members
