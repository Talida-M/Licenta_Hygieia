using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.Common.DTOs
{
    public class ProgramareDto
    {

        public int Id { get; set; }
        public string AppointmentDate { get; set; } = null!;
        public Guid SpecialistId { get; set; }
        public Guid UserId { get; set; }
        public string Doctor { get; set; } = null!;
        public string AppointmentType { get; set; } = null!;
        public string? AppointmentStatus { get; set; }
    }
}
