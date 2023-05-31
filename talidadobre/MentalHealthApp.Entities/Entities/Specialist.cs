using MentalHealthApp.Common;
using MentalHealthApp.Entities.Entities;
using System;
using System.Collections.Generic;

namespace MentalHealthApp.Entities
{
    public partial class Specialist :IEntity
    {
        public Specialist()
        {
            CameraConferinta = new HashSet<CameraConferintum>();
            Appointment = new HashSet<Appointment>();
            MedicalReports = new HashSet<MedicalReport>();
        }

        public Guid SpecialistId { get; set; }
        public string Specialty { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int AppointmentDate { get; set; }
        public int Price { get; set; }

        public virtual IdentityUser SpecialistNavigation { get; set; } = null!;
        public virtual ICollection<CameraConferintum> CameraConferinta { get; set; }
        public virtual ICollection<Appointment> Appointment { get; set; }
        public virtual ICollection<MedicalReport> MedicalReports { get; set; }
        public virtual ICollection<DoctorSchedule> DoctorSchedule { get; set; }
        public virtual ICollection<DoctorReviews> DoctorReviews { get; set; }
        public virtual DoctorCV DoctorCVs { get; set; } = null!;


    }
}
