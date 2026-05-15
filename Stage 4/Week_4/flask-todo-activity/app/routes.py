from flask import Blueprint, jsonify, request, current_app

bp = Blueprint('todos', __name__)

@bp.route('/todos', methods=['GET'])
def get_todos():
    #Return all todos as JSON
    return jsonify(current_app.todos), 200

@bp.route('/todos', methods=['POST'])
def add_todos():
    #Add a new todo.  Return 400 is no title is provided.
    title = request.form.get('title')

    if not title:
        return jsonify({'error': 'title is required'}), 400
    todo = {
        'id': len(current_app.todos) + 1,
        'title': title,
        'completed': False
    }
    current_app.todos.append(todo)
    return jsonify(todo), 201

@bp.route('/todos/<int:todo_id>', methods=['DELETE'])
def delete_todo(todo_id):
    #Delete todo by id.  Return 404 is not found.
    
    return jsonify({'error': 'todo not found'}), 404