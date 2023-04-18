using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.Common.DTOs
{
    public class AcceptedAppointmentsDto
    {
        public int Id { get; set; }
        public string AppointmentDate { get; set; } = null!;
        public Guid UserId { get; set; }
        public string Pacient { get; set; } = null!;
        public string? Specialist { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string SocialCategory { get; set; } = null!;
        public string? HasInsurance { get; set; }
        public string? AppointmentStatus { get; set; }
        public byte[]? UserImage { get; set; }
    }
}
