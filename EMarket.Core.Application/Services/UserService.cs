using EMarket.Core.Application.Interfaces.Repositories;
using EMarket.Core.Application.Interfaces.Services;
using EMarket.Core.Application.ViewModels.Users;
using EMarket.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMarket.Core.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserViewModel> Login(LoginViewModel login)
        {
            User user = await _userRepository.LoginAsync(login);

            if (user == null)
            {
                return null;
            }

            UserViewModel userViewModel = new();
            userViewModel.Id = user.Id;
            userViewModel.FirstName = user.FirstName;
            userViewModel.LastName = user.LastName;
            userViewModel.Email = user.Email;
            userViewModel.Password = user.Password;
            userViewModel.Phone = user.Phone;

            return userViewModel;
        }

        public async Task<SaveUserViewModel> Add(SaveUserViewModel saveViewModel)
        {
            List<User> users = await _userRepository.GetAllAsync();
            User confirmUser = users.FirstOrDefault(user => user.Username == saveViewModel.Username);

            if (confirmUser != null)
            {
                return null;
            }

            User user = new();
            user.FirstName = saveViewModel.FirstName;
            user.LastName = saveViewModel.LastName;
            user.Email = saveViewModel.Email;
            user.Phone = saveViewModel.Phone;
            user.Username = saveViewModel.Username;
            user.Password = saveViewModel.Password;

            user = await _userRepository.AddAsync(user);

            SaveUserViewModel saveUserViewModel = new();
            saveUserViewModel.Id = user.Id;
            saveUserViewModel.FirstName = user.FirstName;
            saveUserViewModel.LastName = user.LastName;
            saveUserViewModel.Email = user.Email;
            saveUserViewModel.Password = user.Password;
            saveUserViewModel.Phone = user.Phone;

            return saveUserViewModel;
        }

        public async Task Update(SaveUserViewModel saveViewModel)
        {
            User user = new();
            user.Id = saveViewModel.Id;
            user.FirstName = saveViewModel.FirstName;
            user.LastName = saveViewModel.LastName;
            user.Email = saveViewModel.Email;
            user.Phone = saveViewModel.Phone;
            user.Username = saveViewModel.Username;
            user.Password = saveViewModel.Password;

            await _userRepository.UpdateAsync(user);
        }

        public async Task Delete(SaveUserViewModel viewModel)
        {
            await _userRepository.DeleteAsync(viewModel.Id);
        }

        public async Task<List<UserViewModel>> GetAllViewModel()
        {
            List<User> users = await _userRepository.GetAllAsync();

            return users.Select(user => new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                Username = user.Username,
                Password = user.Password,
            }).ToList();
        }

        public Task<SaveUserViewModel> GetByIdSaveViewModel(int id)
        {
            throw new NotImplementedException();
        }

        public Task<UserViewModel> GetByIdViewModel(int id)
        {
            throw new NotImplementedException();
        }
    }
}
