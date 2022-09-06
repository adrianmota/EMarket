using EMarket.Core.Application.Helpers;
using EMarket.Core.Application.Interfaces.Services;
using EMarket.Core.Application.ViewModels.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApp.EMarket.Middlewares;

namespace WebApp.EMarket.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccesor;
        private readonly ValidateUserSession _validateUserSession;

        public UserController(IUserService userService, IHttpContextAccessor httpContextAccesor, ValidateUserSession validateUserSession)
        {
            _userService = userService;
            _httpContextAccesor = httpContextAccesor;
            _validateUserSession = validateUserSession;
        }

        public IActionResult Index()
        {
            if (_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel login)
        {
            if (_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            if (!ModelState.IsValid)
            {
                return View(login);
            }

            UserViewModel userViewModel = await _userService.Login(login);

            if (userViewModel == null)
            {
                ModelState.AddModelError("userValidation", "Las credenciales son incorrectas");
                return View();
            }

            _httpContextAccesor.HttpContext.Session.Set<UserViewModel>("user", userViewModel);
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }

        public IActionResult LogOut()
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            _httpContextAccesor.HttpContext.Session.Remove("user");
            return RedirectToRoute(new { controller = "User", action = "Index" });
        }

        public IActionResult Register()
        {
            if (_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            return View(new SaveUserViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(SaveUserViewModel saveViewModel)
        {
            if (_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            if (!ModelState.IsValid)
            {
                return View(saveViewModel);
            }

            SaveUserViewModel userViewModel = await _userService.Add(saveViewModel);

            if (userViewModel == null)
            {
                ModelState.AddModelError("userValidation", "Ya existe un usuario con este nombre de usuario");
                return View(saveViewModel);
            }

            return RedirectToRoute(new { controller = "User", action = "Index" });
        }
    }
}
