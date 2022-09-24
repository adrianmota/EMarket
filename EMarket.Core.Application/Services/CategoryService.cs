using EMarket.Core.Application.Interfaces.Repositories;
using EMarket.Core.Application.Interfaces.Services;
using EMarket.Core.Application.ViewModels.Categories;
using EMarket.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMarket.Core.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<SaveCategoryViewModel> Add(SaveCategoryViewModel saveViewModel)
        {
            Category category = new();
            category.Name = saveViewModel.Name;
            category.Description = saveViewModel.Description;

            category = await _categoryRepository.AddAsync(category);

            SaveCategoryViewModel saveCategoryViewModel = new();
            saveCategoryViewModel.Id = category.Id;
            saveCategoryViewModel.Name = category.Name;
            saveCategoryViewModel.Description = category.Description;

            return saveCategoryViewModel;
        }

        public async Task Update(SaveCategoryViewModel saveViewModel)
        {
            Category category = await _categoryRepository.GetByIdAsync(saveViewModel.Id);
            category.Name = saveViewModel.Name;
            category.Description = saveViewModel.Description;

            await _categoryRepository.UpdateAsync(category);
        }

        public async Task Delete(SaveCategoryViewModel saveViewModel)
        {
            await _categoryRepository.DeleteAsync(saveViewModel.Id);
        }

        public async Task<List<CategoryViewModel>> GetAllViewModel()
        {
            List<Category> categories = await _categoryRepository.GetAllWithIncludeAsync(new List<string> { "Advertisements" });
            int counter = 0;
            List<CategoryViewModel> categoryViewModelList = categories.Select(category => new CategoryViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                AdvertisementsQuantity = category.Advertisements.Count(),
            }).ToList();

            foreach(Category category in categories)
            {
                var userIds = new HashSet<int>();

                foreach (var advertisement in category.Advertisements)
                {
                    userIds.Add(advertisement.UserId);
                }

                categoryViewModelList[counter].OwnersOfAdsQuantity = userIds.Count;
                counter++;
            }

            return categoryViewModelList;
        }

        public async Task<SaveCategoryViewModel> GetByIdSaveViewModel(int id)
        {
            Category category = await _categoryRepository.GetByIdAsync(id);
            SaveCategoryViewModel saveViewModel = new();
            saveViewModel.Id = category.Id;
            saveViewModel.Name = category.Name;
            saveViewModel.Description = category.Description;

            return saveViewModel;
        }

        public Task<CategoryViewModel> GetByIdViewModel(int id)
        {
            throw new NotImplementedException();
        }
    }
}