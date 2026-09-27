using CoreBanking.API.Validators.Extensions;
using CoreBanking.Application.DTOs.Currency;
using FluentValidation;

namespace CoreBanking.API.Validators.CurrencyValidators
{
    public class UpdateCurrencyRateByCodeRequestValidator : AbstractValidator<UpdateCurrencyRateByCodeRequest>
    {
        public UpdateCurrencyRateByCodeRequestValidator()
        {
            RuleFor(x => x.CurrencyCode).IsCurrencyCode();

            RuleFor(x => x.Rate).IsValidRate();
        }
    }
}
