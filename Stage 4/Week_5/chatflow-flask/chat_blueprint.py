from __future__ import annotations

from collections import defaultdict
from datetime import datetime
from typing import Any

from flask import Blueprint, flash, redirect, render_template, request, url_for
from flask_login import current_user, login_required, login_user, logout_user
from flask_socketio import SocketIO, emit, join_room

from models import Message, Room, User, db

chat_bp = Blueprint("chat", __name__, url_prefix="/chat")
_sid_to_user: dict[str, int] = {}
_user_connection_counts: dict[int, int] = defaultdict(int)


@chat_bp.route("/login", methods=["GET", "POST"])
def login() -> str:
    if current_user.is_authenticated:
        return redirect(url_for("chat.chat_list"))

    if request.method == "POST":
        username = (request.form.get("username") or "").strip()
        if not username:
            flash("Username is required.", "danger")
            return render_template("login.html")

        user = User.query.filter_by(username=username).first()
        if user is None:
            user = User(username=username)
            db.session.add(user)
        user.status = "online"
        db.session.commit()

        login_user(user)
        return redirect(url_for("chat.chat_list"))

    return render_template("login.html")


@chat_bp.route("/logout", methods=["POST"])
@login_required
def logout() -> Any:
    current_user.status = "offline"
    db.session.commit()
    logout_user()
    return redirect(url_for("chat.login"))


@chat_bp.route("/", methods=["GET", "POST"])
@chat_bp.route("/rooms", methods=["GET", "POST"])
@login_required
def chat_list() -> Any:
    user = current_user

    if request.method == "POST":
        room_name = (request.form.get("room_name") or "").strip()
        if not room_name:
            flash("Room name cannot be empty.", "danger")
            return redirect(url_for("chat.chat_list"))

        room = Room.query.filter_by(name=room_name).first()
        if room is None:
            room = Room(name=room_name)
            room.members.append(user)
            db.session.add(room)
            db.session.commit()
            flash(f"Created room '{room_name}'.", "success")
        else:
            if user not in room.members:
                room.members.append(user)
                db.session.commit()
            flash(f"Joined room '{room_name}'.", "info")

        return redirect(url_for("chat.chat_window", room_id=room.id))

    rooms = Room.query.order_by(Room.name.asc()).all()
    users = User.query.filter(User.id != user.id).order_by(User.username.asc()).all()
    return render_template("chat_list.html", user=user, rooms=rooms, users=users)


@chat_bp.route("/rooms/<int:room_id>", methods=["GET"])
@login_required
def chat_window(room_id: int) -> Any:
    user = current_user

    room = db.session.get(Room, room_id)
    if room is None:
        flash("Room not found.", "danger")
        return redirect(url_for("chat.chat_list"))

    if user not in room.members:
        room.members.append(user)
        db.session.commit()

    messages = (
        Message.query.filter(Message.room_id == room.id)
        .order_by(Message.timestamp.asc())
        .all()
    )

    return render_template(
        "chat_window.html",
        user=user,
        chat_mode="room",
        room=room,
        rooms=Room.query.order_by(Room.name.asc()).all(),
        users=User.query.filter(User.id != user.id).order_by(User.username.asc()).all(),
        active_peer=None,
        messages=messages,
    )


@chat_bp.route("/dm/<int:peer_id>", methods=["GET"])
@login_required
def direct_message_window(peer_id: int) -> Any:
    user = current_user

    peer = db.session.get(User, peer_id)
    if peer is None or peer.id == user.id:
        flash("User not found.", "danger")
        return redirect(url_for("chat.chat_list"))

    messages = (
        Message.query.filter(
            Message.room_id.is_(None),
            (
                ((Message.sender_id == user.id) & (Message.receiver_id == peer.id))
                | ((Message.sender_id == peer.id) & (Message.receiver_id == user.id))
            ),
        )
        .order_by(Message.timestamp.asc())
        .all()
    )

    return render_template(
        "chat_window.html",
        user=user,
        chat_mode="direct",
        room=None,
        rooms=Room.query.order_by(Room.name.asc()).all(),
        users=User.query.filter(User.id != user.id).order_by(User.username.asc()).all(),
        active_peer=peer,
        messages=messages,
    )


def _room_channel(room_id: int) -> str:
    return f"room:{room_id}"


def _user_channel(user_id: int) -> str:
    return f"user:{user_id}"


def _emit_online_users() -> None:
    online_users = sorted(_user_connection_counts.keys())
    emit("online_users", {"user_ids": online_users}, broadcast=True)


