using MentalHealthApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.Entities.Entities
{
    public class DoctorSchedule : IEntity
    { 
        public Guid Id { get; set; }
        public Guid SpecialistId { get; set; }
        public int WorkDayId { get; set; }
        public string Start { get; set; } = null!;
        public string End { get; set; } = null!;
        public string? Notes { get; set; }
        public virtual Specialist Specialist { get; set; }
        public virtual WorkDay WorkDay { get; set; }
    }
}
