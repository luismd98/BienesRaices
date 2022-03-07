using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;

namespace ApiRaices.Models.Validations
{
    public class PropertyImageValidator : AbstractValidator<PropertyImage>
    {
        public PropertyImageValidator()
        {
            RuleFor(pImage => pImage.IdProperty)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0)
                .Must(BeNumber);

            RuleFor(pImage => pImage.Photo)
                .NotNull()
                .NotEmpty()
                .Length(4, 100);

        }

        private bool BeNumber(int value)
        {
            if (Int32.TryParse(value.ToString(), out int j))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
