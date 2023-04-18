using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.BusinessLogic.Implementation.Account.ViewModels
{
    public class RegisterModel
    {
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public string Email { get; set; } = null!;
        [MaxLength(5)]
        public string PhoneNumberCountryPrefix { get; set; } = null!;
        [MaxLength(15)]
        public string PhoneNumber { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string SocialCategory { get; set; } = null!;
        public string? HasInsurance { get; set; }
        public string PasswordHash { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string County { get; set; } = null!;
        public string City { get; set; } = null!;
        public string? StreetNumber { get; set; }
        public string? ApartmentBuildingFloor { get; set; }
        public string? Sector { get; set; }
        [MaxLength(6)]
        public string? ZipCode { get; set; }
        public IFormFile UserImage { get; set; } = null!;
    }
}
