using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.BusinessLogic.Implementation.Appointments.ViewModels
{
    public class AppointmentsDoctorProfileVM
    {
        public int Id { get; set; }
       // public DateTime AppointmentDate { get; set; }
        public string AppointmentDate { get; set; } = null!;
        public Guid UserId { get; set; }
        public string Pacient { get; set; } = null!;
        public string? AppointmentStatus { get; set; }
        public List<AppointmentsDoctorProfileVM> AppointmentDoctorProfileVMs = null!;
    }
}
