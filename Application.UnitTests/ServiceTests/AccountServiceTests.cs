using CoreBanking.Application.DTOs.Account;
using CoreBanking.Application.Exceptions;
using CoreBanking.Application.Interfaces.Common;
using CoreBanking.Application.Interfaces.IRepositories;
using CoreBanking.Application.Interfaces.IServices;
using CoreBanking.Application.Mappers;
using CoreBanking.Application.Services;
using CoreBanking.Domain.Entities;
using CoreBanking.Domain.Entities.AccountEntities;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.ServiceTests
{
    public class AccountServiceTests
    {
        private readonly Mock<IAccountRepository> _accountRepository = new();
        private readonly Mock<IClientRepository> _clientRepository = new();
        private readonly Mock<ICurrencyRepository> _currencyRepository = new();
        private readonly Mock<AccountMapper> _mapper = new();
        private readonly Mock<IMoneyCalculator> _moneyCalculator = new();
        private readonly Mock<IAccountBalanceService> _accountBalanceService = new();

        private AccountService CreateService()
        {
            return new AccountService(
                _accountRepository.Object,
                _clientRepository.Object,
                _currencyRepository.Object,
                _mapper.Object,
                _moneyCalculator.Object,
                _accountBalanceService.Object);
        }


        [Fact]
        public async Task CreateAsync_ClientDoesNotExist_Throw()
        {
            // Arrange
            var clientId = new Random().Next();

            var currencyCode = "USD";

            var request = new CreateAccountRequest
            {
                ClientId = clientId,
                CurrencyCode = currencyCode
            };

            _clientRepository
                .Setup(x => x.GetByIdAsync(clientId))
                .ReturnsAsync((Client?)null);

            var service = CreateService();

            // Act
            Func<Task> act = () => service.CreateAsync(request);

            // Assert
            await act.Should()
                .ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task GetAccountByIdAsync_AccountDoesNotExist_Throw()
        {
            // Arrange
            var accountId = new Random().Next();

            _accountRepository
                .Setup(x => x.GetByIdAsync(accountId))
                .ReturnsAsync((Account?)null);

            var service = CreateService();

            // Act
            Func<Task> act = () => service.GetAccountByIdAsync(accountId);

            // Assert
            await act.Should()
                .ThrowAsync<EntityNotFoundException>();

        }

        [Fact]
        public async Task RemoveAccountByIdAsync_AccountDoesNotExist_Throw()
        {
            // Arrange
            var accountId = new Random().Next();

            _accountRepository
                .Setup(x => x.GetOnlyAccountByIdAsync(accountId))
                .ReturnsAsync((Account?)null);

            var service = CreateService();

            // Act
            Func<Task> act = () => service.RemoveAccountByIdAsync(accountId);

            // Assert
            await act.Should()
                .ThrowAsync<EntityNotFoundException>();
        }
    }
}
