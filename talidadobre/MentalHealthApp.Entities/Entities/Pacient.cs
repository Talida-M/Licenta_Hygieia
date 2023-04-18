using MentalHealthApp.Common;
using MentalHealthApp.Entities.Entities;
using System;
using System.Collections.Generic;

namespace MentalHealthApp.Entities
{
    public partial class Pacient : IEntity
    {
        public Pacient()
        {
            CameraConferinta = new HashSet<CameraConferintum>();
            IstoricDiagnosticUtilizators = new HashSet<IstoricDiagnosticUtilizator>();
            Appointments = new HashSet<Appointment>();
            MedicalReports = new HashSet<MedicalReport>();
            Simptomes = new HashSet<Simptome>();
        }

        public Guid UserId { get; set; }
        public string Username { get; set; } = null!;
        public string SocialCategory { get; set; } = null!;
        public string? HasInsurance { get; set; }

        public virtual IdentityUser User { get; set; } = null!;
        public virtual ICollection<CameraConferintum> CameraConferinta { get; set; }
        public virtual ICollection<IstoricDiagnosticUtilizator> IstoricDiagnosticUtilizators { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<MedicalReport> MedicalReports { get; set; }
        public virtual ICollection<Simptome> Simptomes { get; set; }
        public virtual ICollection<DoctorReviews> DoctorReviews { get; set; }
        public virtual ICollection<UserJournal> UserJournals { get; set; }

    }
}
