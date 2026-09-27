using System.Text.RegularExpressions;

namespace CoreBanking.Domain.ValueObjects.Client
{
    public sealed record  class Email
    {
        private static readonly int MaxLength = 40;
        public string Value { get; }

        private Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Email cannot be empty.");

            value = value.Trim();

            if (value.Length > MaxLength)
                throw new ArgumentException(
                    $"Email cannot exceed {MaxLength} characters.");

            if (!IsValid(value))
                throw new ArgumentException(
                    "Invalid email address.");

            Value = value;
        }

        public static Email Create(string value)
        {
            return new Email(value);
        }


        private static bool IsValid(string email)
        {
            return Regex.IsMatch(
                email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.CultureInvariant);
        }

    }
}
