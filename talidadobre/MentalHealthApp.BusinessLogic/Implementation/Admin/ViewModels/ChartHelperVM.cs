using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.BusinessLogic.Implementation.Admin.ViewModels
{
    public class ChartHelperVM
    {
        public Guid SpecialistId { get; set; }
        public string? Name { get; set; }
        public double Value1 { get; set; } = 0;
        public double Value2 { get; set; } = 0;

    }
}
