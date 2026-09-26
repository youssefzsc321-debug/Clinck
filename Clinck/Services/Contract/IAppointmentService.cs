using Clinck.Domain.Enities;

namespace Clinck.Web.Services.Contract
{
    public interface IAppointmentService
    {
        Task Add(Appointment dept);

        Task<int> SaveChanges();
        Task<Appointment?> GetAppointmentWithDoctor(DateTime appDate,int doctorId);
        Task<Appointment?> GetAppById(int id);
    }
}
