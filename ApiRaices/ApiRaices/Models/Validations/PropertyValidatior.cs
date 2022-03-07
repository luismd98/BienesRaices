using FluentValidation;
using System;

namespace ApiRaices.Models.Validations
{
    public class PropertyValidatior : AbstractValidator<Property>
    {
        public PropertyValidatior()
        {
            RuleFor(property => property.IdProperty)
                .NotNull()
                .NotEmpty()
                .Must(isNumber)
                .GreaterThan(0);

            RuleFor(property => property.Name)
                .NotNull()
                .NotEmpty()
                .Length(3, 100);

            RuleFor(property => property.Address)
                .NotNull()
                .NotEmpty()
                .Length(5,100);

            RuleFor(property => property.Price)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0)
                .Must(isFloat);

            RuleFor(property => property.CodeInternal)
                .NotNull()
                .NotEmpty()
                .Must(isNumber)
                .GreaterThan(0);

            RuleFor(property => property.Year)
                .NotNull()
                .NotEmpty()
                .NotNull()
                .NotEmpty()
                .Length(4); 

            RuleFor(property => property.IdOwner)
                .NotNull()
                .NotEmpty()
                .Must(isNumber)
                .GreaterThan(0);

        }

        private bool isFloat(float value)
        {
            if (float.TryParse(value.ToString(), out float j))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool isNumber(int value)
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
