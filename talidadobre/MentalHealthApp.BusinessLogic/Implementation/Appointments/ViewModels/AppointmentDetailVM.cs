using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.BusinessLogic.Implementation.Appointments.ViewModels
{
	public class AppointmentDetailVM
	{
        public int Id { get; set; }
        public string AppointmentDate { get; set; } = null!;
        //public string OraProgramare { get; set; } = null!;
        public string Doctor { get; set; } = null!;
        public string Pacient { get; set;} = null!;

        public int Pret { get; set; } = 0;
    }
}
