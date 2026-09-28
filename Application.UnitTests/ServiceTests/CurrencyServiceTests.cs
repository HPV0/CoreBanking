using CoreBanking.Application.DTOs.Currency;
using CoreBanking.Application.Exceptions;
using CoreBanking.Application.Interfaces.IRepositories;
using CoreBanking.Application.Mappers;
using CoreBanking.Application.Services;
using CoreBanking.Domain.Entities.CurrencyEntities;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TestHelper;

namespace Application.UnitTests.ServiceTests
{
    public class CurrencyServiceTests
        {
        private readonly Mock<ICurrencyRepository> _currencyRepository = new();
        private readonly Mock<CurrencyMapper> _mapper = new();

        private CurrencyService CreateService()
        {
            return new CurrencyService(
                _currencyRepository.Object,
                _mapper.Object);
        }


        [Fact]
        public async Task GetCurrencyByIdAsync_CurrencyExists_ReturnCurrency()
        {
            // Arrange
            var id = Guid.NewGuid();
            decimal rate = 400;
            string code = "USD";

            var currency = TestEntityFactory.CreateCurrency(code);
            var currencyRate = CurrencyRate.Create(
                rate,
                currency,
                DateTimeOffset.UtcNow,
                null);

            currency.AddCurrencyRate(currencyRate);

            _currencyRepository
                .Setup(x => x.GetCurrencyWithCurrentRateByIdAsync(id))
                .ReturnsAsync(currency);

            var service = CreateService();

            // Act
            var result = await service.GetCurrencyByIdAsync(id);

            // Assert
            result.CurrencyCode.Should().Be(code);
            result.Rate.Should().Be(rate);

        }


        [Fact]
        public async Task GetRateCurrencyByCodeAsync_ReturnCurrencyRate()
        {
            // Arrange
            var currencyCode = "USD";
            decimal rate = 400;
            var currency = Currency.Create(currencyCode);
            var currencyRate = CurrencyRate.Create(
                rate,
                currency,
                DateTimeOffset.UtcNow,
                null);

            currency.AddCurrencyRate(currencyRate);

            _currencyRepository
                .Setup(x => x.GetCurrencyWithCurrentRateByCodeAsync(currencyCode))
                .ReturnsAsync(currency);

            var service = CreateService();

            // Act
            var result = await service.GetRateCurrencyByCodeAsync(currencyCode);

            // Assert
            result.CurrencyCode.Should().Be(currencyCode);
            result.Rate.Should().Be(rate);

        }

        

        [Fact]
        public async Task RemoveByIdAsync_CurrencyDoesNotExist_Throw()
        {
            // Arrange
            var id = Guid.NewGuid();

            _currencyRepository
                .Setup(x => x.GetOnlyCurrencyByIdAsync(id))
                .ReturnsAsync((Currency?)null);

            var service = CreateService();

            // Act
            Func<Task> act = () =>
                service.RemoveByIdAsync(id);

            // Assert
            await act.Should()
                .ThrowAsync<EntityNotFoundException>();

        }
    }

}
