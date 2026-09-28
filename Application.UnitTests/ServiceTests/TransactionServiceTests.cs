using CoreBanking.Application.DTOs.Transaction;
using CoreBanking.Application.Exceptions;
using CoreBanking.Application.Interfaces.IRepositories;
using CoreBanking.Application.Mappers;
using CoreBanking.Application.Services;
using CoreBanking.Domain.Constants;
using CoreBanking.Domain.Entities.AccountEntities;
using CoreBanking.Domain.Entities.TransactionEntities;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TestHelper;

namespace Application.UnitTests.ServiceTests
{
    public class TransactionServiceTests
    {
        private readonly Mock<ITransactionRepository> _transactionRepository = new();
        private readonly Mock<IAccountRepository> _accountRepository = new();
        private readonly Mock<ICurrencyRepository> _currencyRepository = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly TransactionMapper _mapper = new();

        private TransactionService CreateService()
        {
            return new TransactionService(
                _mapper,
                _transactionRepository.Object,
                _accountRepository.Object,
                _currencyRepository.Object,
                _unitOfWork.Object);
        }


        [Fact]
        public async Task CreateAsync_ShouldCreateTransactionAndCommit()
        {
            // Arrange
            var curr1 = TestEntityFactory.CreateCurrency("ABC");
            var curr2 = TestEntityFactory.CreateCurrency("ABD");
            var curr3 = TestEntityFactory.CreateCurrency("ABE");

            var sender = TestEntityFactory.CreateAccount(currency: curr1);
            var receiver = TestEntityFactory.CreateAccount(currency: curr2);
            var baseAccount = TestEntityFactory.CreateAccount(BankAccount.Id, currency: curr3);

            decimal amount = 100;
            sender.Deposit(TestEntityFactory.CreateMoney(amount, curr1.CurrencyCode));

            var request = new CreateTransactionRequest
            {
                SenderId = sender.Id,
                ReceiverId = receiver.Id,
                Amount = amount,
                SenderExchangeRate = 1,
                ReceiveExchangeRate = 1
            };

            var transactionDetails = new List<TransactionDetail>();

            var expectedResponse = new List<TransactionDetailResponseModel> {
                new(){ 
                    SenderId = sender.Id,
                    ReceiverId = BankAccount.Id,
                    ExchangeRate = 1,
                    Amount = amount,
                },
                new(){
                    ReceiverId = receiver.Id,
                    SenderId = BankAccount.Id,
                    ExchangeRate = 1,
                    Amount = amount,
                }
            };

            _accountRepository
                .Setup(x => x.GetByIdAsync(sender.Id))
                .ReturnsAsync(sender);

            _accountRepository
                .Setup(x => x.GetByIdAsync(receiver.Id))
                .ReturnsAsync(receiver);

            _accountRepository
                .Setup(x => x.GetByIdAsync(BankAccount.Id))
                .ReturnsAsync(baseAccount);

            _transactionRepository
                .Setup(x => x.AddAsync(It.IsAny<Transaction>()))
                .Callback<Transaction>(transaction =>
                {
                    transactionDetails = transaction.Details!.ToList();
                })
                .Returns(Task.CompletedTask);

            

            var service = CreateService();

            // Act
            var result = await service.CreateAsync(request);

            // Assert
            result.Should().BeEquivalentTo(expectedResponse);

            _unitOfWork.Verify(
                x => x.CommitTransactionAsync(),
                Times.Once);

            _unitOfWork.Verify(
                x => x.RollbackTransactionAsync(),
                Times.Never);
        }



        [Fact]
        public async Task GetAccountsTotalDepositBetweenDatesAsync_ShouldReturnRepositoryResult()
        {
            // Arrange
            var timeFrom = DateTime.UtcNow.AddDays(-1);
            var timeTo = DateTime.UtcNow;

            var expected = new List<AccountTotalDepositesResponseModel>
            {
                new()
                {
                    AccountId = Guid.NewGuid(),
                    CurrencyCode = "ABC",
                    TotalDeposites = 500
                }
            };

            _transactionRepository
                .Setup(x => x.GetAccountsTotalDepositBetweenDatesAsync(
                    timeFrom,
                    timeTo))
                .ReturnsAsync(expected);

            var service = CreateService();

            // Act
            var result = await service.GetAccountsTotalDepositBetweenDatesAsync(
                timeFrom,
                timeTo);

            // Assert
            result.Should().BeSameAs(expected);

        }


        [Fact]
        public async Task GetTotalWithdrawalsByAccountsAsync_ShouldReturnRepositoryResult()
        {
            // Arrange
            var timeFrom = DateTime.UtcNow.AddDays(-1);
            var timeTo = DateTime.UtcNow;

            var expected = new List<AccountTotalWithdrawalsResponseModel>
            {
                new()
                {
                    AccountId = Guid.NewGuid(),
                    TotalWithdrawals = 200
                }
            };

            _transactionRepository
                .Setup(x => x.GetTotalWithdrawalsByAccountsAsync(
                    timeFrom,
                    timeTo))
                .ReturnsAsync(expected);

            var service = CreateService();

            // Act
            var result = await service.GetTotalWithdrawalsByAccountsAsync(
                timeFrom,
                timeTo);

            // Assert
            result.Should().BeSameAs(expected);

        }


        [Fact]
        public async Task GetTotalCashFlowsByAccountsAsync_ShouldCalculateCashFlow()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            string code = "ABV";
            decimal totalDeposites = 1000;
            decimal totalWithdrawals = 300;

            var deposits = new List<AccountTotalDepositesResponseModel>
            {
                new()
                {
                    AccountId = accountId,
                    CurrencyCode = code,
                    TotalDeposites = totalDeposites
                }
            };

            var withdrawals = new List<AccountTotalWithdrawalsResponseModel>
            {
                new()
                {
                    AccountId = accountId,
                    CurrencyCode = code,
                    TotalWithdrawals = totalWithdrawals
                }
            };

            _transactionRepository
                .Setup(x => x.GetAccountsTotalDepositBetweenDatesAsync(
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>()))
                .ReturnsAsync(deposits);

            _transactionRepository
                .Setup(x => x.GetTotalWithdrawalsByAccountsAsync(
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>()))
                .ReturnsAsync(withdrawals);

            var service = CreateService();

            // Act
            var result = await service.GetTotalCashFlowsByAccountsAsync(
                DateTime.UtcNow.AddDays(-1),
                DateTime.UtcNow);

            // Assert
            result.Should().ContainSingle();

            result[0].Id.Should().Be(accountId);
            result[0].CurrencyCode.Should().Be(code);
            result[0].IncomesAmount.Should().Be(totalDeposites);
            result[0].SpendingAmount.Should().Be(totalWithdrawals);
            result[0].BalanceChangeAmount.Should().Be(totalDeposites- totalWithdrawals);
        }

    }

}
