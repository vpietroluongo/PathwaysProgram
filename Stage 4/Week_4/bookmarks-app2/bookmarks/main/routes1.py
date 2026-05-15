from flask import Blueprint, render_template, redirect, url_for, flash, abort
from ..models import db, Bookmark
from ..forms import BookmarkForm

main_bp = Blueprint("main", __name__)

@main_bp.route("/")
def index():
    stmt = db.select(Bookmark).order_by(Bookmark.created_at.desc())
    bookmarks = db.session.execute(stmt).scalars().all()
    return render_template("index.html", bookmarks=bookmarks)

@main_bp.route("/bookmarks/<int:bookmark_id>")
def show(bookmark_id):
    bookmark = db.session.get(Bookmark, bookmark_id) or abort(404)
    return render_template("show.html", bookmark=bookmark)

@main_bp.route("/bookmarks/new", methods=["GET", "POST"])
def new():
    form = BookmarkForm()
    if form.validate_on_submit():
        bookmark = Bookmark(title=form.title.data, url=form.url.data)
        db.session.add(bookmark)
        db.session.commit()
        flash(f"Added '{bookmark.title}'.", "success")
        return redirect(url_for("main.index"))
    return render_template("new.html", form=form)

@main_bp.route("/bookmarks/<int:bookmark_id>/delete", methods=["POST"])
def delete(bookmark_id):
    bookmark = db.session.get(Bookmark, bookmark_id) or abort(404)
    db.session.delete(bookmark)
    db.session.commit()
    flash(f"Deleted '{bookmark.title}'.", "success")
    return redirect(url_for("main.index"))