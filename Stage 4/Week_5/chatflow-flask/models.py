from __future__ import annotations

from datetime import datetime
from typing import Any

from flask_login import UserMixin
from flask_sqlalchemy import SQLAlchemy
from sqlalchemy import Column, ForeignKey, Table
from sqlalchemy.orm import Mapped, mapped_column, relationship

db = SQLAlchemy()

room_members = Table(
    "room_members",
    db.metadata,
    Column("room_id", ForeignKey("room.id"), primary_key=True),
    Column("user_id", ForeignKey("user.id"), primary_key=True),
)


class User(UserMixin, db.Model):
    id: Mapped[int] = mapped_column(primary_key=True)
    username: Mapped[str] = mapped_column(db.String(80), unique=True, nullable=False)
    avatar: Mapped[str] = mapped_column(db.String(200), default="default.png")
    status: Mapped[str] = mapped_column(db.String(20), default="online")
    created_at: Mapped[datetime] = mapped_column(default=datetime.utcnow)

    sent_messages: Mapped[list[Message]] = relationship(
        "Message",
        foreign_keys="Message.sender_id",
        back_populates="sender",
        lazy=True,
    )
    received_messages: Mapped[list[Message]] = relationship(
        "Message",
        foreign_keys="Message.receiver_id",
        back_populates="receiver",
        lazy=True,
    )
    rooms: Mapped[list[Room]] = relationship(
        "Room",
        secondary=room_members,
        back_populates="members",
        lazy=True,
    )

    def to_dict(self) -> dict[str, Any]:
        return {
            "id": self.id,
            "username": self.username,
            "avatar": self.avatar,
            "status": self.status,
            "created_at": self.created_at.isoformat() if self.created_at else None,
            "room_ids": [room.id for room in self.rooms],
        }


class Room(db.Model):
    id: Mapped[int] = mapped_column(primary_key=True)
    name: Mapped[str] = mapped_column(db.String(120), unique=True, nullable=False)
    created_at: Mapped[datetime] = mapped_column(default=datetime.utcnow)

    members: Mapped[list[User]] = relationship(
        "User",
        secondary=room_members,
        back_populates="rooms",
        lazy=True,
    )
    messages: Mapped[list[Message]] = relationship(
        "Message",
        back_populates="room",
        cascade="all, delete-orphan",
        lazy=True,
    )

    def to_dict(self) -> dict[str, Any]:
        return {
            "id": self.id,
            "name": self.name,
            "created_at": self.created_at.isoformat() if self.created_at else None,
            "member_ids": [member.id for member in self.members],
        }


class Message(db.Model):
    id: Mapped[int] = mapped_column(primary_key=True)
    sender_id: Mapped[int] = mapped_column(ForeignKey("user.id"), nullable=False)
    receiver_id: Mapped[int | None] = mapped_column(ForeignKey("user.id"), nullable=True)
    room_id: Mapped[int | None] = mapped_column(ForeignKey("room.id"), nullable=True)
    content: Mapped[str] = mapped_column(db.Text, nullable=False)
    timestamp: Mapped[datetime] = mapped_column(default=datetime.utcnow)
    is_read: Mapped[bool] = mapped_column(default=False)

    sender: Mapped[User] = relationship(
        "User",
        foreign_keys=[sender_id],
        back_populates="sent_messages",
        lazy=True,
    )
    receiver: Mapped[User | None] = relationship(
        "User",
        foreign_keys=[receiver_id],
        back_populates="received_messages",
        lazy=True,
    )
    room: Mapped[Room | None] = relationship("Room", back_populates="messages", lazy=True)

    def to_dict(self) -> dict[str, Any]:
        return {
            "id": self.id,
            "sender_id": self.sender_id,
            "receiver_id": self.receiver_id,
            "room_id": self.room_id,
            "content": self.content,
            "timestamp": self.timestamp.isoformat() if self.timestamp else None,
            "is_read": self.is_read,
        }

    @classmethod
    def unread_count_for_user(cls, user_id: int) -> int:
        return cls.query.filter_by(receiver_id=user_id, is_read=False).count()