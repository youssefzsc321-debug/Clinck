using Clinck.Application.Common.Interfaces;
using Clinck.Domain.Enities;
using Clinck.Web.Services.Contract;
using Microsoft.EntityFrameworkCore;

namespace Clinck.Web.Services.Implementation
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IApplicationDbContext _contxet;
        public AppointmentService(IApplicationDbContext contxet)
        {
            _contxet = contxet;
        }
        public async Task Add(Domain.Enities.Appointment App)
        {
            await _contxet.Appointments.AddAsync(App);
        }

        public async Task<Appointment?> GetAppById(int id)
        {
            return await _contxet.Appointments.FindAsync(id);
        }

        public Task<Appointment?> GetAppointmentWithDoctor(DateTime appDate, int doctorId)
        {
            return _contxet.Appointments.SingleOrDefaultAsync(a => a.AppointmentDate == appDate && a.DoctorId == doctorId);

        }

        public async Task<int> SaveChanges()
        {
            return await _contxet.SaveChangesAsync();
        }
    }
}
