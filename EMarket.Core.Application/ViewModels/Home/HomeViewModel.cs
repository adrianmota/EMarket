using System.Collections.Generic;
using EMarket.Core.Application.ViewModels.Advertisements;
using EMarket.Core.Application.ViewModels.Categories;

namespace EMarket.Core.Application.ViewModels.Home
{
    public class HomeViewModel
    {
        public List<AdvertisementViewModel> Advertisements { get; set; }
        public List<CategoryViewModel> Categories { get; set; }
        public string ArticleName { get; set; }
        public List<int> CategoryIds { get; set; }
    }
}
