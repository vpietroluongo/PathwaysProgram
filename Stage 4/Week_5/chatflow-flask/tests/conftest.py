from __future__ import annotations

import pytest
from flask import Flask
from flask_socketio import SocketIO

from app import login_manager
from chat_blueprint import chat_bp, register_chat_socket_handlers
from models import db


@pytest.fixture()
def app() -> Flask:
    app = Flask(__name__)
    app.config.update(
        SECRET_KEY="test-secret",
        TESTING=True,
        SQLALCHEMY_DATABASE_URI="sqlite:///:memory:",
        SQLALCHEMY_TRACK_MODIFICATIONS=False,
    )

    db.init_app(app)
    login_manager.init_app(app)
    app.register_blueprint(chat_bp)

    socketio = SocketIO(app, async_mode="threading", logger=False, engineio_logger=False)
    register_chat_socket_handlers(socketio)
    app.extensions["test_socketio"] = socketio

    with app.app_context():
        db.create_all()
        yield app
        db.session.remove()
        db.drop_all()


@pytest.fixture()
def client(app: Flask):
    return app.test_client()


@pytest.fixture()
def socketio(app: Flask) -> SocketIO:
    return app.extensions["test_socketio"]
