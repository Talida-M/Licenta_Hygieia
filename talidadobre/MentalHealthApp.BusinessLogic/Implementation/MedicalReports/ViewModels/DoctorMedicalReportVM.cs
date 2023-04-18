using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.BusinessLogic.Implementation.MedicalReports.ViewModels
{
    public class DoctorMedicalReportVM
    {
        public Guid Id { get; set; }
        public Guid PacientId { get; set; }
        public string Pacient { get; set; } = null!;
        public string PacientEmail { get; set; } = null!;
        public Guid DoctorId { get; set; }
        public string? Doctor { get; set; }
        public DateTime ReportDate { get; set; }
        public string ReportDescription { get; set; } = null!;
        public string? MedicalHistory { get; set; }
        public string Condition { get; set; } = null!;
        public string Strategy { get; set; } = null!;
        public string Prescription { get; set; } = null!;
        public string? DoctorNotes { get; set; }
    }
}
