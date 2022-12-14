using EMarket.Core.Application.ViewModels.Advertisements;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EMarket.Core.Application.Interfaces.Services
{
    public interface IAdvertisementService : IGenericService<SaveAdvertisementViewModel, AdvertisementViewModel>
    {
        Task<List<AdvertisementViewModel>> GetAllViewModelFromOtherUsers();
        Task<List<AdvertisementViewModel>> Search(string ArticleName);
        Task<List<AdvertisementViewModel>> Filter(List<int> categoryIds);
    }
}