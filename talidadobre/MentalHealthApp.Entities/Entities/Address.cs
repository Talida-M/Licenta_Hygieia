using MentalHealthApp.Common;
using System;
using System.Collections.Generic;

namespace MentalHealthApp.Entities
{
    public partial class Address :IEntity
    {
        public Address()
        {
            IdentityUsers = new HashSet<IdentityUser>();
        }

        public int Id { get; set; }
        public string Country { get; set; } = null!;
        public string County { get; set; } = null!;
        public string City { get; set; } = null!;
        public string? StreetNumber { get; set; }
        public string? ApartmentBuildingFloor { get; set; }
        public string? Sector { get; set; }
        public string? ZipCode { get; set; }

        public virtual ICollection<IdentityUser> IdentityUsers { get; set; }
    }
}
