from __future__ import annotations

from typing import Any

from flask import Blueprint, abort, flash, redirect, render_template, request, url_for
from flask_login import current_user, login_required
from flask_socketio import SocketIO, emit, join_room as socket_join_room

from app.chat.forms import MessageForm
from app.extensions import db
from models import Message, Room, User


chat_bp = Blueprint("chat", __name__)
ONLINE_USERS: set[int] = set()


def calculate_unread_count(room: Room, user: User) -> int:
    return (
        Message.query.filter(Message.room_id == room.id, Message.sender_id != user.id)
        .filter(~Message.read_by.any(User.id == user.id))
        .count()
    )


def _room_or_404(room_id: int) -> Room:
    room = db.session.get(Room, room_id)
    if room is None:
        abort(404)
    if not room.members.filter_by(id=current_user.id).first():
        abort(403)
    return room


def _get_or_create_general_room() -> Room:
    room = Room.query.filter_by(is_direct=False, name="General").first()
    if room is None:
        room = Room(name="General", is_direct=False)
        db.session.add(room)
        db.session.commit()
    if not room.members.filter_by(id=current_user.id).first():
        room.members.append(current_user)
        db.session.commit()
    return room


def _get_or_create_direct_room(current: User, target: User) -> Room:
    low_id, high_id = sorted([current.id, target.id])
    dm_key = f"{low_id}:{high_id}"

    room = Room.query.filter_by(is_direct=True, dm_key=dm_key).first()
    if room is None:
        room = Room(name=f"{current.username} and {target.username}", is_direct=True, dm_key=dm_key)
        room.members.append(current)
        room.members.append(target)
        db.session.add(room)
        db.session.commit()
    return room


def _mark_messages_read(room: Room, user: User) -> None:
    unread_messages = (
        Message.query.filter(Message.room_id == room.id, Message.sender_id != user.id)
        .filter(~Message.read_by.any(User.id == user.id))
        .all()
    )
    for message in unread_messages:
        message.read_by.append(user)
    if unread_messages:
        db.session.commit()


@chat_bp.get("/")
@login_required
def index():
    _get_or_create_general_room()

    rooms = current_user.rooms.order_by(Room.created_at.desc()).all()
    selected_room = None
    room_id = request.args.get("room", type=int)

    if room_id:
        selected_room = db.session.get(Room, room_id)
        if selected_room and not selected_room.members.filter_by(id=current_user.id).first():
            selected_room = None
    if selected_room is None and rooms:
        selected_room = rooms[-1]

    messages = []
    if selected_room:
        messages = selected_room.messages.order_by(Message.timestamp.asc()).all()
        _mark_messages_read(selected_room, current_user)

    unread_counts = {room.id: calculate_unread_count(room, current_user) for room in rooms}
    online_users = User.query.filter_by(status="online").order_by(User.username.asc()).all()

    form = MessageForm()
    return render_template(
        "chat/index.html",
        form=form,
        rooms=rooms,
        active_room=selected_room,
        messages=messages,
        unread_counts=unread_counts,
        online_users=online_users,
    )


@chat_bp.get("/rooms/<int:room_id>")
@login_required
def room(room_id: int):
    return redirect(url_for("chat.index", room=room_id))


@chat_bp.post("/rooms/<int:room_id>/messages")
@login_required
def post_message(room_id: int):
    room = _room_or_404(room_id)
    form = MessageForm()
    if not form.validate_on_submit():
        flash("Message cannot be empty", "warning")
        return redirect(url_for("chat.index", room=room.id))

    message = Message(room_id=room.id, sender_id=current_user.id, content=form.content.data.strip())
    db.session.add(message)
    db.session.flush()
    message.read_by.append(current_user)
    db.session.commit()

    return redirect(url_for("chat.index", room=room.id))


@chat_bp.post("/direct/<int:user_id>")
@login_required
def direct_message(user_id: int):
    target = db.session.get(User, user_id)
    if target is None or target.id == current_user.id:
        abort(404)

    room = _get_or_create_direct_room(current_user, target)
    return redirect(url_for("chat.index", room=room.id))


def register_socketio_handlers(socketio: SocketIO) -> None:
    @socketio.on("connect")
    def on_connect() -> bool | None:
        if not current_user.is_authenticated:
            return False

        ONLINE_USERS.add(current_user.id)
        user = db.session.get(User, current_user.id)
        if user:
            user.status = "online"
            db.session.commit()

        emit("status_update", {"user_id": current_user.id, "status": "online"}, broadcast=True)
        return None

    @socketio.on("disconnect")
    def on_disconnect() -> None:
        if not current_user.is_authenticated:
            return

        ONLINE_USERS.discard(current_user.id)
        user = db.session.get(User, current_user.id)
        if user:
            user.status = "offline"
            db.session.commit()

        emit("status_update", {"user_id": current_user.id, "status": "offline"}, broadcast=True)

    @socketio.on("join_room")
    def on_join_room(data: dict[str, Any]) -> None:
        if not current_user.is_authenticated:
            return

        room_id = int(data.get("room_id", 0))
        room = db.session.get(Room, room_id)
        if room is None:
            return
        if not room.members.filter_by(id=current_user.id).first():
            return

        room_token = str(room.id)
        socket_join_room(room_token)
        emit("room_joined", {"room_id": room.id, "user_id": current_user.id}, to=room_token)

    @socketio.on("typing")
    def on_typing(data: dict[str, Any]) -> None:
        if not current_user.is_authenticated:
            return

        room_id = int(data.get("room_id", 0))
        room = db.session.get(Room, room_id)
        if room is None or not room.members.filter_by(id=current_user.id).first():
            return

        emit(
            "typing",
            {
                "room_id": room_id,
                "user_id": current_user.id,
                "username": current_user.username,
                "is_typing": bool(data.get("is_typing", False)),
            },
            to=str(room_id),
            include_self=False,
        )

    @socketio.on("send_message")
    def on_send_message(data: dict[str, Any]) -> None:
        if not current_user.is_authenticated:
            return

        room_id = int(data.get("room_id", 0))
        content = str(data.get("content", "")).strip()
        if not content:
            return

        room = db.session.get(Room, room_id)
        if room is None or not room.members.filter_by(id=current_user.id).first():
            return

        message = Message(room_id=room.id, sender_id=current_user.id, content=content)
        db.session.add(message)
        db.session.flush()
        message.read_by.append(current_user)
        db.session.commit()

        emit("new_message", message.to_dict(), to=str(room.id))
