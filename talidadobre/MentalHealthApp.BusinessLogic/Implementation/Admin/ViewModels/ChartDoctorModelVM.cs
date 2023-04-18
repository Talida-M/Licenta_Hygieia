using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.BusinessLogic.Implementation.Admin.ViewModels
{
    public class ChartDoctorModelVM
    {
        public Guid SpecialistId { get; set; }
        public string? Name { get; set; }
        public double Info { get; set; } = 0;
    }
}
