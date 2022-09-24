using EMarket.Core.Application.Helpers;
using EMarket.Core.Application.Interfaces.Repositories;
using EMarket.Core.Application.Interfaces.Services;
using EMarket.Core.Application.ViewModels.Advertisements;
using EMarket.Core.Application.ViewModels.Users;
using EMarket.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMarket.Core.Application.Services
{
    public class AdvertisementService : IAdvertisementService
    {
        private readonly IAdvertisementRepository _advertisementRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserViewModel _userViewModel;

        public AdvertisementService(IAdvertisementRepository advertisementRepository, IHttpContextAccessor httpContextAccessor)
        {
            _advertisementRepository = advertisementRepository;
            _httpContextAccessor = httpContextAccessor;
            _userViewModel = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
        }

        public async Task<SaveAdvertisementViewModel> Add(SaveAdvertisementViewModel saveViewModel)
        {
            Advertisement advertisement = new();
            advertisement.Name = saveViewModel.Name;
            advertisement.Description = saveViewModel.Description;
            advertisement.ImageUrl1 = saveViewModel.ImageUrl1;
            advertisement.ImageUrl2 = saveViewModel.ImageUrl2;
            advertisement.ImageUrl3 = saveViewModel.ImageUrl3;
            advertisement.ImageUrl4 = saveViewModel.ImageUrl4;
            advertisement.Price = saveViewModel.Price;
            advertisement.CategoryId = saveViewModel.CategoryId;
            advertisement.UserId = _userViewModel.Id;

            advertisement = await _advertisementRepository.AddAsync(advertisement);

            SaveAdvertisementViewModel saveAdvertisementViewModel = new();
            saveAdvertisementViewModel.Id = advertisement.Id;
            saveAdvertisementViewModel.Name = advertisement.Name;
            saveAdvertisementViewModel.Description = advertisement.Description;
            saveAdvertisementViewModel.ImageUrl1 = advertisement.ImageUrl1;
            saveAdvertisementViewModel.ImageUrl2 = advertisement.ImageUrl2;
            saveAdvertisementViewModel.ImageUrl3 = advertisement.ImageUrl3;
            saveAdvertisementViewModel.ImageUrl4 = advertisement.ImageUrl4;
            saveAdvertisementViewModel.CategoryId = saveViewModel.CategoryId;
            saveAdvertisementViewModel.Price = advertisement.Price;

            return saveAdvertisementViewModel;
        }

        public async Task Update(SaveAdvertisementViewModel saveViewModel)
        {
            Advertisement advertisement = await _advertisementRepository.GetByIdAsync(saveViewModel.Id);
            advertisement.Id = saveViewModel.Id;
            advertisement.Name = saveViewModel.Name;
            advertisement.Description = saveViewModel.Description;
            advertisement.ImageUrl1 = saveViewModel.ImageUrl1;
            advertisement.ImageUrl2 = saveViewModel.ImageUrl2;
            advertisement.ImageUrl3 = saveViewModel.ImageUrl3;
            advertisement.ImageUrl4 = saveViewModel.ImageUrl4;
            advertisement.CategoryId = saveViewModel.CategoryId;
            advertisement.Price = saveViewModel.Price;

            await _advertisementRepository.UpdateAsync(advertisement);
        }

        public async Task Delete(SaveAdvertisementViewModel viewModel)
        {
            await _advertisementRepository.DeleteAsync(viewModel.Id);
        }

        public async Task<List<AdvertisementViewModel>> GetAllViewModel()
        {
            List<Advertisement> advertisements = await _advertisementRepository.GetAllWithIncludeAsync(new List<string> { "Category" });

            return advertisements.Where(advertisement => advertisement.UserId == _userViewModel.Id)
            .Select(advertisement => new AdvertisementViewModel
            {
                Id = advertisement.Id,
                Name = advertisement.Name,
                Description = advertisement.Description,
                ImageUrl1 = advertisement.ImageUrl1,
                ImageUrl2 = advertisement.ImageUrl2,
                ImageUrl3 = advertisement.ImageUrl3,
                ImageUrl4 = advertisement.ImageUrl4,
                Price = advertisement.Price,
                CategoryName = advertisement.Category.Name
            }).ToList();
        }

        public async Task<List<AdvertisementViewModel>> GetAllViewModelFromOtherUsers()
        {
            List<Advertisement> advertisements = await _advertisementRepository.GetAllWithIncludeAsync(new List<string> { "Category", "User" });

            return advertisements.Where(advertisement => advertisement.UserId != _userViewModel.Id)
            .Select(advertisement => new AdvertisementViewModel
            {
                Id = advertisement.Id,
                Name = advertisement.Name,
                Description = advertisement.Description,
                ImageUrl1 = advertisement.ImageUrl1,
                ImageUrl2 = advertisement.ImageUrl2,
                ImageUrl3 = advertisement.ImageUrl3,
                ImageUrl4 = advertisement.ImageUrl4,
                Price = advertisement.Price,
                CategoryId = advertisement.CategoryId,
                CategoryName = advertisement.Category.Name
            }).ToList();
        }

        public async Task<AdvertisementViewModel> GetByIdViewModel(int id)
        {
            List<Advertisement> advertisements = await _advertisementRepository.GetAllWithIncludeAsync(new List<string> { "Category", "User" });
            Advertisement advertisement = advertisements.FirstOrDefault(advertisement => advertisement.Id == id);

            AdvertisementViewModel advertisementViewModel = new();
            advertisementViewModel.Id = advertisement.Id;
            advertisementViewModel.Name = advertisement.Name;
            advertisementViewModel.Description = advertisement.Description;
            advertisementViewModel.ImageUrl1 = advertisement.ImageUrl1;
            advertisementViewModel.ImageUrl2 = advertisement.ImageUrl2;
            advertisementViewModel.ImageUrl3 = advertisement.ImageUrl3;
            advertisementViewModel.ImageUrl4 = advertisement.ImageUrl4;
            advertisementViewModel.CategoryId = advertisement.CategoryId;
            advertisementViewModel.CategoryName = advertisement.Category.Name;
            advertisementViewModel.Price = advertisement.Price;
            advertisementViewModel.AdvertiserName = advertisement.User.FirstName + " " + advertisement.User.LastName;
            advertisementViewModel.AdvertiserEmail = advertisement.User.Email;
            advertisementViewModel.AdvertiserPhone = advertisement.User.Phone;
            advertisementViewModel.DatePublished = advertisement.Created.ToShortDateString();

            return advertisementViewModel;
        }

        public async Task<SaveAdvertisementViewModel> GetByIdSaveViewModel(int id)
        {
            Advertisement advertisement = await _advertisementRepository.GetByIdAsync(id);

            SaveAdvertisementViewModel saveAdvertisementViewModel = new();
            saveAdvertisementViewModel.Id = advertisement.Id;
            saveAdvertisementViewModel.Name = advertisement.Name;
            saveAdvertisementViewModel.Description = advertisement.Description;
            saveAdvertisementViewModel.ImageUrl1 = advertisement.ImageUrl1;
            saveAdvertisementViewModel.ImageUrl2 = advertisement.ImageUrl2;
            saveAdvertisementViewModel.ImageUrl3 = advertisement.ImageUrl3;
            saveAdvertisementViewModel.ImageUrl4 = advertisement.ImageUrl4;
            saveAdvertisementViewModel.CategoryId = advertisement.CategoryId;
            saveAdvertisementViewModel.Price = advertisement.Price;

            return saveAdvertisementViewModel;
        }

        public async Task<List<AdvertisementViewModel>> Filter(List<int> categoryIds)
        {
            List<AdvertisementViewModel> advertisementViewModelUnfiltered = await GetAllViewModelFromOtherUsers();
            List<AdvertisementViewModel> advertisementViewModelFiltered = new();

            foreach (int categoryId in categoryIds)
            {
                List<AdvertisementViewModel> advertisementViewModelList = advertisementViewModelUnfiltered.Where(viewModel => viewModel.CategoryId == categoryId).ToList();

                foreach (var advertisementViewModel in advertisementViewModelList)
                {
                    advertisementViewModelFiltered.Add(advertisementViewModel);
                }
            }

            return advertisementViewModelFiltered;
        }

        public async Task<List<AdvertisementViewModel>> Search(string ArticleName)
        {
            List<AdvertisementViewModel> advertisementViewModelList = await GetAllViewModelFromOtherUsers();

            if (ArticleName == null)
            {
                return advertisementViewModelList;
            }

            return advertisementViewModelList.Where(viewModel => viewModel.Name == ArticleName).ToList();
        }
    }
}