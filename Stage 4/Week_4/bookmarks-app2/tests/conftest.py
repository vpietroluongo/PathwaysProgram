import pytest
from bookmarks import create_app
from bookmarks.config import TestConfig
from bookmarks.models import db, User

@pytest.fixture
def app():
    app = create_app(TestConfig)
    yield app

@pytest.fixture
def client(app):
    return app.test_client()

@pytest.fixture
def user(app):
    with app.app_context():
        u = User(email="test@example.com")
        u.set_password("password123")
        db.session.add(u)
        db.session.commit()
        # Detach so the instance can be used outside the session
        #db.session.expunge(u)
        return u

@pytest.fixture
def auth_client(client, user):
    client.post(
        "/auth/login",
        data={"email": "test@example.com", "password": "password123"},
    )
    return client