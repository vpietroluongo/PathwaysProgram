from bookmarks import create_app

app=create_app()

@app.route("/debug/routes")
def debug_routes():
    routes = []
    for rule in app.url_map.iter_rules():
        routes.append(str(rule))
    return "<br>".join(sorted(routes))

if __name__ == "__main__":
    app.run(debug=True)