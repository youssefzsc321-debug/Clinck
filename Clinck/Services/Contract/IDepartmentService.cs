using Clinck.Domain.Enities;

namespace Clinck.Web.Services.Contract
{
    public interface IDepartmentService
    {
        Task<IEnumerable<Department>> GetAllAsync();
    }
}
