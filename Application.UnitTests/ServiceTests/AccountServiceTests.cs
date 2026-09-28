using CoreBanking.Application.DTOs.Account;
using CoreBanking.Application.Exceptions;
using CoreBanking.Application.Interfaces.IRepositories;
using CoreBanking.Application.Mappers;
using CoreBanking.Application.Services;
using CoreBanking.Domain.Entities;
using CoreBanking.Domain.Entities.AccountEntities;
using CoreBanking.Domain.Entities.CurrencyEntities;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using System.Timers;

namespace Application.UnitTests.ServiceTests
{
public class AccountServiceTests
    {
        private readonly Mock<IAccountRepository> _accountRepository = new();
        private readonly Mock<IClientRepository> _clientRepository = new();
        private readonly Mock<ICurrencyRepository> _currencyRepository = new();
        private readonly Mock<AccountMapper> _mapper = new();

        private AccountService CreateService()
        {
            return new AccountService(
                _accountRepository.Object,
                _clientRepository.Object,
                _currencyRepository.Object,
                _mapper.Object);
        }


        [Fact]
        public async Task CreateAsync_ClientAlreadyHasAccountWithCurrency_Throw()
        {
            // Arrange
            var clientId = Guid.NewGuid();
            var currencyCode = "USD";

            var request = new CreateAccountRequest
            {
                ClientId = clientId,
                CurrencyCode = currencyCode
            };

            _accountRepository
                .Setup(x => x.ClientHasAccountWithCurrencyCodeAsync(
                    clientId,
                    currencyCode))
                .ReturnsAsync(true);

            var service = CreateService();

            // Act
            Func<Task> act = () => service.CreateAsync(request);
            // Assert
            await act.Should()
                .ThrowAsync<UnprocessableException>();

        }

        [Fact]
        public async Task CreateAsync_ClientDoesNotExist_Throw()
        {
            // Arrange
            var clientId = Guid.NewGuid();
            var currencyCode = "USD";

            var request = new CreateAccountRequest
            {
                ClientId = clientId,
                CurrencyCode = currencyCode
            };

            _accountRepository
                .Setup(x => x.ClientHasAccountWithCurrencyCodeAsync(
                    clientId,
                    currencyCode))
                .ReturnsAsync(false);

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
            var accountId = Guid.NewGuid();

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
            var accountId = Guid.NewGuid();

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
