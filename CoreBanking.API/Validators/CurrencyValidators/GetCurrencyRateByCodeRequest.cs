using CoreBanking.API.Validators.Extensions;
using CoreBanking.Application.DTOs.Currency;
using FluentValidation;

namespace CoreBanking.API.Validators.CurrencyValidators
{
    public class GetCurrencyRateByCodeRequestValidator : AbstractValidator<GetCurrencyRateByCodeRequest>
    {
        public GetCurrencyRateByCodeRequestValidator()
        {
            RuleFor(x => x.Code).IsCurrencyCode();

        }
    }
    
}
