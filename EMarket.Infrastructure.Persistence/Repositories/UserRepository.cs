using EMarket.Core.Application.Helpers;
using EMarket.Core.Application.Interfaces.Repositories;
using EMarket.Core.Application.ViewModels.Users;
using EMarket.Core.Domain.Entities;
using EMarket.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EMarket.Infrastructure.Persistence.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly ApplicationContext _dbContext;

        public UserRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<User> AddAsync(User user)
        {
            user.Password = PasswordEncryption.ComputeSHA256Hash(user.Password);
            return await base.AddAsync(user);
        }

        public async Task<User> LoginAsync(LoginViewModel login)
        {
            string passwordEncrypted = PasswordEncryption.ComputeSHA256Hash(login.Password);
            User user = await _dbContext.Set<User>()
                        .FirstOrDefaultAsync(user => user.Username == login.Username && user.Password == passwordEncrypted);
        
            if (user == null)
            {
                return null;
            }

            return user;
        }
    }
}
