using FluentValidation;
using MentalHealthApp.BusinessLogic.Implementation.Appointments.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.BusinessLogic.Implementation.Appointments.Validations
{
    public class AppointmentValidation :AbstractValidator<AppointmentModel>
    {
        public AppointmentValidation()
        {
            RuleFor(r => r.AppointmentType)
                    .NotEmpty()
                    .Must(r => r.Equals("Fizic") || r.Equals("Online"))
                    .WithMessage("Introduceti cuvintele cheie Fizic/Online in functie de tipul de programare dorit!");
            RuleFor(r => Convert.ToDateTime(r.AppointmentDate).Date)
                    .NotEmpty()
                    .GreaterThanOrEqualTo(DateTime.Now.Date)
                    .WithMessage("Data nu poate fi una trecuta");
        }
    }
}
