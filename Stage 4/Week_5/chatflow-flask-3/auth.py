from __future__ import annotations

from flask import Blueprint, flash, redirect, render_template, request, url_for
from flask_login import current_user, login_required, login_user, logout_user
from werkzeug.security import check_password_hash, generate_password_hash

from models import Room, User, db


auth_bp = Blueprint("auth", __name__)


@auth_bp.route("/register", methods=["GET", "POST"])
def register():
    if current_user.is_authenticated:
        return redirect(url_for("chat.chat_list"))

    if request.method == "POST":
        username = request.form.get("username", "").strip()
        password = request.form.get("password", "")

        if not username or not password:
            flash("Username and password are required.", "danger")
            return render_template("register.html")

        existing = User.query.filter_by(username=username).first()
        if existing:
            flash("Username is already taken.", "danger")
            return render_template("register.html")

        user = User(username=username, password_hash=generate_password_hash(password))
        for room in Room.query.filter_by(is_direct=False).all():
            user.rooms.append(room)
        db.session.add(user)
        db.session.commit()
        login_user(user)
        return redirect(url_for("chat.chat_list"))

    return render_template("register.html")


@auth_bp.route("/login", methods=["GET", "POST"])
def login():
    if current_user.is_authenticated:
        return redirect(url_for("chat.chat_list"))

    if request.method == "POST":
        username = request.form.get("username", "").strip()
        password = request.form.get("password", "")

        user = User.query.filter_by(username=username).first()
        if user is None or not check_password_hash(user.password_hash, password):
            flash("Invalid username or password.", "danger")
            return render_template("login.html")

        login_user(user)
        return redirect(url_for("chat.chat_list"))

    return render_template("login.html")


@auth_bp.route("/logout")
@login_required
def logout():
    logout_user()
    return redirect(url_for("auth.login"))
