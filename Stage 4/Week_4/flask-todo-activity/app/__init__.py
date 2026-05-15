from flask import Flask

def create_app():
    app = Flask(__name__)
    app.todos = []   #In-memory storage

    from app.routes import bp
    app.register_blueprint(bp)
    return app