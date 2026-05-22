from __future__ import annotations

from flask_wtf import FlaskForm
from wtforms import SubmitField, TextAreaField
from wtforms.validators import DataRequired, Length


class MessageForm(FlaskForm):
    content = TextAreaField("Message", validators=[DataRequired(), Length(max=2000)])
    submit = SubmitField("Send")
