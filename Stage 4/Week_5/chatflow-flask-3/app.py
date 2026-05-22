from __future__ import annotations

from pathlib import Path

from flask import Flask
from flask_login import LoginManager
from flask_socketio import SocketIO

from auth import auth_bp
from chat import chat_bp
from models import Room, User, db
from socket_events import socket_bp, register_socket_events


login_manager = LoginManager()
login_manager.login_view = "auth.login"
socketio = SocketIO(async_mode="threading")


@login_manager.user_loader
def load_user(user_id: str) -> User | None:
    return db.session.get(User, int(user_id))


def create_app(test_config: dict | None = None) -> Flask:
    app = Flask(__name__)
    app.config.update(
        SECRET_KEY="dev-secret-key",
        SQLALCHEMY_DATABASE_URI=f"sqlite:///{Path(app.root_path) / 'chat.db'}",
        SQLALCHEMY_TRACK_MODIFICATIONS=False,
    )

    if test_config:
        app.config.update(test_config)

    db.init_app(app)
    login_manager.init_app(app)
    socketio.init_app(app)

    app.register_blueprint(auth_bp)
    app.register_blueprint(chat_bp)
    app.register_blueprint(socket_bp)
    register_socket_events(socketio)

    with app.app_context():
        db.create_all()
        if Room.query.count() == 0:
            db.session.add(Room(name="General"))
            db.session.commit()

    return app


if __name__ == "__main__":
    app = create_app()
    socketio.run(app, debug=True)
