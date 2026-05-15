def test_register_creates_user(client):
    response = client.post(
        "/auth/register",
        data={
            "email": "new@example.com",
            "password": "password123",
            "confirm": "password123",
        },
        follow_redirects=True,
    )
    assert response.status_code == 200

def test_login_with_wrong_password_fails(client, user):
    response = client.post(
        "/auth/login",
        data={"email": "test@example.com", "password": "wrong"},
    )
    assert b"Invalid credentials" in response.data

def test_index_requires_login(client):
    response = client.get("/", follow_redirects=False)
    assert response.status_code == 302
    assert "/auth/login" in response.headers["Location"]