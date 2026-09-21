using Clinck.Domain.Enities;

namespace Clinck.Web.Services.Contract
{
    public interface IDocotorService
    {
        Task<IEnumerable<Doctor>> GetAllAsync();
        Task<Doctor?> GetByIdAsync(int id);
        Task<Doctor?> GetByIdWithDeptAsync(int id);

        Task<Doctor?> GetByNameAsync(string Name);
        Task Add(Doctor doctor);
        Task<int> SaveChanges();

    }
}
