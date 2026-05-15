from flask_login import login_required, current_user
from flask import Blueprint, render_template, redirect, url_for, abort, flash
from ..models import db, Bookmark
from ..forms import BookmarkForm

main_bp = Blueprint("main", __name__)

@main_bp.route("/")
@login_required
def index():
    stmt = (
        db.select(Bookmark)
        .filter_by(owner_id=current_user.id)
        .order_by(Bookmark.created_at.desc())
    )
    bookmarks = db.session.execute(stmt).scalars().all()
    return render_template("index.html", bookmarks=bookmarks)

@main_bp.route("/bookmarks/new", methods=["GET", "POST"])
@login_required
def new():
    form = BookmarkForm()
    if form.validate_on_submit():
        bookmark = Bookmark(
            title=form.title.data,
            url=form.url.data,
            owner_id=current_user.id
        )
        db.session.add(bookmark)
        db.session.commit()
        return redirect(url_for("main.index"))
    return render_template("new.html", form=form)

@main_bp.route("/bookmarks/<int:bookmark_id>")
@login_required
def show(bookmark_id):
    bookmark = db.session.get(Bookmark, bookmark_id) 
    if not bookmark or bookmark.owner_id != current_user.id:
        abort(404)
    return render_template("show.html", bookmark=bookmark)

@main_bp.route("/bookmarks/<int:bookmark_id>/delete", methods=["POST"])
@login_required
def delete(bookmark_id):
    bookmark = db.session.get(Bookmark, bookmark_id)
    if not bookmark or bookmark.owner_id != current_user.id:
        abort(404)
    db.session.delete(bookmark)
    db.session.commit()
    flash(f"Deleted '{bookmark.title}'.", "success")
    return redirect(url_for("main.index"))

@main_bp.route("/bookmarks/<int:bookmark_id>/edit", methods=["GET", "POST"])
@login_required
def update(bookmark_id):
    bookmark = db.session.get(Bookmark, bookmark_id)
    if not bookmark or bookmark.owner_id != current_user.id:
        abort(404)
    form = BookmarkForm(obj=bookmark)
    if form.validate_on_submit():
        bookmark.title = form.title.data
        bookmark.url = form.url.data
        db.session.commit()
        flash(f"Updated '{bookmark.title}'.", "success")
        return render_template("show.html", bookmark=bookmark)
    return render_template("update.html", form=form)
    
    
    
    
    # bookmark = db.session.get(Bookmark, bookmark_id)
    # if not bookmark or bookmark.owner_id != current_user.id:
    #     abort(404)
    # data = request.get.json(silent=True) or {}
    # title = data.get("title")
    # url = data.get("url")
    # if not title or not url:
    #     return jsonify({"error": "title and url are required"}), 400

    # bookmark.title = title
    # bookmark.url = url
    # #bookmark.description = data.get("description")
    # #bookmark.tags = data.get("tags")
    # db.session.commit()
    # flash(f"Updated '{bookmark.title}'.", "success")
    # return render_template("show.html", bookmark=bookmark)