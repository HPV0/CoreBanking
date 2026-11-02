using CoreBanking.Application.DTOs.Account;
using CoreBanking.Domain.Constants;
using FluentValidation;

namespace CoreBanking.API.Validators.AccountValidators
{
    public class CashFlowRequestValidator : AbstractValidator<CashFlowRequest>
    {
        public CashFlowRequestValidator()
        {
            RuleFor(x => x.Amount)
                .PrecisionScale(18, FinancialPrecision.AmountPrecision, true).WithMessage($"Amount cannot have more than 18 digits before the decimal point and {FinancialPrecision.AmountPrecision} decimal places.")
                .GreaterThan(0).WithMessage("Amount must be positive");

        }
    }
}
