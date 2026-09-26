using Clinck.Domain.Enities;

namespace Clinck.Web.Services.Contract
{
    public interface IPatientService
    {
        Task<Patient?>GetByPhone(string phoneNumber);
        Task<Patient?>GetByEmail(string Email);
        Task<Patient?>GetById(int id);
        Task Add(Patient patient);

        Task<int> SaveChanges();

        Task<Patient?> GetByIdWithAppointment(int id);

    }
}
