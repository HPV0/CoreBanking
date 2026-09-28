using CoreBanking.Application.Exceptions;
using CoreBanking.Domain.Entities;
using CoreBanking.Domain.Entities.AccountEntities;
using CoreBanking.Domain.ValueObjects;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using TestHelper;

namespace Domain.UnitTests.AccountTest
{
    public class AccountTests
    {
        
        [Fact]
        public void Deposit_SameCurrency_IncreaseBalance()
        {
            // Arrange
            string code = "ABC";
            decimal amount = 100;
            var client = TestEntityFactory.CreateClient();
            var currency = TestEntityFactory.CreateCurrency(currencyCode: code);
            var account = Account.Create(currency, client);
            var money = TestEntityFactory.CreateMoney(currencyCode: code, amount: amount);

            // Act
            account.Deposit(money);

            // Assert
            account.Balance.Amount.Should().Be(amount);
        }

        

        [Fact]
        public void Withdraw_InsufficientFunds_ThrowUnprocessableException()
        {
            // Arrange
            string code = "ABC";
            decimal amount = 100;
            var client = TestEntityFactory.CreateClient();
            var currency = TestEntityFactory.CreateCurrency(currencyCode: code);
            var account = Account.Create(currency, client);
            var money = TestEntityFactory.CreateMoney(currencyCode: code, amount: amount);

            account.Deposit(Money.Create(100, code));

            // Act
            Action act = () =>
                account.Withdraw(Money.Create(150, code));

            // Assert
            act.Should().Throw<UnprocessableException>();
        }

        [Fact]
        public void Withdraw_DifferentCurrency_ThrowUnprocessableException()
        {
            // Arrange
            string code = "ABC";
            string otherCode = "EDF";
            decimal amount = 100;
            var client = TestEntityFactory.CreateClient();
            var currency = TestEntityFactory.CreateCurrency(currencyCode: code);
            var account = Account.Create(currency, client);
            var money = TestEntityFactory.CreateMoney(currencyCode: otherCode, amount: amount);


            // Act
            Action act = () => account.Withdraw(money);

            // Assert
            act.Should()
                .Throw<UnprocessableException>();
        }



    }
}
