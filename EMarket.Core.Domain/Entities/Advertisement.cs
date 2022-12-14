using EMarket.Core.Domain.Common;

namespace EMarket.Core.Domain.Entities
{
    public class Advertisement : AuditableBaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl1 { get; set; }
        public string ImageUrl2 { get; set; }
        public string ImageUrl3 { get; set; }
        public string ImageUrl4 { get; set; }
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }

        // Navigation properties
        public Category Category { get; set; }
        public User User { get; set; }
    }
}