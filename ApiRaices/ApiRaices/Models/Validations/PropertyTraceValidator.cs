using FluentValidation;
using System;
using System.Security.Cryptography;

namespace ApiRaices.Models.Validations
{
    public class PropertyTraceValidator : AbstractValidator<PropertyTrace>
    {
        public PropertyTraceValidator()
        {

            //IdPropertyTrace: Identity value

            //missing DateSale, generated on runtime

            RuleFor(propertyTrace => propertyTrace.Name)
                .NotNull()
                .NotEmpty()
                .Length(3,100);


            RuleFor(propertyTrace => propertyTrace.Value)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0)
                .Must(BeNumber);

            RuleFor(propertyTrace => propertyTrace.Tax)
                .NotNull()
                .NotEmpty()
                .Must(BeNumber);

            RuleFor(propertyTrace => propertyTrace.IdProperty)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0)
                .Must(BeNumber);

            RuleFor(propertyTrace => propertyTrace.IdOwner)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0)
                .Must(BeNumber);
        }

        private bool BeNumber(float value)
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

        private bool BeNumber(byte value)
        {
            if (byte.TryParse(value.ToString(), out byte j))
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
