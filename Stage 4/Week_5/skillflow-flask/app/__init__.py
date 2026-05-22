from flask import Flask
from config import Config
from .models import db


app = Flask(__name__)
app.config.from_object(Config)
print(app.config.get("SQLALCHEMY_DATABASE_URI"))

db.init_app(app)

with app.app_context():
    db.create_all()
    
from . import routes
