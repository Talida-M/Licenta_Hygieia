using FluentValidation.Results;
using MentalHealthApp.Common.Exceptions;

namespace MentalHealthApp.Common.Extensions
{
    public static class ValidationExtension
    {
        public static void ThenThrow(this ValidationResult result, object model)
        {
            if (!result.IsValid)
            {
                throw new ValidationErrorException(result, model);
            }
        }
    }
}
