from flask_wtf import FlaskForm
from wtforms import StringField, SubmitField, TextAreaField
from wtforms.validators import DataRequired, URL, Length, Optional

class BookmarkForm(FlaskForm):
    title = StringField(
        "Title",
        validators=[DataRequired(), Length(max=200)]
    )
    url = StringField(
        "URL",
        validators=[DataRequired(), URL(message="Must be a valid URL")]
    )
    description = TextAreaField(
        "Description",
        validators=[Optional()]
    )
    tags = StringField(
        "Tags (comma-separated)",
        validators=[Optional(), Length(max=300)]
    )
    submit = SubmitField("Save")