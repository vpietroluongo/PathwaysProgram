from __future__ import annotations

import tempfile
from pathlib import Path

import pytest

from app import create_app
from app.extensions import db, socketio
from models import Message, Room, User


@pytest.fixture
def app():
    db_file = Path(tempfile.mkdtemp()) / "test.db"
    app = create_app(
        {
            "TESTING": True,
            "SQLALCHEMY_DATABASE_URI": f"sqlite:///{db_file}",
            "SECRET_KEY": "test-secret",
            "WTF_CSRF_ENABLED": False,
        }
    )

    with app.app_context():
        db.drop_all()
        db.create_all()
        yield app
        db.session.remove()
        db.drop_all()


@pytest.fixture
def client(app):
    return app.test_client()


@pytest.fixture
def seed_users(app):
    with app.app_context():
        alice = User(username="alice")
        alice.set_password("password123")
        bob = User(username="bob")
        bob.set_password("password123")
        db.session.add_all([alice, bob])
        db.session.commit()

        general = Room(name="General", is_direct=False)
        general.members.append(alice)
        general.members.append(bob)
        db.session.add(general)
        db.session.commit()

        return {"alice": alice.id, "bob": bob.id, "general": general.id}


def _login(client, username: str, password: str = "password123"):
    return client.post(
        "/login",
        data={"username": username, "password": password},
        follow_redirects=True,
    )


@pytest.fixture
def authenticated_clients(app, seed_users):
    alice_client = app.test_client()
    bob_client = app.test_client()

    _login(alice_client, "alice")
    _login(bob_client, "bob")

    return {"alice": alice_client, "bob": bob_client}


@pytest.fixture
def socket_clients(app, authenticated_clients):
    alice_socket = socketio.test_client(app, flask_test_client=authenticated_clients["alice"])
    bob_socket = socketio.test_client(app, flask_test_client=authenticated_clients["bob"])

    yield {"alice": alice_socket, "bob": bob_socket}

    alice_socket.disconnect()
    bob_socket.disconnect()


@pytest.fixture
def room_with_messages(app, seed_users):
    with app.app_context():
        room = db.session.get(Room, seed_users["general"])
        alice = db.session.get(User, seed_users["alice"])
        bob = db.session.get(User, seed_users["bob"])

        m1 = Message(room_id=room.id, sender_id=alice.id, content="First")
        m2 = Message(room_id=room.id, sender_id=alice.id, content="Second")
        db.session.add_all([m1, m2])
        db.session.flush()
        m1.read_by.append(alice)
        m2.read_by.append(alice)
        db.session.commit()

        return {"room": room.id, "alice": alice.id, "bob": bob.id, "first": m1.id}
