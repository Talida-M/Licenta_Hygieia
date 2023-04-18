using MentalHealthApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.Entities.Entities
{
    public class WorkDay : IEntity
    {
        public WorkDay()
        {
            DoctorSchedule = new HashSet<DoctorSchedule>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<DoctorSchedule> DoctorSchedule { get; set; }

    }
}
