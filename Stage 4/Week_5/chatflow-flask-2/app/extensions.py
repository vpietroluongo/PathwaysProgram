from __future__ import annotations

from flask_login import LoginManager
from flask_socketio import SocketIO
from flask_sqlalchemy import SQLAlchemy


db = SQLAlchemy()
login_manager = LoginManager()
login_manager.login_view = "auth.login"
socketio = SocketIO(async_mode="threading")


@login_manager.user_loader
def load_user(user_id: str):
    from models import User

    return db.session.get(User, int(user_id))
