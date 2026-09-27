using CoreBanking.Domain.Common;
using CoreBanking.Domain.Entities.AccountEntities;
using CoreBanking.Domain.ValueObjects.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using System.Text;
using System.Xml.Linq;

namespace CoreBanking.Domain.Entities
{
    public class Client : BaseEntity
    {
        public static readonly int PassportNumberLength = 12;
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public Email Email { get; private set; }
        public DateOnly? Birthday { get; private set; }
        public string PassportNumber { get; private set; }

        private HashSet<Account> _accounts = new();
        public IReadOnlyCollection<Account>? Accounts => _accounts?.ToList();

        private Client() { }


        private Client(
            Guid id, string name, string surname,
            string email, DateOnly? birthday,
            string passportNumber, ICollection<Account>? accounts) 
            : this(name, surname, email, birthday, passportNumber, accounts)
        {
            Id = id;
        }

        private Client(
            string name,
            string surname,
            string email,
            DateOnly? birthday,
            string passportNumber,
            ICollection<Account>? accounts)
        {
            _accounts = accounts?.ToHashSet() ?? new HashSet<Account>();

            Update(name, surname,birthday,passportNumber, email);
        }

        public static Client Create(
            string name,
            string surname,
            string email,
            DateOnly? birthday,
            string passportNumber,
            ICollection<Account>? accounts)
        {
            return new Client(
                name, surname,
                email, birthday,
                passportNumber, accounts);
        }

        public static Client Create(
            Guid id,
            string name,
            string surname,
            string email,
            DateOnly? birthday,
            string passportNumber,
            ICollection<Account>? accounts)
        {
            return new Client(
                id,
                name,
                surname,
                email,
                birthday,
                passportNumber,
                accounts);
        }


        // Update methods
        public void Update(
            string name, string surname,
            DateOnly? birthday,
            string passportNumber,
            string email)
        {
            UpdateEmail(email);
            UpdateName(name);
            UpdateSurname(surname);
            UpdatePassportNumber(passportNumber);
            UpdateBirthday(birthday);
        }

        public void UpdateEmail(string email)
        {
            Email = Email.Create(email);
        }

        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.");
            Name = name;
        }

        public void UpdateSurname(string surname)
        {
            if (string.IsNullOrWhiteSpace(surname))
                throw new ArgumentException("Surname cannot be empty.");
            Surname = surname;
        }

        public void UpdatePassportNumber(string passportNumber)
        {
            if (string.IsNullOrWhiteSpace(passportNumber))
                throw new ArgumentException("Name cannot be empty.");

            if(passportNumber.Length != PassportNumberLength)
                throw new ArgumentException("Wrong passport number size.");

            PassportNumber = passportNumber;
        }

        public void UpdateBirthday(DateOnly? birthday)
        {
            if (birthday != null && birthday > DateOnly.FromDateTime(DateTime.UtcNow))
                throw new ArgumentException("Birthday must be in past");
            Birthday = birthday;
        }


        // Accounts managemnt
        public void AddAccount(Account account)
        {
            if (account == null)
                throw new ArgumentException("Cannot add null account to Client.");

            var _account = _accounts.FirstOrDefault(a => a.Id == account.Id);
            if (_account != null)
                throw new ArgumentException("Account already exist.");

            _accounts.Add(account!);

        }

        public void RemoveAccount(Guid accountId)
        {
            var account = _accounts.FirstOrDefault(a => a.Id == accountId);
            if (account is null)
                throw new ArgumentException("Account not found.");

            _accounts.Remove(account);
        }
    }
}
