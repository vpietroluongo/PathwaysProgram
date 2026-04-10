public interface IEnrollmentRepository
{
    Task<Enrollment?> GetEnrollmentAsync(int studentId, int courseId);
    Task AddEnrollmentAsync(Enrollment enrollment);
    Task<bool> IsCourseFullAsync(int courseId);
    Task<int> GetEnrollmentCountAsync(int courseId);
}