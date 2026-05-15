from bookmarks.models import db, Bookmark, User

def test_create_bookmark(auth_client, app, user):
    response = auth_client.post(
        "/bookmarks/new",
        data={"title": "Test", "url": "https://example.com"},
        follow_redirects=True,
    )
    assert response.status_code == 200
    # with app.app_context():
    #     count = db.session.execute(
    #         db.select(Bookmark).filter_by(owner_id=user.id)
    #     ).scalars().all()
    #     assert len(count) == 1
    #     assert count[0].title == "Test"
    
    with app.app_context():
        current = db.session.execute(
            db.select(User).filter_by(email="test@example.com")
        ).scalar_one()

        count = db.session.execute(
            db.select(Bookmark).filter_by(owner_id=current.id)
        ).scalars().all()
        assert len(count) == 1
        assert count[0].title == "Test"

def test_cannot_see_others_bookmarks(auth_client, app):
    with app.app_context():
        from bookmarks.models import User
        other = User(email="other@example.com")
        other.set_password("pw12345678")
        db.session.add(other)
        db.session.flush()
        db.session.add(Bookmark(
            title="Theirs", url="https://x.com", owner_id=other.id
        ))
        db.session.commit()
        bookmark_id = db.session.execute(
            db.select(Bookmark).filter_by(owner_id=other.id)
        ).scalar_one().id
    response = auth_client.get(f"/bookmarks/{bookmark_id}")
    assert response.status_code == 404