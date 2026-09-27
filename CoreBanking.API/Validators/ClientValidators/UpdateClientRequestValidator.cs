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
                .NotNull().WithMessage("Email cannot be null.")
                .EmailAddress().WithMessage("Invalid Email format.");

            RuleFor(x => x.PassportNumber)
                .NotNull().WithMessage("Passport number cannot be null.")
                .Length(12).WithMessage("Passport number must have length of 12.");

        }
    }
}
