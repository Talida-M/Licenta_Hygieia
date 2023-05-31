using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.Common.DTOs
{
    public class DoctorsCardsDto
    {
        public byte[] DoctorImage { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Specialitate { get; set; } = null!;
        public string City { get; set; } = null!;
        public int Price { get; set; } = 0;
        public double Rating { get; set; } = 0;
    }
}
