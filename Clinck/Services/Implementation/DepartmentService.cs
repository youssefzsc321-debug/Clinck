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
        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            var depts =await _context.Departments.ToListAsync();
            return depts;
        }
    }
}
