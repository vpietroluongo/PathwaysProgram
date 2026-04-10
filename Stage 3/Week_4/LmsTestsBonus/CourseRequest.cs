public record CourseRequest(int StudentId, int CourseId);

public record EnrollStudentRequest(int StudentId, int CourseId, string StudentEmail, string CourseTitle)
    : CourseRequest(StudentId, CourseId);

public record CheckAvailabilityRequest(int CourseId)
    : CourseRequest(0, CourseId);
