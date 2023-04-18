using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.Common.DTOs
{
    public class ProgramareProfilMedicDto
    {
        public int Id { get; set; }
        public string AppointmentDate { get; set; } = null!;
        //public string OraProgramare { get; set; } 
        public Guid UserId { get; set; }
        public string Pacient { get; set; } = null!;
        public string? AppointmentStatus { get; set; }
    }
}
