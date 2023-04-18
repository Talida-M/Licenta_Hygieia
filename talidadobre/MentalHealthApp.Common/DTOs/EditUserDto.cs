using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.Common.DTOs
{
    public class EditUserDto
    {
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public string Email { get; set; } = null!;
       // public string PhoneNumberCountryPrefix { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string SocialCategory { get; set; } = null!;
        public string? HasInsurance { get; set; }
        public string Country { get; set; } = null!;
        public string County { get; set; } = null!;
        public string City { get; set; } = null!;
        public string? StreetNumber { get; set; }
        public string? ApartmentBuildingFloor { get; set; }
        public string? Sector { get; set; }
        public string? ZipCode { get; set; }
        public byte[] UserImage { get; set; } = null!;
    }
}
