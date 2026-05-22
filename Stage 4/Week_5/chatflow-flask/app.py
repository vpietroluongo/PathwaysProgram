from __future__ import annotations

from flask import Flask, redirect, url_for
from flask_login import LoginManager
from flask_socketio import SocketIO

from chat_blueprint import chat_bp, register_chat_socket_handlers
from models import User, db

socketio = SocketIO(cors_allowed_origins="*")
login_manager = LoginManager()
login_manager.login_view = "chat.login"


@login_manager.user_loader
def load_user(user_id: str) -> User | None:
    return db.session.get(User, int(user_id))


def create_app() -> Flask:
    app = Flask(__name__)
    app.config.update(
        SECRET_KEY="dev-secret-change-me",
        SQLALCHEMY_DATABASE_URI="sqlite:///chat.db",
        SQLALCHEMY_TRACK_MODIFICATIONS=False,
    )

    db.init_app(app)
    login_manager.init_app(app)
    app.register_blueprint(chat_bp)

    socketio.init_app(app)
    register_chat_socket_handlers(socketio)

    with app.app_context():
        db.create_all()

    @app.route("/")
    def index():
        return redirect(url_for("chat.chat_list"))

    return app


app = create_app()


if __name__ == "__main__":
    socketio.run(app, host="127.0.0.1", port=5000, debug=True)
