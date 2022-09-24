using EMarket.Core.Domain.Common;
using System.Collections.Generic;

namespace EMarket.Core.Domain.Entities
{
    public class User : AuditableBaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        // Navigation properties
        public ICollection<Advertisement> Advertisements { get; set; }
    }
}