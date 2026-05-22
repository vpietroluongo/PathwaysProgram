from flask import Flask, request, render_template, redirect, url_for

app = Flask(__name__)

# Pretend database for now
quotes = [
    {"id": 1, "text": "The only way to do great work is to love what you do.", "author": "Steve Jobs"},
    {"id": 2, "text": "In the middle of difficulty lies opportunity.", "author": "Albert Einstein"},
    {"id": 3, "text": "The best way to predict the future is to invent it.", "author": "Alan Kay"}
]

@app.route('/')
def index():
    #return f"<h1>Quote Collector</h1><p>{len(quotes)} quotes loaded.</p>"
    return render_template('index.html', quotes=quotes)

@app.route('/quote/<int:quote_id>')
def show(quote_id):
    quote = next((q for q in quotes if q["id"] == quote_id), None)
    if quote is None:
        return f"No quote with id {quote_id}", 404
    return f"<blockquote>{quote['text']}</blockquote><p>- {quote["author"]}</p>"

@app.route('/search')
def search():
    query = request.args.get('q', '').lower()
    if not query:
        return "<p>Add ?q=something to the URL to search.</p>"
    matches = [
        q for q in quotes
        if query in q["text"].lower() or query in q["author"].lower()
    ]
    if not matches:
        return f"<p>No quotes match '{query}'.</p>"
    items = "".join(
        f"<li><strong>{q['author']}</strong>: {q['text']}</li>"
        for q in matches
    )
    return f"<h2>Results for '{query}'</h2><ul>{items}</ul>"

@app.route('/quote', methods=['POST'])
def create():
    text = request.form['text'].strip()
    author = request.form['author'].strip()
    if not text or not author:
        return "Both fields are required.", 400
    new_id = max((q["id"] for q in quotes), default=0) + 1
    quotes.append({"id": new_id, "text": text, "author": author})
    return redirect(url_for('index'))