using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.BusinessLogic.Implementation.Admin.ViewModels
{
    public class HomePageVM
    {
        public List<TopReadsVM> TopReads { get; set; } = null!;
        public List<ConditionsPostVM> ConditionsPostVMs { get; set; } = null!;
    }
}
