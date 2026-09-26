using Clinck.Application.Common.Interfaces;
using Clinck.Domain.Enities;
using Clinck.Web.Services.Contract;
using Microsoft.EntityFrameworkCore;

namespace Clinck.Web.Services.Implementation
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IApplicationDbContext _context;
        public DepartmentService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(Department department)
        {
            await _context.Departments.AddAsync(department);
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            var depts =await _context.Departments.ToListAsync();
            return depts;
        }

        public async Task<IEnumerable<Department>> GetAllWithDoctorsAsync()
        {
            return await _context.Departments.Include(d=>d.Doctors).ToListAsync();
        }

        public async Task<Department?> GetByName(string name)
        {
            return await _context.Departments.SingleOrDefaultAsync(d=>d.Name==name);
        }

        public async Task<Department?> GetDeptById(int id)
        {
            return await _context.Departments.FindAsync(id);
        }

        public async Task<Department?> GetDeptWithDoctorsById(int id)
        {
            return await _context.Departments.Include(d=>d.Doctors).FirstOrDefaultAsync(d=>d.Id==id);
        }

        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
