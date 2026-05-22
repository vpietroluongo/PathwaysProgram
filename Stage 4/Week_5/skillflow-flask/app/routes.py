
from flask import Blueprint, render_template, url_for, redirect, flash
from . import app
from .models import Course

# User visits / 
#   → home() runs
#   → Fetches all courses from database
#   → Sends courses data to home.html template
#   → Template displays courses on the page
@app.route('/')   # Maps the home() function to the root URL. When someone visits the home page, this function runs.
def home():
    courses = Course.query.all() # Queries the database and retrieves all Course records from the database.
    return render_template('home.html', courses=courses)  # Renders the home.html template and passes the courses list to it so the template can display them.


# User visits /course/3
#   → course_detail(3) runs
#   → Fetches course with ID 3 from database (or 404 if not found)
#   → Sends course data to course.html template
#   → Template displays that course's details
@app.route('/course/<int:course_id>')    # <int:course_id> captures the course ID from the URL
def course_detail(course_id):
    course = Course.query.get_or_404(course_id)
    return render_template('course.html', course=course)

@app.route('/enroll/<int:course_id>', methods=['POST'])
def enroll(course_id):
    # Add to user progress (session or database)
    course = Course.query.get_or_404(course_id)
    flash('Successfully enrolled!', 'success')
    return redirect(url_for('course_detail', course_id=course_id))