using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.BusinessLogic.Implementation.Account.ViewModels
{
    public class DoctorNameModel
    {
        public Guid SpecialistId { get; set; } 
        public string DoctorName { get; set; } = null!;
    }
}
