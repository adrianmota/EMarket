using Microsoft.Extensions.DependencyInjection;
using EMarket.Core.Application.Services;
using EMarket.Core.Application.Interfaces.Services;

namespace EMarket.Core.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            #region Services
            services.AddTransient<IAdvertisementService, AdvertisementService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IUserService, UserService>();
            #endregion
        }
    }
}