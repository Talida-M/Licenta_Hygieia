using FluentValidation;
using MentalHealthApp.BusinessLogic.Implementation.Account.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.BusinessLogic.Implementation.Account.Validations
{
    public class RegisterUserValidator : AbstractValidator<RegisterModel>
    {
        public RegisterUserValidator()
        {
            RuleFor(r => r.Email)
                .NotEmpty().WithMessage("Camp obligatoriu!")
                .Must(NotAlreadyExist)
                .EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible);
            RuleFor(r => r.PasswordHash)
                .NotEmpty().WithMessage("Camp obligatoriu!");
            RuleFor(r => Convert.ToDateTime(r.BirthDate))
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Invalid Date")
                .NotEmpty().WithMessage("Camp obligatoriu!");
            RuleFor(r => r.FirstName)
                .NotEmpty().WithMessage("Camp obligatoriu!");
            RuleFor(r => r.LastName)
                .NotEmpty().WithMessage("Camp obligatoriu!");
            RuleFor(r => r.PhoneNumberCountryPrefix)
                .MaximumLength(4).WithMessage("Incorect prefix")
                .NotEmpty().WithMessage("Camp Obligatoriu!");
            RuleFor(r => r.PhoneNumber)
                 .Matches(@"[0-9]+")
                 .WithMessage("Incorect numar de telefon")
                //.Matches(@"^(\+(([0-9]){1,3})[-.])?((((([0-9]){2,3})[-.]){1,2}([0-9]{4,10}))|([0-9]{10}))$")
                .NotEmpty().WithMessage("Camp Obligatoriu!");
            RuleFor(r => r.Country)
                .NotEmpty().WithMessage("Camp Obligatoriu!");
            RuleFor(r => r.County)
                .NotEmpty().WithMessage("Camp Obligatoriu!");
            RuleFor(r => r.City)
                .NotEmpty().WithMessage("Camp Obligatoriu!");
            RuleFor(r => r.Username)
                .NotEmpty().WithMessage("Camp Obligatoriu!");
            RuleFor(r => r.SocialCategory)
                .NotEmpty().WithMessage("Camp Obligatoriu!");

        }
        public bool NotAlreadyExist(string email)
        {
            return true;
        }
    }
}
