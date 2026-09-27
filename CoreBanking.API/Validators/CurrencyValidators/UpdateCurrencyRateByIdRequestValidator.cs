using CoreBanking.API.Validators.Extensions;
using CoreBanking.Application.DTOs.Currency;
using FluentValidation;

namespace CoreBanking.API.Validators.CurrencyValidators
{
    public class UpdateCurrencyRateByIdRequestValidator : AbstractValidator<UpdateCurrencyRateByIdRequest>
    {
        public UpdateCurrencyRateByIdRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotNull().WithMessage("Id cannot be null.");

            RuleFor(x => x.Rate).IsValidRate();
        }
    }
}
