using CoreBanking.API.Validators.Extensions;
using CoreBanking.Application.DTOs.Currency;
using CoreBanking.Application.DTOs.Transaction;
using FluentValidation;

namespace CoreBanking.API.Validators.TransactionValidators
{
    public class CreateTransactionRequestValidator : AbstractValidator<CreateTransactionRequest>
    {
        public CreateTransactionRequestValidator()
        {
            RuleFor(x => x.ReceiverId)
                .NotNull().WithMessage("ReciverId cannot be null");

            RuleFor(x => x.SenderId)
                .NotNull().WithMessage("SenderId cannot be null");

            RuleFor(x => x.SenderExchangeRate).IsValidRate();
            
            RuleFor(x => x.ReceiveExchangeRate).IsValidRate();

            RuleFor(x => x.Amount)
                .PrecisionScale(18, 2, true).WithMessage("Amount cannot have more than 18 digits before the decimal point and 2 decimal places.")
                .GreaterThan(0).WithMessage("Transaction amount must be positive");

        }
    }
}
