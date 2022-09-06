using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EMarket.Core.Application.Interfaces.Services;
using WebApp.EMarket.Middlewares;
using EMarket.Core.Application.ViewModels.Home;
using System.Collections.Generic;

namespace EMarket.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAdvertisementService _advertisementService;
        private readonly ICategoryService _categoryService;
        private readonly HomeViewModel _homeViewModel;
        private readonly ValidateUserSession _validateUserSession;

        public HomeController(IAdvertisementService advertisementService, ICategoryService categoryService, ValidateUserSession validateUserSession)
        {
            _advertisementService = advertisementService;
            _categoryService = categoryService;
            _validateUserSession = validateUserSession;
            _homeViewModel = new HomeViewModel();
        }

        public async Task<IActionResult> Index(List<int> categoryIds)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            if (categoryIds.Count != 0)
            {
                _homeViewModel.Advertisements = await _advertisementService.Filter(categoryIds);
            }
            else
            {
                _homeViewModel.Advertisements = await _advertisementService.GetAllViewModelFromOtherUsers();
            }

            _homeViewModel.Categories = await _categoryService.GetAllViewModel();
            return View(_homeViewModel);
        }

        public async Task<IActionResult> Detail(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            return View(await _advertisementService.GetByIdViewModel(id));
        }

        [HttpPost]
        public async Task<IActionResult> SearchByName(string ArticleName)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            _homeViewModel.Advertisements = await _advertisementService.Search(ArticleName);
            _homeViewModel.Categories = await _categoryService.GetAllViewModel();
            return View("Index", _homeViewModel);
        }
    }
}