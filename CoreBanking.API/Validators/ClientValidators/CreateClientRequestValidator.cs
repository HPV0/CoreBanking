using CoreBanking.Application.DTOs.Client;
using CoreBanking.Application.DTOs.Client.CreateClient;
using FluentValidation;

namespace CoreBanking.API.Validators.ClientValidators
{
    public class CreateClientRequestValidator : AbstractValidator<CreateClientRequest>
    {
        public CreateClientRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotNull().WithMessage("Name cannot be null")
                .NotEmpty().WithMessage("Name is requird")
                .MaximumLength(20).WithMessage("Maximum length is 20");

            RuleFor(x => x.Surname)
                .NotNull().WithMessage("Surname cannot be null")
                .NotEmpty().WithMessage("Surname is requird")
                .MaximumLength(20).WithMessage("Maximum length of Surname is 20");


            RuleFor(x => x.Birthday)
                .Must(date => !date.HasValue || date.Value < DateOnly.FromDateTime(DateTime.Now))
                .WithMessage("Date must be before today.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email cannot be empty.")
                .NotNull().WithMessage("Email cannot be null.")
                .EmailAddress().WithMessage("Invalid Email format.");

            RuleFor(x => x.PassportNumber)
                .NotNull().WithMessage("Passport number cannot be null.")
                .Length(12).WithMessage("Passport number must have length of 12.");

        }
    }
}
