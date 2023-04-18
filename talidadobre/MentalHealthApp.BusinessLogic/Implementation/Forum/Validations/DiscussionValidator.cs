using FluentValidation;
using MentalHealthApp.BusinessLogic.Implementation.Forum.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.BusinessLogic.Implementation.Forum.Validations
{
    public class DiscussionValidator : AbstractValidator<CreateDiscussionVM>
    {
        public DiscussionValidator()
        {
            RuleFor(r => r.Title)
               .NotEmpty().WithMessage("Camp obligatoriu!");
            RuleFor(r => r.MessageContent)
                .NotEmpty().WithMessage("Camp obligatoriu!");
        }
        public bool NotAlreadyExist(string email)
        {
            return true;
        }
    }
}
