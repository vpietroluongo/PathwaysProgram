import pytest
from app import create_app

@pytest.fixture
def client():
    app = create_app()
    app.config['TESTING'] = True
    return app.test_client()

def test_get_todos_empty(client):
    response = client.get('/todos')
    assert response.status_code == 200
    assert response.get_json() == []

def test_add_todo(client):
    response = client.post('/todos', data={'title': 'Get groceries'})

    assert response.status_code == 201
    assert response.get_json() == {
        'id': 1,
        'title': 'Get groceries',
        'completed': False
    }

    #Confirm it was stored
    all_todos = client.get('/todos')
    assert all_todos.status_code == 200
    assert all_todos.get_json() == [{
        'id': 1,
        'title': 'Get groceries',
        'completed': False
    }]

def test_add_todo_no_title(client):
    response = client.post('/todos')

    assert response.status_code == 400
    assert response.get_json() == {'error': 'title is required'}
    assert response.get_json()['error'] == 'title is required'

def test_delete_todo(client):
    response = client.delete('/todos', data={'id': 1})

    assert response.status_code == 200
    assert response.get_json() == 

def test_delete_todo_id_not_found(client):
    response = client.delete('/todos', data={'id': 4})

    assert response.status_code == 404
    assert response.get_json() == 