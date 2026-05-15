from flask_wtf import FlaskForm
from wtforms import PasswordField, StringField, SubmitField
from wtforms.validators import Email, EqualTo, DataRequired, Length, URL

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

class LoginForm(FlaskForm):
    email = StringField("Email", validators=[DataRequired(), Email()])
    password = PasswordField("Password", validators=[DataRequired()])
    submit = SubmitField("Log in")

class RegisterForm(FlaskForm):
    email = StringField("Email", validators=[DataRequired(), Email()])
    password = PasswordField(
        "Password",
        validators=[DataRequired(), Length(min=8)]
    )
    confirm = PasswordField(
        "Confirm password",
        validators=[DataRequired(), EqualTo("password")]
    )
    submit = SubmitField("Register")