using CoreBanking.API.Validators.Extensions;
using CoreBanking.Application.DTOs.Account;
using CoreBanking.Application.DTOs.Client;
using FluentValidation;

namespace CoreBanking.API.Validators.AddValidators
{
    public class CreateAccountRequestValidator : AbstractValidator<CreateAccountRequest>
    {
        public CreateAccountRequestValidator()
        {
            RuleFor(x => x.CurrencyCode)
                .IsCurrencyCode();

            RuleFor(x => x.ClientId)
                .NotNull().WithMessage("ClientId cannot be null")
                .NotEmpty().WithMessage("ClientId is requird");

        }
    }
}
