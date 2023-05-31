using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.BusinessLogic.Implementation.Account.ViewModels
{
    public class InactiveDoctor
    {
        public Guid SpecialistId { get; set; }
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Specialty { get; set; } = null!;
        public string County { get; set; } = null!;
        public string City { get; set; } = null!;
        public byte[]? CV { get; set; }
        public int CVId { get; set; }
    }
}
