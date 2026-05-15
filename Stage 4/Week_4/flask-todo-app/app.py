from flask import Flask, render_template, request, redirect, url_for

app = Flask(__name__)

# In-memory todo list
todos = []

@app.route('/')
def index():
    return render_template('index.html', todos=todos)

@app.route('/add', methods=['POST'])
def add():
    title = request.form.get('title')
    if title:
        todos.append({
            'id': len(todos) + 1,
            'title': title,
            'completed': False
        })
    return redirect(url_for('index'))

@app.route('/toggle/<int:todo_id>')
def toggle(todo_id):
    todo = next((t for t in todos if t['id'] == todo_id), None)
    if todo:
        todo['completed'] = not todo['completed']
    return redirect(url_for('index'))

if __name__ == '__main__':
    app.run(debug=True)