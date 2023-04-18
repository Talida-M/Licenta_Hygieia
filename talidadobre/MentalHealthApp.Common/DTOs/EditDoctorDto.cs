using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.Common.DTOs
{
    public class EditDoctorDto
    {
        public Guid SpecialistId { get; set; }
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumberCountryPrefix { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Specialty { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int AppointmentDate { get; set; }
        public string Country { get; set; } = null!;
        public string County { get; set; } = null!;
        public string City { get; set; } = null!;
        public string? StreetNumber { get; set; }
        public string? ApartmentBuildingFloor { get; set; }
        public string? Sector { get; set; }
        public string? ZipCode { get; set; }
        public byte[] DoctorImage { get; set; } = null!;
        public int Price { get; set; }
    }
}
