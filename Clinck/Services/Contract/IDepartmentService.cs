using Clinck.Domain.Enities;

namespace Clinck.Web.Services.Contract
{
    public interface IDepartmentService
    {
        Task<IEnumerable<Department>> GetAllAsync();
        Task<IEnumerable<Department>> GetAllWithDoctorsAsync();
        Task Add(Department department);
        Task<int> SaveChanges();
        Task<Department?>GetDeptWithDoctorsById(int id);
        Task<Department?> GetDeptById(int id);
        Task<Department?> GetByName(string name);


    }
}
