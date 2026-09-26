using Clinck.Application.ViewModels.UserVms;
using Clinck.Domain.Enities;

namespace Clinck.Web.Services.Contract
{
    public interface IUserService
    {
        Task Add(AppUser user, UserForm modelVm);
        Task<int> SaveChanges();
        Task<AppUser?> GetById(string id);
        Task<AppUser?> GetByName(string name);
        Task<IEnumerable<AppUser>> GetAllAsync();

    }
}
