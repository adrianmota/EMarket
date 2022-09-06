using EMarket.Core.Application.Interfaces.Services;
using EMarket.Core.Application.ViewModels.Advertisements;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WebApp.EMarket.Middlewares;

namespace WebApp.EMarket.Controllers
{
    public class AdvertisementController : Controller
    {
        private readonly IAdvertisementService _advertisementService;
        private readonly ICategoryService _categoryService;
        private readonly ValidateUserSession _validateUserSession;

        public AdvertisementController(IAdvertisementService advertisementService, ICategoryService categoryService, ValidateUserSession validateUserSession)
        {
            _advertisementService = advertisementService;
            _categoryService = categoryService;
            _validateUserSession = validateUserSession;
        }

        public async Task<IActionResult> Index()
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            return View(await _advertisementService.GetAllViewModel());
        }

        public async Task<IActionResult> Create()
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            SaveAdvertisementViewModel saveViewModel = new SaveAdvertisementViewModel();
            saveViewModel.Categories = await _categoryService.GetAllViewModel();

            return View("SaveAdvertisement", saveViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveAdvertisementViewModel saveViewModel)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            if (!ModelState.IsValid)
            {
                saveViewModel.Categories = await _categoryService.GetAllViewModel();
                return View("SaveAdvertisement", saveViewModel);
            }

            List<IFormFile> files = new List<IFormFile> { saveViewModel.ImageFile1, saveViewModel.ImageFile2, saveViewModel.ImageFile3, saveViewModel.ImageFile4 };
            SaveAdvertisementViewModel saveAdvertisementViewModel = await _advertisementService.Add(saveViewModel);

            if (saveAdvertisementViewModel != null && saveAdvertisementViewModel.Id != 0)
            {
                List<string> imagesPath = UploadImage(files, saveAdvertisementViewModel.Id);
                saveAdvertisementViewModel.ImageUrl1 = imagesPath[0];
                saveAdvertisementViewModel.ImageUrl2 = imagesPath[1];
                saveAdvertisementViewModel.ImageUrl3 = imagesPath[2];
                saveAdvertisementViewModel.ImageUrl4 = imagesPath[3];

                await _advertisementService.Update(saveAdvertisementViewModel);
            }

            return RedirectToRoute(new { controller = "Advertisement", action = "Index" });
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            SaveAdvertisementViewModel saveViewModel = await _advertisementService.GetByIdSaveViewModel(id);
            saveViewModel.Categories = await _categoryService.GetAllViewModel();

            return View("SaveAdvertisement", saveViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveAdvertisementViewModel saveViewModel)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            List<IFormFile> files = new List<IFormFile> { saveViewModel.ImageFile1, saveViewModel.ImageFile2, saveViewModel.ImageFile3, saveViewModel.ImageFile4 };
            SaveAdvertisementViewModel oldSaveViewModel = await _advertisementService.GetByIdSaveViewModel(saveViewModel.Id);

            if (saveViewModel != null && saveViewModel.Id != 0)
            {
                List<string> oldImagesPath = new List<string> { oldSaveViewModel.ImageUrl1, oldSaveViewModel.ImageUrl2, oldSaveViewModel.ImageUrl3, oldSaveViewModel.ImageUrl4 };

                List<string> imagesPath = UploadImage(files, oldSaveViewModel.Id, true, oldImagesPath);
                saveViewModel.ImageUrl1 = imagesPath[0] == null ? oldImagesPath[0] : imagesPath[0];
                saveViewModel.ImageUrl2 = imagesPath[1] == null ? oldImagesPath[1] : imagesPath[1];
                saveViewModel.ImageUrl3 = imagesPath[2] == null ? oldImagesPath[2] : imagesPath[2];
                saveViewModel.ImageUrl4 = imagesPath[3] == null ? oldImagesPath[3] : imagesPath[3];

                await _advertisementService.Update(saveViewModel);
            }

            return RedirectToRoute(new { controller = "Advertisement", action = "Index" });
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            return View(await _advertisementService.GetByIdSaveViewModel(id));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SaveAdvertisementViewModel viewModel)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            string basePath = $"/images/Advertisements/{viewModel.Id}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/{basePath}");

            if (Directory.Exists(path))
            {
                DirectoryInfo directoryInfo = new(path);

                foreach(FileInfo file in directoryInfo.GetFiles())
                {
                    file.Delete();
                }

                foreach(DirectoryInfo directory in directoryInfo.GetDirectories())
                {
                    directory.Delete();
                }

                Directory.Delete(path);
            }

            await _advertisementService.Delete(viewModel);
            return RedirectToRoute(new { controller = "Advertisement", action = "Index" });
        }

        private List<string> UploadImage(List<IFormFile> files, int id, bool isEditMode = false, List<string> oldImagesPath = null)
        {
            if (isEditMode && files == null)
            {
                return oldImagesPath;
            }

            List<string> relativeImagesPath = new();

            string basePath = $"/images/Advertisements/{id}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/{basePath}");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (isEditMode)
            {
                int counter = 0;

                foreach (IFormFile file in files)
                {
                    string relativeFilename = null;

                    if (file != null)
                    {
                        Guid guid = Guid.NewGuid();
                        FileInfo fileInfo = new(file.FileName);
                        string filename = guid + fileInfo.Extension;

                        string fullPathName = Path.Combine(path, filename);

                        using (var stream = new FileStream(fullPathName, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }

                        relativeFilename = $"{basePath}/{filename}";
                        relativeImagesPath.Add(relativeFilename);

                        string imageUrl = oldImagesPath[counter];

                        if (imageUrl != null)
                        {
                            System.IO.File.Delete($"{Directory.GetCurrentDirectory()}/wwwroot{imageUrl}");
                        }
                    }
                    else
                    {
                        relativeImagesPath.Add(relativeFilename);
                    }

                    counter++;
                }
            }
            else
            {
                foreach (IFormFile file in files)
                {
                    string relativeFilename = null;

                    if (file != null)
                    {
                        Guid guid = Guid.NewGuid();
                        FileInfo fileInfo = new(file.FileName);
                        string filename = guid + fileInfo.Extension;

                        string fullPathName = Path.Combine(path, filename);

                        using (var stream = new FileStream(fullPathName, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }

                        relativeFilename = $"{basePath}/{filename}";
                        relativeImagesPath.Add(relativeFilename);
                    }
                    else
                    {
                        relativeImagesPath.Add(relativeFilename);
                    }
                }
            }

            return relativeImagesPath;
        }
    }
}
