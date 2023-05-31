using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.BusinessLogic.Implementation.PayPal.ViewModels
{
    public class PaymentModel
    {
        //public Guid Id { get; set; }
        public int AppointmentId { get; set; }
        public int Amount { get; set; } = 0;
        public string Currency { get; set; } = "RON";
        public string PaymentStatus { get; set; } = "";

    }
}
