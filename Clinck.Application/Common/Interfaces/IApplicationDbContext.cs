using Clinck.Domain.Enities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinck.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Department> Departments { get; }
        DbSet<Doctor> Doctors { get; }


        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
