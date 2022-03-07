using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace ApiRaices.Models.Validations
{
    public class FileValidator : AbstractValidator<IFormFile>
    {
        public FileValidator()
        {

            //Validate the file extension
            RuleFor(x => x.ContentType)
                .NotNull()
                .Must(x => x.Equals("image/jpeg") || x.Equals("image/jpg") || x.Equals("image/png"))
                .WithMessage("File type is not allowed.");
        }
    }
}
