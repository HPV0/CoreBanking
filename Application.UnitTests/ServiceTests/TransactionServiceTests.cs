using CoreBanking.Application.Common;
using CoreBanking.Application.DTOs.Transaction;
using CoreBanking.Application.Exceptions;
using CoreBanking.Application.Interfaces.Common;
using CoreBanking.Application.Interfaces.IRepositories;
using CoreBanking.Application.Interfaces.IServices;
using CoreBanking.Application.Mappers;
using CoreBanking.Application.Services;
using CoreBanking.Domain.Constants;
using CoreBanking.Domain.Entities.TransactionEntities;
using FluentAssertions;
using Moq;
using TestHelper;

namespace Application.UnitTests.ServiceTests
{
    public class TransactionServiceTests
    {
        private readonly Mock<ITransactionRepository> _transactionRepository = new();
        private readonly Mock<IAccountRepository> _accountRepository = new();
        private readonly Mock<ICurrencyRepository> _currencyRepository = new();
        private readonly IMoneyCalculator _moneyCalculator = new MoneyCalculator();
        private readonly TransactionMapper _mapper = new();
        private readonly IAccountBalanceService _accountBalanceService = new AccountBalanceService(new MoneyCalculator());
        private readonly Mock<IUnitOfWork> _unitOfWork = new();

        private TransactionService CreateService()
        {
            return new TransactionService(
                _mapper,
                _transactionRepository.Object,
                _accountRepository.Object,
                _currencyRepository.Object,
                _moneyCalculator,
                _accountBalanceService,
                _unitOfWork.Object
                );
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateTransactionAndCommit()
        {
            // Arrange
            var curr1 = TestEntityFactory.CreateCurrency(currencyCode: "ABC");
            var curr2 = TestEntityFactory.CreateCurrency(currencyCode: "ABD");
            var curr3 = TestEntityFactory.CreateCurrency(currencyCode: "ABE");

            var sender = TestEntityFactory.CreateAccount(currency: curr1);
            var receiver = TestEntityFactory.CreateAccount(currency: curr2);
            var baseAccount = TestEntityFactory.CreateAccount(id: BankAccount.Id, currency: curr3);

            decimal amount = 100;

            sender.Balance.Amount = amount;

            var request = new CreateTransactionRequest
            {
                SenderId = sender.Id,
                ReceiverId = receiver.Id,
                Amount = amount,
                SenderExchangeRate = 1,
                ReceiveExchangeRate = 1
            };

            var transactionDetails = new List<Transaction>();

            var expectedResponse = new List<TransactionDetailResponseModel>
            {
                new()
                {
                    SenderId = sender.Id,
                    ReceiverId = BankAccount.Id,
                    ExchangeRate = 1,
                    Amount = amount,
                },
                new()
                {
                    SenderId = BankAccount.Id,
                    ReceiverId = receiver.Id,
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
                    transactionDetails.Add(transaction);
                })
                .Returns(Task.CompletedTask);

            var service = CreateService();

            // Act
            var result = await service.CreateAsync(request);

            // Assert
            result.Should().BeEquivalentTo(expectedResponse);

            _transactionRepository.Verify(
                x => x.SaveChangesAsync(),
                Times.Once);
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
                    AccountId = new Random().Next(),
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
                    AccountId = new Random().Next(),
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
        public async Task CreateAsync_WithInsufficientFunds_Throw()
        {
            // Arrange
            var currency = TestEntityFactory.CreateCurrency(currencyCode: "ABC");

            var sender = TestEntityFactory.CreateAccount(
                currency: currency);

            var receiver = TestEntityFactory.CreateAccount(
                currency: currency);

            var baseAccount = TestEntityFactory.CreateAccount(
                currency: currency);

            decimal amount = 50;

            _accountBalanceService.Deposit(sender, amount);

            var request = new CreateTransactionRequest
            {
                SenderId = sender.Id,
                ReceiverId = receiver.Id,
                Amount = amount + 50,
                SenderExchangeRate = 1,
                ReceiveExchangeRate = 1
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

            var service = CreateService();

            // Act
            var act = () => service.CreateAsync(request);

            // Assert
            await act.Should()
                .ThrowAsync<UnprocessableException>();
        }


        [Fact]
        public async Task CreateAsync_DifferentCurrencies_CreateTwoDetails()
        {
            // Arrange
            var usd = TestEntityFactory.CreateCurrency(currencyCode: "USD");
            var amd = TestEntityFactory.CreateCurrency(currencyCode: "AMD");

            var sender = TestEntityFactory.CreateAccount(
                currency: usd);

            var receiver = TestEntityFactory.CreateAccount(
                currency: amd);

            var baseAccount = TestEntityFactory.CreateAccount(
                currency: usd);

            decimal amount = 100;

            _accountBalanceService.Deposit(sender, amount);

            var request = new CreateTransactionRequest
            {
                SenderId = sender.Id,
                ReceiverId = receiver.Id,
                Amount = amount,
                SenderExchangeRate = 1,
                ReceiveExchangeRate = 1
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

            var service = CreateService();

            // Act
            var result = await service.CreateAsync(request);

            // Assert
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task CreateAsync_DifferentCurrencies_WithdrawFromSenderAndDepositToReceiver()
        {
            // Arrange
            var usd = TestEntityFactory.CreateCurrency(currencyCode: "USD");
            var amd = TestEntityFactory.CreateCurrency(currencyCode: "AMD");

            var sender = TestEntityFactory.CreateAccount(currency: usd);
            var receiver = TestEntityFactory.CreateAccount(currency: amd);
            var baseAccount = TestEntityFactory.CreateAccount(currency: usd);

            decimal reciverExchangeRate = 400;
            decimal senderExchangeRate = 400;
            decimal sendingAmount = 100;

            _accountBalanceService.Deposit(sender, sendingAmount);

            var request = new CreateTransactionRequest
            {
                SenderId = sender.Id,
                ReceiverId = receiver.Id,
                Amount = sendingAmount,
                SenderExchangeRate = 1,
                ReceiveExchangeRate = 1
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

            var service = CreateService();

            // Act
            var result = await service.CreateAsync(request);

            // Assert
            sender.Balance.Amount.Should().Be(0);
            receiver.Balance.Amount.Should().Be(_moneyCalculator.ExchangeCalc(sendingAmount, senderExchangeRate, reciverExchangeRate));
        }
    }
}
