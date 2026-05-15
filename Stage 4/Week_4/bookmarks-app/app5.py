from flask import Flask, render_template, redirect, url_for, abort, flash
#from flask_wtf import FlaskForm 
from forms import BookmarkForm
from flask_wtf.csrf import CSRFProtect

app = Flask(__name__)
app.config["SECRET_KEY"] = "dev-only-change-me"
csrf = CSRFProtect(app)

#In-memory store
bookmarks = [
    {"id": 1, "title": "Flask docs", "url": "https://flask.palletsprojects.com"},
    {"id": 2, "title": "Python docs", "url": "https://docs.python.org"}
]

@app.route("/")
def index():
    return render_template("index.html", bookmarks=bookmarks)

@app.route("/bookmarks/<int:bookmark_id>")
def show(bookmark_id):
    match = next((b for b in bookmarks if b["id"] == bookmark_id), None)
    if match is None:
        abort(404)
    return render_template("show.html", bookmark=match)

@app.route("/bookmarks/new", methods=["GET", "POST"])
def new():
    form = BookmarkForm()
    if form.validate_on_submit():
        new_id = max((b["id"] for b in bookmarks), default=0) + 1
        bookmarks.append({
            "id": new_id,
            "title": form.title.data,
            "url": form.url.data,
            "description": form.description.data
        })
        flash(f"Added '{form.title.data}'.", "success")
        return redirect(url_for("index"))
    return render_template("new.html", form=form)

@app.route("/bookmarks/<int:bookmark_id>/delete", methods=["POST"])
def delete(bookmark_id):
    match = next((b for b in bookmarks if b["id"] == bookmark_id), None)
    if match is None:
        abort(404)
    bookmarks.remove(match)
    flash(f"Deleted '{match["title"]}'.", "success")
    return redirect(url_for("index"))

@app.errorhandler(404)
def page_not_found(error):
    print("Int error handler")
    return render_template("404.html"), 404
