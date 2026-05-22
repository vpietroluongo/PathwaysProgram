from __future__ import annotations

from datetime import datetime, UTC
from typing import Any

from flask_login import UserMixin
from flask_sqlalchemy import SQLAlchemy
from sqlalchemy import ForeignKey, Integer, String, Text
from sqlalchemy.orm import Mapped, mapped_column, relationship


db = SQLAlchemy()


room_members = db.Table(
    "room_members",
    db.Column("room_id", db.Integer, db.ForeignKey("room.id"), primary_key=True),
    db.Column("user_id", db.Integer, db.ForeignKey("user.id"), primary_key=True),
)


class User(UserMixin, db.Model):
    id: Mapped[int] = mapped_column(Integer, primary_key=True)
    username: Mapped[str] = mapped_column(String(80), unique=True, nullable=False)
    password_hash: Mapped[str] = mapped_column(String(255), nullable=False)
    avatar: Mapped[str] = mapped_column(String(200), default="default.png", nullable=False)
    status: Mapped[str] = mapped_column(String(20), default="offline", nullable=False)

    sent_messages: Mapped[list[Message]] = relationship(
        "Message",
        foreign_keys="Message.sender_id",
        back_populates="sender",
        cascade="all, delete-orphan",
    )
    rooms: Mapped[list[Room]] = relationship(
        "Room",
        secondary=room_members,
        back_populates="members",
    )

    def unread_count(self, room_id: int) -> int:
        return Message.query.filter(
            Message.room_id == room_id,
            Message.sender_id != self.id,
            Message.is_read.is_(False),
        ).count()

    def to_dict(self) -> dict[str, Any]:
        return {
            "id": self.id,
            "username": self.username,
            "avatar": self.avatar,
            "status": self.status,
        }


class Room(db.Model):
    id: Mapped[int] = mapped_column(Integer, primary_key=True)
    name: Mapped[str] = mapped_column(String(120), nullable=False)
    is_direct: Mapped[bool] = mapped_column(default=False, nullable=False)

    members: Mapped[list[User]] = relationship(
        "User",
        secondary=room_members,
        back_populates="rooms",
    )
    messages: Mapped[list[Message]] = relationship(
        "Message",
        back_populates="room",
        cascade="all, delete-orphan",
        order_by="Message.created_at.asc()",
    )

    def to_dict(self, current_user_id: int | None = None) -> dict[str, Any]:
        unread = 0
        if current_user_id is not None:
            unread = Message.query.filter(
                Message.room_id == self.id,
                Message.sender_id != current_user_id,
                Message.is_read.is_(False),
            ).count()

        return {
            "id": self.id,
            "name": self.name,
            "is_direct": self.is_direct,
            "members": [member.to_dict() for member in self.members],
            "unread_count": unread,
        }


class Message(db.Model):
    id: Mapped[int] = mapped_column(Integer, primary_key=True)
    room_id: Mapped[int] = mapped_column(ForeignKey("room.id"), nullable=False)
    sender_id: Mapped[int] = mapped_column(ForeignKey("user.id"), nullable=False)
    content: Mapped[str] = mapped_column(Text, nullable=False)
    created_at: Mapped[datetime] = mapped_column(default=lambda: datetime.now(UTC), nullable=False)
    is_read: Mapped[bool] = mapped_column(default=False, nullable=False)

    room: Mapped[Room] = relationship("Room", back_populates="messages")
    sender: Mapped[User] = relationship("User", foreign_keys=[sender_id], back_populates="sent_messages")

    def to_dict(self) -> dict[str, Any]:
        return {
            "id": self.id,
            "room_id": self.room_id,
            "sender": self.sender.to_dict(),
            "content": self.content,
            "created_at": self.created_at.isoformat(),
            "is_read": self.is_read,
        }