using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.Entities.Enums
{
    public enum AppointmentStatus :int
    {
       In_Asteptare = 1,
       Programare_Acceptata,
       Programare_Respinsa,
       Programare_Realizata,
       Programare_Anulata
    }
}
