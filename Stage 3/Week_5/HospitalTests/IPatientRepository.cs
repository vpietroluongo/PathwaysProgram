public interface IPatientRepository
{
    Task<Patient?> GetPatientAsync(int patientId);
    Task<List<Patient>> GetAdmittedPatientsAsync();
    Task AddPatientAsync(Patient patient);
    Task UpdatePatientAsync(Patient patient);
    Task<bool> DeletePatientAsync(int patientId);
}