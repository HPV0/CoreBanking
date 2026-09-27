using CoreBanking.API.Validators.Extensions;
using CoreBanking.Application.DTOs.Client;
using CoreBanking.Application.DTOs.Currency;
using FluentValidation;

namespace CoreBanking.API.Validators.CurrencyValidators
{
    public class CreateCurrencyRequestValidator : AbstractValidator<CreateCurrencyRequest>
    {
        public CreateCurrencyRequestValidator()
        {
            RuleFor(x => x.CurrencyCode).IsCurrencyCode();

            RuleFor(x => x.Rate).IsValidRate();
        }
    }
}
