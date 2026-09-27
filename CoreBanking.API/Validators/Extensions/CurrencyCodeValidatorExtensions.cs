using FluentValidation;

namespace CoreBanking.API.Validators.Extensions
{
    public static class CurrencyCodeValidatorExtensions
    {
        public static IRuleBuilderOptions<T, string> IsCurrencyCode<T>(
            this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotNull()
                .WithMessage("Currency code cannot be null")
                .Length(3)
                .Matches("^[A-Z]{3}$")
                .WithMessage("Currency code must consist of exactly 3 uppercase letters.");
        }

        public static IRuleBuilderOptions<T, decimal?> IsValidRate<T>(
            this IRuleBuilder<T, decimal?> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThan(0).WithMessage("Rate must be positive.")
                .PrecisionScale(18, 4, true).WithMessage("Amount cannot have more than 18 digits before the decimal point and 4 decimal places.");
        }

        public static IRuleBuilderOptions<T, decimal> IsValidRate<T>(
            this IRuleBuilder<T, decimal> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThan(0).WithMessage("Rate must be positive.")
                .PrecisionScale(18, 4, true).WithMessage("Amount cannot have more than 18 digits before the decimal point and 4 decimal places.");
        }
    }
}
