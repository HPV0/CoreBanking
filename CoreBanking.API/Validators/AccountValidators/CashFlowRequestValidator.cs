using CoreBanking.Application.DTOs.Account;
using FluentValidation;

namespace CoreBanking.API.Validators.AccountValidators
{
    public class CashFlowRequestValidator : AbstractValidator<CashFlowRequest>
    {
        public CashFlowRequestValidator()
        {
            RuleFor(x => x.Amount)
                .PrecisionScale(18, 4, true).WithMessage("Amount cannot have more than 18 digits before the decimal point and 4 decimal places.")
                .GreaterThan(0).WithMessage("Amount must be positive");

        }
    }
}
