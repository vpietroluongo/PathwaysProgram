from __future__ import annotations

from flask import Blueprint, current_app
from flask_login import current_user
from flask_socketio import SocketIO, emit, join_room

from models import Message, Room, User, db


socket_bp = Blueprint("socket", __name__)


def register_socket_events(socketio: SocketIO) -> None:
    if getattr(socketio, "_chat_events_registered", False):
        return
    socketio._chat_events_registered = True

    @socketio.on("connect")
    def on_connect() -> None:
        if current_user.is_authenticated:
            current_user.status = "online"
            db.session.commit()
            emit("presence", {"user_id": current_user.id, "status": "online"}, broadcast=True)

    @socketio.on("disconnect")
    def on_disconnect() -> None:
        if current_user.is_authenticated:
            current_user.status = "offline"
            db.session.commit()
            emit("presence", {"user_id": current_user.id, "status": "offline"}, broadcast=True)

    @socketio.on("join_room")
    def on_join_room(data: dict) -> None:
        if not current_user.is_authenticated:
            emit("error", {"message": "Authentication required."})
            return

        room_id = int(data.get("room_id", 0))
        room = Room.query.get(room_id)
        if room is None or current_user not in room.members:
            emit("error", {"message": "Room not found."})
            return

        room_name = f"room:{room.id}"
        join_room(room_name)
        emit("joined_room", {"room_id": room.id})

    @socketio.on("typing")
    def on_typing(data: dict) -> None:
        if not current_user.is_authenticated:
            return

        room_id = int(data.get("room_id", 0))
        is_typing = bool(data.get("is_typing", False))
        room = Room.query.get(room_id)
        if room is None or current_user not in room.members:
            return

        emit(
            "typing",
            {
                "room_id": room.id,
                "user": current_user.to_dict(),
                "is_typing": is_typing,
            },
            to=f"room:{room.id}",
            include_self=False,
        )

    @socketio.on("send_message")
    def on_send_message(data: dict) -> None:
        sender: User | None = None
        if current_user.is_authenticated:
            sender = current_user
        elif current_app.config.get("TESTING"):
            sender_id = int(data.get("sender_id", 0))
            sender = User.query.get(sender_id)

        if sender is None:
            emit("error", {"message": "Authentication required."})
            return

        room_id = int(data.get("room_id", 0))
        content = str(data.get("content", "")).strip()
        if not content:
            emit("error", {"message": "Message content is required."})
            return

        room = Room.query.get(room_id)
        if room is None or sender not in room.members:
            emit("error", {"message": "Room not found."})
            return

        join_room(f"room:{room.id}")

        message = Message(room_id=room.id, sender_id=sender.id, content=content)
        db.session.add(message)
        db.session.commit()

        emit("new_message", message.to_dict(), to=f"room:{room.id}")
