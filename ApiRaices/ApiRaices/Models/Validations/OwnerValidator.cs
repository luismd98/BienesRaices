using FluentValidation;
using System;

namespace ApiRaices.Models.Validations
{
    public class OwnerValidator : AbstractValidator<Owner>
    {
        public OwnerValidator()
        {

            RuleFor(owner => owner.Name)
                .NotEmpty()
                .WithMessage("El nombre es requerido")
                .NotNull()
                .WithMessage("El nombre es requerido")
                .Length(3,100)
                .WithMessage("La longitud del nombre está por fuera del límite");

            RuleFor(owner => owner.Address)
                .NotEmpty()
                .WithMessage("El campo de Dirección es requerido")
                .NotNull()
                .WithMessage("El campo de Dirección es requerido")
                .Length(5, 100)
                .WithMessage("La longitud de la dirección está por fuera del límite");

            RuleFor(owner => owner.Birthday)
                .NotEmpty()
                .WithMessage("La fecha de nacimiento es requerida")
                .NotNull()
                .WithMessage("La fecha de nacimiento es requerida")
                .Must(ValidDateAndAge)
                .WithMessage("La fecha de nacimiento es inválida");
        }

        private bool ValidDateAndAge(string dateStr)
        {
            try
            {
                DateTime date = DateTime.Parse(dateStr);

                int currentYear = DateTime.Now.Year;
                int inputYear = date.Year;

                if (inputYear <= currentYear 
                    && inputYear > (currentYear - 120)
                     && (currentYear - inputYear) >= 18)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
