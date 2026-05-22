from flask_sqlalchemy import SQLAlchemy
from datetime import datetime, timezone

db = SQLAlchemy()

class Course(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    title = db.Column(db.String(150), nullable=False)
    description = db.Column(db.Text, nullable=False)
    instructor = db.Column(db.String(100))
    level = db.Column(db.String(20))       # Beginner / Intermediate / Advanced
    duration = db.Column(db.Integer)       # hours
    thumbnail = db.Column(db.String(200))
    date_created = db.Column(db.DateTime, default=datetime.now(timezone.utc))

    def to_dict(self):
        #convert and SQLAlchemy model instance into a dictionary where keys are column names and values are their corresponding data
        return {c.name: getattr(self, c.name) for c in self.__table__.columns}

class User(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    username = db.Column(db.String)
    password = db.Column(db.String)

class UserCourse(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    user_id = db.Column(db.Integer)
    course_id = db.Column(db.Integer)

