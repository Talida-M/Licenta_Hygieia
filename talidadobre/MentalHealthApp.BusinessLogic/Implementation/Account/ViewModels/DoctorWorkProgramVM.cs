using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.BusinessLogic.Implementation.Account.ViewModels
{
    public class DoctorWorkProgramVM
    {
        public Guid Id { get; set; }
        public Guid SpecialistId { get; set; }
        public int WorkDayId { get; set; }
        public string WorkDay { get; set; } = null!;
        [MaxLength(8)]
        public string Start { get; set; } = null!;
        [MaxLength(8)]
        public string End { get; set; } = null!;
        public string? Notes { get; set; }
        public List<WorkDaysVM> workDaysVMs { get; set; } = null!;
        public List<DoctorWorkProgramVM> DoctorWorkProgramVMs { get; set; } = null!;
    }
}
