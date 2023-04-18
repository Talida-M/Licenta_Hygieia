using MentalHealthApp.BusinessLogic.Implementation.Account.ViewModels;
using MentalHealthApp.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.BusinessLogic.Implementation.Appointments.ViewModels
{
    public class AppointmentModel
    {
        public int Id { get; set; }
        public Guid SpecialistId { get; set; }
        public Guid UserId { get; set; }
        public string AppointmentDate { get; set; } = null!;
       public string AppointmentHour { get; set; } = null!;
        public string AppointmentType { get; set; } = null!;
        public string? AppointmentStatus { get; set; }

        public string AppointmentTime { get; set; } = null!;
        public string Price { get; set; } = null!;

        public List<AppointmentsVM> AppointmentsVMs = null!;
        public List<AppointmentsDoctorProfileVM> AcceptedAppointmentsData = null!;
        public List<DoctorWorkProgramVM> DoctorWorkProgramVMs = null!;
     
    }
}
