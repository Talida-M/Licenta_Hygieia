using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.BusinessLogic.Implementation.Appointments.ViewModels
{
    public class AppointmentsVM
    {
        public int Id { get; set; }
        public string AppointmentDate { get; set; } = null!;
        //public string OraProgramare { get; set; } = null!;
        public string Doctor { get; set; } = null!;
        public string AppointmentType { get; set; } = null!;
        public string AppointmentStatus { get; set; } = null!;
    }
}
