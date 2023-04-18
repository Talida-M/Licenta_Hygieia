using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.BusinessLogic.Implementation.Admin.ViewModels
{
    public class DoctorChartInfoModel
    {

        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public byte[] Image { get; set; } = null!;

    }
}
