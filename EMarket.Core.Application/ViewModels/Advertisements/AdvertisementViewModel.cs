namespace EMarket.Core.Application.ViewModels.Advertisements
{
    public class AdvertisementViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl1 { get; set; }
        public string ImageUrl2 { get; set; }
        public string ImageUrl3 { get; set; }
        public string ImageUrl4 { get; set; }
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string AdvertiserName { get; set; }
        public string AdvertiserEmail { get; set; }
        public string AdvertiserPhone { get; set; }
        public string DatePublished { get; set; }
    }
}
