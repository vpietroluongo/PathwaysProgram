from __future__ import annotations

from datetime import datetime
from typing import Any

from flask_login import UserMixin
from werkzeug.security import check_password_hash, generate_password_hash

from app.extensions import db


room_members = db.Table(
    "room_members",
    db.Column("room_id", db.Integer, db.ForeignKey("room.id"), primary_key=True),
    db.Column("user_id", db.Integer, db.ForeignKey("user.id"), primary_key=True),
)

message_reads = db.Table(
    "message_reads",
    db.Column("message_id", db.Integer, db.ForeignKey("message.id"), primary_key=True),
    db.Column("user_id", db.Integer, db.ForeignKey("user.id"), primary_key=True),
)


class User(UserMixin, db.Model):
    id: int = db.Column(db.Integer, primary_key=True)
    username: str = db.Column(db.String(80), unique=True, nullable=False)
    password_hash: str = db.Column(db.String(255), nullable=False)
    avatar: str = db.Column(db.String(200), default="default.png", nullable=False)
    status: str = db.Column(db.String(20), default="offline", nullable=False)

    rooms = db.relationship("Room", secondary=room_members, back_populates="members", lazy="dynamic")
    sent_messages = db.relationship("Message", back_populates="sender", lazy="dynamic")
    read_messages = db.relationship("Message", secondary=message_reads, back_populates="read_by", lazy="dynamic")

    def set_password(self, password: str) -> None:
        self.password_hash = generate_password_hash(password)

    def check_password(self, password: str) -> bool:
        return check_password_hash(self.password_hash, password)

    def to_dict(self) -> dict[str, Any]:
        return {
            "id": self.id,
            "username": self.username,
            "avatar": self.avatar,
            "status": self.status,
        }


class Room(db.Model):
    id: int = db.Column(db.Integer, primary_key=True)
    name: str = db.Column(db.String(120), nullable=False)
    is_direct: bool = db.Column(db.Boolean, default=False, nullable=False)
    dm_key: str | None = db.Column(db.String(64), unique=True, nullable=True)
    created_at: datetime = db.Column(db.DateTime, default=datetime.utcnow, nullable=False)

    members = db.relationship("User", secondary=room_members, back_populates="rooms", lazy="dynamic")
    messages = db.relationship("Message", back_populates="room", lazy="dynamic", cascade="all, delete-orphan")

    def to_dict(self) -> dict[str, Any]:
        return {
            "id": self.id,
            "name": self.name,
            "is_direct": self.is_direct,
            "member_count": self.members.count(),
            "created_at": self.created_at.isoformat(),
        }


class Message(db.Model):
    id: int = db.Column(db.Integer, primary_key=True)
    room_id: int = db.Column(db.Integer, db.ForeignKey("room.id"), nullable=False)
    sender_id: int = db.Column(db.Integer, db.ForeignKey("user.id"), nullable=False)
    content: str = db.Column(db.Text, nullable=False)
    timestamp: datetime = db.Column(db.DateTime, default=datetime.utcnow, nullable=False)

    room = db.relationship("Room", back_populates="messages")
    sender = db.relationship("User", back_populates="sent_messages")
    read_by = db.relationship("User", secondary=message_reads, back_populates="read_messages", lazy="dynamic")

    def to_dict(self) -> dict[str, Any]:
        return {
            "id": self.id,
            "room_id": self.room_id,
            "sender_id": self.sender_id,
            "sender": self.sender.username,
            "content": self.content,
            "timestamp": self.timestamp.isoformat(),
        }