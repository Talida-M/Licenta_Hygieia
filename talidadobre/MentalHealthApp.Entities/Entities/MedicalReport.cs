using MentalHealthApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.Entities.Entities
{
    public  class MedicalReport : IEntity
    {
        public Guid Id { get; set; }
        public Guid PacientId { get; set; }
        public Guid DoctorId { get; set; }
        public DateTime ReportDate { get; set; }
        public string ReportDescription { get; set; } = null!;
        public string? MedicalHistory { get; set; }
        public string Condition { get; set; } = null!;
        public string Strategy { get; set; } = null!;
        public string Prescription { get; set; } = null!;
        public string? DoctorNotes { get; set; }
        public virtual Specialist Specialist { get; set; } = null!;
        public virtual Pacient Pacient { get; set; } = null!;



    }
}
