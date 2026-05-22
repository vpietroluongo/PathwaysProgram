from __future__ import annotations

import pytest

from app import create_app
from models import Room, User, db


@pytest.fixture()
def app():
    app = create_app(
        {
            "TESTING": True,
            "SECRET_KEY": "test-secret",
            "SQLALCHEMY_DATABASE_URI": "sqlite:///:memory:",
            "SQLALCHEMY_ENGINE_OPTIONS": {"connect_args": {"check_same_thread": False}},
        }
    )

    with app.app_context():
        db.drop_all()
        db.create_all()

        general = Room(name="General", is_direct=False)
        alice = User(username="alice", password_hash="pbkdf2:sha256:600000$test$hash", status="online")
        bob = User(username="bob", password_hash="pbkdf2:sha256:600000$test$hash", status="offline")
        general.members.extend([alice, bob])
        db.session.add_all([general, alice, bob])
        db.session.commit()

    yield app


@pytest.fixture()
def client(app):
    return app.test_client()


@pytest.fixture()
def app_context(app):
    with app.app_context():
        yield
