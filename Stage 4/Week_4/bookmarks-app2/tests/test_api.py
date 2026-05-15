import base64

def auth_header(email="test@example.com", password="password123"):
    token = base64.b64encode(f"{email}:{password}".encode()).decode()
    return {"Authorization": f"Basic {token}"}

def test_api_requires_auth(client):
    response = client.get("/api/bookmarks")
    assert response.status_code == 401

def test_api_lists_bookmarks(client, user):
    response = client.get("/api/bookmarks", headers=auth_header())
    assert response.status_code == 200
    assert response.json == []

def test_api_creates_bookmark(client, user):
    response = client.post(
        "/api/bookmarks",
        headers=auth_header(),
        json={"title": "Hello", "url": "https://hello.com"},
    )
    assert response.status_code == 201
    assert response.json["title"] == "Hello"

def test_api_rejects_missing_fields(client, user):
    response = client.post(
        "/api/bookmarks", headers=auth_header(), json={"title": "x"}
    )
    assert response.status_code == 400