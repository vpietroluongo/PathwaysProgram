public class PatientService
{
    private readonly IPatientRepository _repository;
    private readonly INotificationService _notificationService;

    public PatientService(IPatientRepository repository, INotificationService notificationService)
    {
        _repository = repository;
        _notificationService = notificationService;
    }

    public async Task<int> AdmitPatientAsync(string fullName, string email, string phone, DateTime dateOfBirth)
    {
        var patient = new Patient
        {
            FullName = fullName,
            Email = email,
            Phone = phone,
            DateOfBirth = dateOfBirth,
            IsAdmitted = true
        };

        await _repository.AddPatientAsync(patient);
        await _notificationService.SendAdmissionNotificationAsync(email, fullName);

        return patient.Id;
    }

    public async Task DischargePatientAsync(int patientId)
    {
        var patient = await _repository.GetPatientAsync(patientId);
        if (patient == null)
            throw new InvalidOperationException("Patient not found");

        if (!patient.IsAdmitted)
            throw new InvalidOperationException("Patient is not currently admitted");

        patient.IsAdmitted = false;
        await _repository.UpdatePatientAsync(patient);

        await _notificationService.SendDischargeNotificationAsync(patient.Email, patient.FullName);
    }

    public async Task<List<Patient>> GetCurrentlyAdmittedPatientsAsync()
    {
        var patients = await _repository.GetAdmittedPatientsAsync();
        return patients.Where(p => p.IsAdmitted).ToList();
    }

    public async Task SendEmergencyAlertAsync(int patientId, string message)
    {
        var patient = await _repository.GetPatientAsync(patientId);
        if (patient == null)
            throw new InvalidOperationException("Patient not found");

        await _notificationService.SendCriticalAlertAsync(patient.Phone, message);
    }
}