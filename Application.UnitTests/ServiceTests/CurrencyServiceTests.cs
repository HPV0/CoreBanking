using CoreBanking.Application.Exceptions;
using CoreBanking.Application.Interfaces.IRepositories;
using CoreBanking.Application.Mappers;
using CoreBanking.Application.Services;
using CoreBanking.Domain.Entities.CurrencyEntities;
using FluentAssertions;
using Moq;
using TestHelper;

namespace Application.UnitTests.ServiceTests
{
    public class CurrencyServiceTests
    {
        private readonly Mock<ICurrencyRepository> _currencyRepository = new();
        private readonly Mock<ICurrencyRateRepository> _currencyRateRepository = new();

        private readonly Mock<CurrencyMapper> _mapper = new();

        private CurrencyService CreateService()
        {
            return new CurrencyService(
                _currencyRepository.Object,
                _mapper.Object,
                _currencyRateRepository.Object);
        }

        [Fact]
        public async Task GetCurrencyByIdAsync_CurrencyExists_ReturnCurrency()
        {
            // Arrange
            var id = new Random().Next();
            decimal rate = 400;
            string code = "USD";

            var currencyRate = TestEntityFactory.CreateCurrencyRate(rate);

            var currency = TestEntityFactory.CreateCurrency(id, code, new() { currencyRate });

            _currencyRepository
                .Setup(x => x.GetCurrencyWithActiveRateByIdAsync(id))
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

            var currencyRate = TestEntityFactory.CreateCurrencyRate(rate);

            var currency = TestEntityFactory.CreateCurrency(currencyCode: currencyCode, currencyRates: new() { currencyRate });

            _currencyRepository
                .Setup(x => x.GetCurrencyWithActiveRateByCodeAsync(currencyCode))
                .ReturnsAsync(currency);

            var service = CreateService();

            // Act
            var result = await service.GetCurrencyByCodeAsync(currencyCode);

            // Assert
            result.CurrencyCode.Should().Be(currencyCode);
            result.Rate.Should().Be(rate);
        }

        [Fact]
        public async Task RemoveByIdAsync_CurrencyDoesNotExist_Throw()
        {
            // Arrange
            var id = new Random().Next();

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
