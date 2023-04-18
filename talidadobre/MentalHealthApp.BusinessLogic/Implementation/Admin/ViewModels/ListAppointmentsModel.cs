using MentalHealthApp.BusinessLogic.Implementation.Account.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.BusinessLogic.Implementation.Admin.ViewModels
{
    public class ListAppointmentsModel
    {
        public List<AppointmentsAdminModel> AppointmentsAdminModels { get; set; } = null!;

        public List<DoctorNameModel> DoctorNameModels { get; set; } = null!;

    }
}
