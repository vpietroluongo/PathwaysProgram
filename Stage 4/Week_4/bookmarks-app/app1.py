from flask import Flask

app = Flask(__name__)

@app.route("/")
def index():
    return "<h1>Bookmarks</h1><p>Coming soon eventually.</p>"

@app.route("/about")
def about():
    return "<p>A simple app for saving links you want to revisit.</p>"

@app.route("/health")
def health():
    return "<p>Ok</p>"

@app.route("/version")
def version():
    return "<p>0.1.0</p>"