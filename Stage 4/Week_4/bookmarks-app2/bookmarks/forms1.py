from flask_wtf import FlaskForm
from wtforms import StringField, SubmitField
from wtforms.validators import DataRequired, URL, Length

class BookmarkForm(FlaskForm):
    title = StringField(
        "Title",
        validators=[DataRequired(), Length(max=200)]
    )
    url = StringField(
        "URL",
        validators=[DataRequired(), URL(message="Must be a valid URL")]
    )
    submit = SubmitField("Save")