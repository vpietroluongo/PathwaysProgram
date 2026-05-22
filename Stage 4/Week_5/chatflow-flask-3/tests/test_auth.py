from __future__ import annotations

from werkzeug.security import generate_password_hash

from models import User, db


def test_login_success(client, app_context):
    user = User(username="valid_user", password_hash=generate_password_hash("secret123"))
    db.session.add(user)
    db.session.commit()

    response = client.post(
        "/login",
        data={"username": "valid_user", "password": "secret123"},
        follow_redirects=True,
    )

    assert response.status_code == 200
    assert b"Your Rooms" in response.data


def test_login_failure(client, app_context):
    user = User(username="valid_user", password_hash=generate_password_hash("secret123"))
    db.session.add(user)
    db.session.commit()

    response = client.post(
        "/login",
        data={"username": "valid_user", "password": "wrong"},
        follow_redirects=True,
    )

    assert response.status_code == 200
    assert b"Invalid username or password" in response.data
