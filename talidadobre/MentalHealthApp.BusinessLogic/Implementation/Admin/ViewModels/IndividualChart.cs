using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.BusinessLogic.Implementation.Admin.ViewModels
{
    public class IndividualChart
    {
        public int NumberOfAppointments { get; set; } = 0;
        public string AppointmentStatus { get; set; } = null!;
    }
}
