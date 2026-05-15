from flask import Blueprint, request, jsonify, abort
from flask_httpauth import HTTPBasicAuth
from ..models import db, User, Bookmark

api_bp = Blueprint("api", __name__, url_prefix="/api")
auth = HTTPBasicAuth()

@auth.verify_password
def verify(email, password):
    user = db.session.execute(
        db.select(User).filter_by(email=email)
    ).scalar_one_or_none()
    if user and user.check_password(password):
        return user
    return None

def serialize(bookmark: Bookmark) -> dict:
    return {
        "id": bookmark.id,
        "title": bookmark.title,
        "url": bookmark.url,
        "created_at": bookmark.created_at.isoformat()
    }

@api_bp.get("/bookmarks")
@auth.login_required
def list_bookmarks():
    user = auth.current_user()
    stmt = db.select(Bookmark).filter_by(owner_id=user.id)
    items = db.session.execute(stmt).scalars().all()
    return jsonify([serialize(b) for b in items])

@api_bp.get("/bookmarks/<int:bookmark_id>")
@auth.login_required
def get_bookmark(bookmark_id):
    user = auth.current_user()
    bookmark = db.session.get(Bookmark, bookmark_id)
    if not bookmark or bookmark.owner_id != user.id:
        abort(404)
    return jsonify(serialize(bookmark))

@api_bp.post("/bookmarks")
@auth.login_required
def create_bookmark():
    user = auth.current_user()
    data = request.get_json(silent=True) or {}
    title = data.get("title")
    url = data.get("url")
    if not title or not url:
        return jsonify({"error": "title and url are required"}), 400
    bookmark = Bookmark(title=title, url=url, owner_id=user.id)
    db.session.add(bookmark)
    db.session.commit()
    return jsonify(serialize(bookmark)), 201

@api_bp.delete("/bookmarks/<int:bookmark_id>")
@auth.login_required
def delete_bookmark(bookmark_id):
    user = auth.current_user()
    bookmark = db.session.get(Bookmark, bookmark_id)
    if not bookmark or bookmark.owner_id != user.id:
        abort(404)
    db.session.delete(bookmark)
    db.session.commit()
    return "", 204

@api_bp.put("/bookmarks/<int:bookmark_id>")
@auth.login_required
def update_bookmark(bookmark_id):
    user = auth.current_user()
    bookmark = db.session.get(Bookmark, bookmark_id)
    if not bookmark or bookmark.owner_id != user.id:
        abort(404)
    data = request.get.json(silent=True) or {}
    title = data.get("title")
    url = data.get("url")
    if not title or not url:
        return jsonify({"error": "title and url are required"}), 400

    bookmark.title = title
    bookmark.url = url
    #bookmark.description = data.get("description")
    #bookmark.tags = data.get("tags")
    db.session.commit()
    return jsonify(serialize(bookmark)), 200