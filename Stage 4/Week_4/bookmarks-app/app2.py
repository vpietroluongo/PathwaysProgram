from flask import Flask, request, jsonify, redirect, url_for

app = Flask(__name__)

#In-memory store
bookmarks = [
    {"id": 1, "title": "Flask docs", "url": "https://flask.palletsprojects.com"},
    {"id": 2, "title": "Python docs", "url": "https://docs.python.org"}
]

@app.route("/")
def index():
    links = "".join(
        f'<li><a href="{b["url"]}">{b["title"]}</a> '
        f'(<a href="/bookmarks/{b["id"]}">details</a>)</li>'
        for b in bookmarks 
    )
    return f"<h1>Bookmarks</h1><ul>{links}</ul>"

@app.route("/bookmarks/<int:bookmark_id>")
def show(bookmark_id):
    match = next((b for b in bookmarks if b["id"] == bookmark_id), None)
    if match is None:
        return "Not found", 404
    return f"<h1>{match['title']}</h1><p>{match['url']}</p>"

@app.route("/search")
def search():
    query = request.args.get("q", "").lower()
    results = [b for b in bookmarks if query in b["title"].lower()]
    return jsonify(results)

@app.route("/bookmarks", methods=["POST"])
def create():
    title = request.form["title"]
    url = request.form["url"]
    new_id = max((b["id"] for b in bookmarks), default=0) + 1
    bookmarks.append({"id": new_id, "title": title, "url": url})
    return redirect(url_for("index"))

@app.route("/bookmarks/<int:bookmark_id>", methods=["DELETE"])
def delete(bookmark_id):
    match = next((b for b in bookmarks if b["id"] == bookmark_id), None)
    if match is None:
        return "No Content", 204
    bookmarks.remove(match)
    return jsonify(bookmarks)
