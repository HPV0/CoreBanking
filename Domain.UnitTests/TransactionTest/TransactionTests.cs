using CoreBanking.Application.Exceptions;
using CoreBanking.Domain.Entities;
using CoreBanking.Domain.Entities.CurrencyEntities;
using CoreBanking.Domain.Entities.TransactionEntities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using TestHelper;

namespace Domain.UnitTests.TransactionTest
{
    public class TransactionTests
    {
        [Fact]
        public void Create_SameCurrency_CreateOneDetail()
        {
            // Arrange
            string code = "ABC";
            decimal amount = 40;
            var currency = TestEntityFactory.CreateCurrency(code);
            var sender = TestEntityFactory.CreateAccount(
                currency: currency);

            var receiver = TestEntityFactory.CreateAccount(
                currency: currency);

            sender.Deposit(TestEntityFactory.CreateMoney(100, code));

            var baseAccount = TestEntityFactory.CreateAccount(
                currency: currency);

            // Act
            var transaction = Transaction.Create(
                sender,
                receiver,
                baseAccount,
                1, 1,
                amount);

            // Assert
            transaction.Details.Should().ContainSingle();

            var detail = transaction.Details!.Single();

            detail.AccountFrom.Should().Be(sender);
            detail.AccountTo.Should().Be(receiver);
            detail.ExchangeRate.Should().Be(1);
            detail.Amount.Should().Be(40);
        }


        [Fact]
        public void Create_SameCurrency_DepositToReceiverAndWithdrowFromSender()
        {
            // Arrange
            string code = "ABC";
            decimal depositAmount = 100;
            decimal sendingAmount = 40;
            var currency = TestEntityFactory.CreateCurrency(code);
            var sender = TestEntityFactory.CreateAccount(
                currency: currency);

            var receiver = TestEntityFactory.CreateAccount(
                currency: currency);

            sender.Deposit(TestEntityFactory.CreateMoney(depositAmount, code));

            var baseAccount = TestEntityFactory.CreateAccount(
                currency: TestEntityFactory.CreateCurrency(code));

            // Act
            Transaction.Create(
                sender,
                receiver,
                baseAccount,
                1, 1,
                sendingAmount);

            // Assert
            receiver.Balance.Amount.Should().Be(40);
            sender.Balance.Amount.Should().Be(depositAmount - sendingAmount);
        }

        [Fact]
        public void Create_WithInsufficientFunds_Throw()
        {
            // Arrange
            var currency = TestEntityFactory.CreateCurrency("ABC");

            var sender = TestEntityFactory.CreateAccount(
                currency: currency);

            var receiver = TestEntityFactory.CreateAccount(
                currency: currency);

            var baseAccount = TestEntityFactory.CreateAccount(
                currency: currency);

            sender.Deposit(TestEntityFactory.CreateMoney(50, "ABC"));

            // Act
            Action act = () => Transaction.Create(
                sender,
                receiver,
                baseAccount,
                1, 1,
                100);

            // Assert
            act.Should()
                .Throw<UnprocessableException>();
        }


        [Fact]
        public void Create_DifferentCurrencies_CreateTwoDetails()
        {
            // Arrange
            var usd = TestEntityFactory.CreateCurrency("USD");
            var amd = TestEntityFactory.CreateCurrency("AMD");

            var sender = TestEntityFactory.CreateAccount(
                currency: usd);

            var receiver = TestEntityFactory.CreateAccount(
                currency: amd);

            var baseAccount = TestEntityFactory.CreateAccount(
                currency: usd);

            sender.Deposit(
                TestEntityFactory.CreateMoney(100, "USD"));

            // Act
            var transaction = Transaction.Create(
                sender,
                receiver,
                baseAccount,
                1,
                400,
                100);

            // Assert
            transaction.Details.Should().HaveCount(2);
        }

        [Fact]
        public void Create_DifferentCurrencies_WithdrawFromSenderAndDepositToReceiver()
        {
            // Arrange
            var usd = TestEntityFactory.CreateCurrency("USD");
            var amd = TestEntityFactory.CreateCurrency("AMD");

            var sender = TestEntityFactory.CreateAccount(currency: usd);
            var receiver = TestEntityFactory.CreateAccount(currency: amd);
            var baseAccount = TestEntityFactory.CreateAccount(currency: usd);

            decimal reciverExchangeRate = 400;
            decimal senderExchangeRate = 400;
            decimal sendingAmount = 100;
            sender.Deposit(
                TestEntityFactory.CreateMoney(sendingAmount, "USD"));

            

            // Act
            Transaction.Create(
                sender,
                receiver,
                baseAccount,
                senderExchangeRate,
                reciverExchangeRate,
                sendingAmount);

            // Assert
            sender.Balance.Amount.Should().Be(0);
            receiver.Balance.Amount.Should().Be(senderExchangeRate * sendingAmount / senderExchangeRate);
        }

    }
}
