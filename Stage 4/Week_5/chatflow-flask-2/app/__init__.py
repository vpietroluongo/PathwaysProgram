from __future__ import annotations

from flask import Flask, redirect, url_for
from flask_login import current_user

from app.extensions import db, login_manager, socketio


def create_app(test_config: dict | None = None) -> Flask:
    app = Flask(__name__)
    app.config.from_mapping(
        SECRET_KEY="dev-secret-key",
        SQLALCHEMY_DATABASE_URI="sqlite:///chat.db",
        SQLALCHEMY_TRACK_MODIFICATIONS=False,
        WTF_CSRF_ENABLED=True,
    )

    if test_config:
        app.config.update(test_config)

    db.init_app(app)
    login_manager.init_app(app)
    socketio.init_app(app)

    from app.auth.routes import auth_bp
    from app.chat.routes import chat_bp, register_socketio_handlers

    app.register_blueprint(auth_bp)
    app.register_blueprint(chat_bp, url_prefix="/chat")
    register_socketio_handlers(socketio)

    @app.route("/")
    def home() -> str:
        if current_user.is_authenticated:
            return redirect(url_for("chat.index"))
        return redirect(url_for("auth.login"))

    with app.app_context():
        db.create_all()

    return app
