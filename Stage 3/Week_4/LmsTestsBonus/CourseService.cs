public class CourseService
{
    private readonly IEnrollmentRepository _repository;
    private readonly INotificationService _notificationService;

    public CourseService(IEnrollmentRepository repository, INotificationService notificationService)
    {
        _repository = repository;
        _notificationService = notificationService;
    }

    public async Task<bool> ProcessRequestAsync(CourseRequest request)
    {
        return request switch
        {
            EnrollStudentRequest enroll => await HandleEnrollmentAsync(enroll),
            CheckAvailabilityRequest availability => await HandleAvailabilityCheckAsync(availability),
            _ => throw new ArgumentException("Unsupported request type")
        };
    }

    private async Task<bool> HandleEnrollmentAsync(EnrollStudentRequest request)
    {
        // Check if already enrolled
        var existing = await _repository.GetEnrollmentAsync(request.StudentId, request.CourseId);
        if (existing != null)
            return false;

        // Check if course is full
        bool isFull = await _repository.IsCourseFullAsync(request.CourseId);
        if (isFull)
        {
            await _notificationService.SendCourseFullNotificationAsync(request.CourseTitle);
            return false;
        }

        var enrollment = new Enrollment(
            Id: 0,
            StudentId: request.StudentId,
            CourseId: request.CourseId,
            EnrolledDate: DateTime.UtcNow,
            IsCompleted: false
        );

        await _repository.AddEnrollmentAsync(enrollment);
        await _notificationService.SendEnrollmentConfirmationAsync(request.StudentEmail, request.CourseTitle);

        return true;
    }

    private async Task<bool> HandleAvailabilityCheckAsync(CheckAvailabilityRequest request)
    {
        int enrolled = await _repository.GetEnrollmentCountAsync(request.CourseId);
        return enrolled < 30;   // Max 30 students
    }
}