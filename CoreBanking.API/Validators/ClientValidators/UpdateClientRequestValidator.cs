using CoreBanking.Application.DTOs.Client;
using FluentValidation;

namespace CoreBanking.API.Validators.ClientValidators
{
    public class UpdateClientRequestValidator : AbstractValidator<UpdateClientRequest>
    {
        public UpdateClientRequestValidator()
        {

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email cannot be empty.")
                .When(x => x.Email != null)
                .EmailAddress().WithMessage("Invalid Email format.");

            RuleFor(x => x.PassportNumber)
                .NotEmpty().WithMessage("Passport Number cannot be empty.")
                .When(x => x.PassportNumber != null)
                .Length(12).WithMessage("Passport number must have length of 12.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is requird")
                .When(x => x.Name != null)
                .MaximumLength(20).WithMessage("Maximum length is 20")
                .Matches("^[a-zA-Z]*$").WithMessage("Name must contain only English characters"); ;

            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Surname is required")
                .When(x => x.Surname != null)
                .MaximumLength(20).WithMessage("Maximum length of Surname is 20")
                .Matches("^[a-zA-Z]*$").WithMessage("Surname must contain only English characters"); ;

            RuleFor(x => x.Birthday)
                .Must(date => !date.HasValue || date.Value < DateOnly.FromDateTime(DateTime.Now))
                .WithMessage("Date must be before today.");
        }
    }
}
