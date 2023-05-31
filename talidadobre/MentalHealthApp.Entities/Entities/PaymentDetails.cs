using MentalHealthApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.Entities.Entities
{
    public class PaymentDetails : IEntity
    {
        public Guid Id { get; set; }
        public int AppointmentId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int Amount { get; set; } = 0;
        public string Currency { get; set; } = "RON";
        public string PaymentStatus { get; set; } = "";
        public virtual Appointment Appointment { get; set; } = null!;
    }
}
