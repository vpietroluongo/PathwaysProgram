from __future__ import annotations

from flask import Blueprint, abort, flash, redirect, render_template, request, url_for
from flask_login import current_user, login_required

from models import Message, Room, User, db


chat_bp = Blueprint("chat", __name__)


@chat_bp.route("/")
def index():
    if current_user.is_authenticated:
        return redirect(url_for("chat.chat_list"))
    return redirect(url_for("auth.login"))


@chat_bp.route("/chats")
@login_required
def chat_list():
    rooms = (
        Room.query.join(Room.members)
        .filter(User.id == current_user.id)
        .order_by(Room.name.asc())
        .all()
    )
    return render_template("chat_list.html", rooms=rooms)


@chat_bp.route("/chats/create", methods=["POST"])
@login_required
def create_room():
    room_name = request.form.get("room_name", "").strip()
    if not room_name:
        flash("Room name is required.", "danger")
        return redirect(url_for("chat.chat_list"))

    existing = Room.query.filter(Room.is_direct.is_(False), Room.name == room_name).first()
    if existing is not None:
        flash("A room with that name already exists.", "warning")
        return redirect(url_for("chat.chat_list"))

    room = Room(name=room_name, is_direct=False)
    room.members.append(current_user)
    db.session.add(room)
    db.session.commit()
    flash("Room created.", "success")
    return redirect(url_for("chat.chat_room", room_id=room.id))


@chat_bp.route("/chats/<int:room_id>")
@login_required
def chat_room(room_id: int):
    room = db_get_room_for_user(room_id)
    messages = (
        Message.query.filter_by(room_id=room.id)
        .order_by(Message.created_at.asc())
        .all()
    )

    for message in messages:
        if message.sender_id != current_user.id and not message.is_read:
            message.is_read = True

    db.session.commit()

    room_data = room.to_dict(current_user.id)
    return render_template(
        "chat_room.html",
        room=room,
        room_data=room_data,
        rooms=sorted(current_user.rooms, key=lambda item: item.name.lower()),
        messages=messages,
    )


@chat_bp.route("/dm/<username>")
@login_required
def start_dm(username: str):
    target = User.query.filter_by(username=username).first_or_404()
    if target.id == current_user.id:
        return redirect(url_for("chat.chat_list"))

    for room in current_user.rooms:
        if room.is_direct and {member.id for member in room.members} == {current_user.id, target.id}:
            return redirect(url_for("chat.chat_room", room_id=room.id))

    room = Room(name=f"{current_user.username} / {target.username}", is_direct=True)
    room.members.extend([current_user, target])
    db.session.add(room)
    db.session.commit()
    return redirect(url_for("chat.chat_room", room_id=room.id))


def db_get_room_for_user(room_id: int) -> Room:
    room = Room.query.get(room_id)
    if room is None or current_user not in room.members:
        abort(404)
    return room
