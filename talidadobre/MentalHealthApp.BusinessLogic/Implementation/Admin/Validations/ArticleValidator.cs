using FluentValidation;
using MentalHealthApp.BusinessLogic.Implementation.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthApp.BusinessLogic.Implementation.Admin.Validations
{
    public class ArticleValidator : AbstractValidator<TopReadsVM>
    {
        public ArticleValidator()
        {
            RuleFor(r => r.ArticleTile)
                .NotEmpty().WithMessage("Camp obligatoriu!")
                .Must(NotAlreadyExist);
            RuleFor(r => r.Content)
                .NotEmpty().WithMessage("Camp obligatoriu!");
            RuleFor(r => r.ImageForm)
                .NotEmpty().WithMessage("Camp obligatoriu!"); 
          
        }
        public bool NotAlreadyExist(string email)
        {
            return true;
        }
    }
}
