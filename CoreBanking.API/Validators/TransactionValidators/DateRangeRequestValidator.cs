using CoreBanking.Application.DTOs.Transaction;
using FluentValidation;

namespace CoreBanking.API.Validators.TransactionValidators
{
    public class DateRangeRequestValidator : AbstractValidator<DateRangeRequest>
    {
        public DateRangeRequestValidator()
        {
            RuleFor(x => x.timeFrom)
            .NotNull().WithMessage("TimeFrom cannot be null.");

            RuleFor(x => x.timeTo)
                .NotNull().WithMessage("timeTo cannot be null.");

            RuleFor(x => x)
                .Must(x => x.timeFrom <= x.timeTo)
                .WithMessage("TimeFrom cannot be biger then TimeTo.");


        }
    }
}