def register_chat_socket_handlers(socketio: SocketIO) -> None:
    @socketio.on("connect")
    def handle_connect() -> None:
        emit("connected", {"message": "Socket connected."})

    @socketio.on("join_room")
    def handle_join_room(payload: dict[str, Any]) -> None:
        user_id = payload.get("user_id")
        room_id = payload.get("room_id")
        username = payload.get("username", "Anonymous")

        if user_id is None:
            emit("error", {"message": "'user_id' is required."})
            return

        try:
            user_id_int = int(user_id)
        except (TypeError, ValueError):
            emit("error", {"message": "'user_id' must be an integer."})
            return

        # Personal channel for direct messages/typing events.
        join_room(_user_channel(user_id_int))

        sid = request.sid
        previous_user_id = _sid_to_user.get(sid)
        if previous_user_id != user_id_int:
            if previous_user_id is not None:
                _user_connection_counts[previous_user_id] -= 1
                if _user_connection_counts[previous_user_id] <= 0:
                    _user_connection_counts.pop(previous_user_id, None)

            _sid_to_user[sid] = user_id_int
            _user_connection_counts[user_id_int] += 1

            user = db.session.get(User, user_id_int)
            if user is not None:
                user.status = "online"
                db.session.commit()
            _emit_online_users()

        if room_id is None:
            emit("joined_room", {"user_id": user_id_int, "channel": _user_channel(user_id_int)})
            return

        try:
            room_id_int = int(room_id)
        except (TypeError, ValueError):
            emit("error", {"message": "'room_id' must be an integer."})
            return

        channel = _room_channel(room_id_int)
        join_room(channel)

        emit(
            "joined_room",
            {"user_id": user_id_int, "room_id": room_id_int, "channel": channel},
        )
        emit(
            "user_joined",
            {
                "user_id": user_id_int,
                "username": str(username),
                "room_id": room_id_int,
                "joined_at": datetime.utcnow().isoformat(),
            },
            to=channel,
            include_self=False,
        )

    @socketio.on("send_message")
    def handle_send_message(payload: dict[str, Any]) -> None:
        sender_id = payload.get("sender_id")
        content = payload.get("content")
        room_id = payload.get("room_id")
        receiver_id = payload.get("receiver_id")

        if sender_id is None or not isinstance(content, str) or not content.strip():
            emit("error", {"message": "'sender_id' and non-empty 'content' are required."})
            return

        if room_id is None and receiver_id is None:
            emit("error", {"message": "Provide either 'room_id' or 'receiver_id'."})
            return

        try:
            sender_id_int = int(sender_id)
        except (TypeError, ValueError):
            emit("error", {"message": "'sender_id' must be an integer."})
            return

        room_id_int: int | None = None
        receiver_id_int: int | None = None

        if room_id is not None:
            try:
                room_id_int = int(room_id)
            except (TypeError, ValueError):
                emit("error", {"message": "'room_id' must be an integer."})
                return

        if receiver_id is not None:
            try:
                receiver_id_int = int(receiver_id)
            except (TypeError, ValueError):
                emit("error", {"message": "'receiver_id' must be an integer."})
                return

        message = Message(
            sender_id=sender_id_int,
            receiver_id=receiver_id_int,
            room_id=room_id_int,
            content=content.strip(),
        )
        db.session.add(message)
        db.session.commit()

        message_payload = message.to_dict()
        message_payload["sender_username"] = message.sender.username

        if room_id_int is not None:
            emit("new_message", message_payload, to=_room_channel(room_id_int))
            return

        if receiver_id_int is not None:
            emit("new_message", message_payload, to=_user_channel(receiver_id_int))
            emit("new_message", message_payload, to=_user_channel(sender_id_int))

    @socketio.on("typing")
    def handle_typing(payload: dict[str, Any]) -> None:
        user_id = payload.get("user_id")
        room_id = payload.get("room_id")
        receiver_id = payload.get("receiver_id")
        is_typing = bool(payload.get("is_typing", True))

        if user_id is None:
            emit("error", {"message": "'user_id' is required."})
            return

        try:
            user_id_int = int(user_id)
        except (TypeError, ValueError):
            emit("error", {"message": "'user_id' must be an integer."})
            return

        user = db.session.get(User, user_id_int)
        typing_payload = {
            "user_id": user_id_int,
            "username": user.username if user is not None else f"User #{user_id_int}",
            "is_typing": is_typing,
        }

        if room_id is not None:
            try:
                room_id_int = int(room_id)
            except (TypeError, ValueError):
                emit("error", {"message": "'room_id' must be an integer."})
                return

            emit("typing", typing_payload, to=_room_channel(room_id_int), include_self=False)
            return

        if receiver_id is not None:
            try:
                receiver_id_int = int(receiver_id)
            except (TypeError, ValueError):
                emit("error", {"message": "'receiver_id' must be an integer."})
                return

            emit("typing", typing_payload, to=_user_channel(receiver_id_int))
            return

        emit("error", {"message": "Provide either 'room_id' or 'receiver_id'."})

    @socketio.on("disconnect")
    def handle_disconnect() -> None:
        sid = request.sid
        user_id = _sid_to_user.pop(sid, None)
        if user_id is None:
            return

        _user_connection_counts[user_id] -= 1
        if _user_connection_counts[user_id] <= 0:
            _user_connection_counts.pop(user_id, None)
            user = db.session.get(User, user_id)
            if user is not None:
                user.status = "offline"
                db.session.commit()

        _emit_online_users()
