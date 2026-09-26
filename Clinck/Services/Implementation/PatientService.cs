using Clinck.Application.Common.Interfaces;
using Clinck.Domain.Enities;
using Clinck.Web.Services.Contract;
using Microsoft.EntityFrameworkCore;

namespace Clinck.Web.Services.Implementation
{
    public class PatientService : IPatientService
    {
        private readonly IApplicationDbContext _context;
        public PatientService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(Patient patient)
        {
            await _context.Patients.AddAsync(patient);
        }

        public async Task<Patient?> GetByEmail(string Email)
        {
            return await _context.Patients.FirstOrDefaultAsync(x=>x.Email == Email);
        }

        public async Task<Patient?> GetById(int id)
        {
            return await _context.Patients.FindAsync(id);
        }

        public async Task<Patient?> GetByIdWithAppointment(int id)
        {
            return await _context.Patients
                .Include(p => p.Appointments)
                .ThenInclude(p=>p.Doctor)
                .Include(p=>p.Appointments)
                .ThenInclude(p=>p.Department)
                .SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Patient?> GetByPhone(string phoneNumber)
        {
            return await _context.Patients.FirstOrDefaultAsync(x=>x.Phone==phoneNumber);
        }

        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
