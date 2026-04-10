public class CourseService
{
    private readonly IEnrollmentRepository _repository;
    private readonly INotificationService _notificationService;

    public CourseService(IEnrollmentRepository repository, INotificationService notificationService)
    {
        _repository = repository;
        _notificationService = notificationService;
    }

    public async Task<bool> EnrollStudentAsync(int studentId, int courseId, string studentEmail, string courseTitle)
    {
        // Check if already enrolled
        var existing = await _repository.GetEnrollmentAsync(studentId, courseId);
        if (existing != null)
            return false;

        // Check if course is full
        bool isFull = await _repository.IsCourseFullAsync(courseId);
        if (isFull)
        {
            await _notificationService.SendCourseFullNotificationAsync(courseTitle);
            return false;
        }

        // Create and save enrollment
        var enrollment = new Enrollment
        {
            StudentId = studentId,
            CourseId = courseId,
            EnrolledDate = DateTime.UtcNow,
            IsCompleted = false
        };

        await _repository.AddEnrollmentAsync(enrollment);

        // Send confirmation
        await _notificationService.SendEnrollmentConfirmationAsync(studentEmail, courseTitle);

        return true;
    }

    public async Task<int> GetAvailableSpotsAsync(int courseId)
    {
        int enrolled = await _repository.GetEnrollmentCountAsync(courseId);
        // Assume we have a Course with MaxStudents = 30 for simplicity
        return 30 - enrolled;
    }
}