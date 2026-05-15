from flask import Flask, render_template, redirect, url_for, flash, abort, request
from models import db, Bookmark
from forms import BookmarkForm
from flask_wtf.csrf import CSRFProtect

app = Flask(__name__)
app.config["SECRET_KEY"] = "dev-only-change-me"
app.config["SQLALCHEMY_DATABASE_URI"] = "sqlite:///bookmarks.db"
csrf = CSRFProtect(app)
db.init_app(app)

with app.app_context():
    db.create_all()

@app.route("/")
def index():
    tag = (request.args.get("tag") or "").strip().lower()
    stmt = db.select(Bookmark).order_by(Bookmark.created_at.desc())
    if tag:
        #stmt = stmt.filter(Bookmark.tags.contains(tag))
        stmt = stmt.filter(Bookmark.tags.ilike(f"%{tag}%"))
    bookmarks = db.session.execute(stmt).scalars().all()
    return render_template("index.html", bookmarks=bookmarks, tag=tag)

@app.route("/bookmarks/<int:bookmark_id>")
def show(bookmark_id):
    bookmark = db.session.get(Bookmark, bookmark_id) or abort(404)
    return render_template("show.html", bookmark=bookmark)

@app.route("/bookmarks/new", methods=["GET", "POST"])
def new():
    form = BookmarkForm()
    if form.validate_on_submit():
        bookmark = Bookmark(
            title=form.title.data, 
            url=form.url.data,
            description=form.description.data,
            tags=normalize_tags(form.tags.data))
        db.session.add(bookmark)
        db.session.commit()
        flash(f"Added '{bookmark.title}'.", "success")
        return redirect(url_for("index"))
    return render_template("new.html", form=form)

@app.route("/bookmarks/<int:bookmark_id>/delete", methods=["POST"])
def delete(bookmark_id):
    bookmark = db.session.get(Bookmark, bookmark_id) or abort(404)
    db.session.delete(bookmark)
    db.session.commit()
    flash(f"Deleted '{bookmark.title}'.", "success")
    return redirect(url_for("index"))

@app.errorhandler(404)
def page_not_found(error):
    print("Int error handler")
    return render_template("404.html"), 404

def normalize_tags(raw):
    if not raw:
        return None
    parts = [t.strip().lower() for t in raw.split(",") if t.strip()]
    return ",".join(dict.fromkeys(parts)) or None