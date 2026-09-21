using Clinck.Application.Common.Interfaces;
using Clinck.Domain.Enities;
using Clinck.Web.Services.Contract;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Clinck.Web.Services.Implementation
{
    public class DocotorService : IDocotorService
    {
        private readonly IApplicationDbContext _context;
        

        public DocotorService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(Doctor doctor)
        {
           await _context.Doctors.AddAsync(doctor);
            
        }

        public async Task<IEnumerable<Doctor>> GetAllAsync()
        {
            var doctors =await _context.Doctors.Include(d=>d.Department).ToListAsync();
            return doctors;
        }

        public async Task<Doctor?> GetByIdAsync(int id)
        {
            var doctor =await _context.Doctors.FindAsync(id);
            return doctor;
        }

        public async Task<Doctor?> GetByIdWithDeptAsync(int id)
        {
            var doc =await _context.Doctors.Include(d => d.Department).FirstOrDefaultAsync(d => d.Id == id);
            return doc;
        }

        public async Task<Doctor?> GetByNameAsync(string Name)
        {
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => $"{d.FirstName} {d.LastName}" == Name);
            return doctor;
        }

        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }

        
    }
}
