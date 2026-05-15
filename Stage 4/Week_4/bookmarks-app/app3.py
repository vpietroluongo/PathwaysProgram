from flask import Flask, render_template, request, jsonify, redirect, url_for, abort

app = Flask(__name__)

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

@app.errorhandler(404)
def page_not_found(error):
    print("Int error handler")
    return render_template("404.html"), 404