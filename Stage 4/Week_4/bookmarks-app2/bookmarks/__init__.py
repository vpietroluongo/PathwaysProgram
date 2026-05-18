from flask import Flask
from flask_login import LoginManager
from .config import Config
from .models import db, User
from flask_wtf.csrf import CSRFProtect
#from flask_migrate import Migrate

login_manager = LoginManager()
login_manager.login_view = "auth.login"
csrf = CSRFProtect()
#migrate = Migrate()

#adding small change to test workflow onyl runs when pushing from Week_4 

@login_manager.user_loader
def load_user(user_id):
    return db.session.get(User, int(user_id))

def create_app(config_class=Config):
    app = Flask(__name__)
    app.config.from_object(config_class)

    db.init_app(app)
    #migrate.init_app(app, db)
    login_manager.init_app(app)
    csrf.init_app(app)

    from .main.routes import main_bp
    from .auth.routes import auth_bp
    app.register_blueprint(main_bp)
    app.register_blueprint(auth_bp)

    from .api.routes import api_bp
    csrf.exempt(api_bp)
    app.register_blueprint(api_bp)

    ### NOTE: db.create_all() is gone.  Use `flask db upgrade` instead.  Removed so that is does not silently create any missing tables on every app start, bypassing migrations
    with app.app_context():
         db.create_all()
    
    return app