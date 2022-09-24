using EMarket.Core.Application.ViewModels.Users;
using EMarket.Core.Domain.Entities;
using System.Threading.Tasks;

namespace EMarket.Core.Application.Interfaces.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> LoginAsync(LoginViewModel login);
    }
}