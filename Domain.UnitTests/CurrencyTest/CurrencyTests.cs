using CoreBanking.Application.Exceptions;
using CoreBanking.Domain.Entities.CurrencyEntities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using TestHelper;

namespace Domain.UnitTests.CurrencyTest
{
    public class CurrencyTests
    {
        [Fact]
        public void AddCurrencyRate_ValidRate_AddRate()
        {
            // Arrange
            var currency = TestEntityFactory.CreateCurrency("ABC");
            var timeFrom = DateTimeOffset.Parse("2026-09-27T18:08:00Z");
            var rate = CurrencyRate.Create(1, currency, timeFrom, null); 

            // Act
            currency.AddCurrencyRate(rate);

            // Assert
            currency.Rates.Should().ContainSingle();
            currency.Rates.Should().Contain(rate);
        }

      

        [Fact]
        public void AddCurrencyRate_CurrentRateAlreadyExists_ThrowUnprocessableException()
        {
            // Arrange
            var currency = TestEntityFactory.CreateCurrency();
            var timeFrom = DateTimeOffset.Parse("2026-09-27T18:08:00Z");

            var firstRate = CurrencyRate.Create( 1, 
                currency,
                timeFrom,
                null);

            var secondRate = CurrencyRate.Create(1,
                currency,
                timeFrom,
                null);

            currency.AddCurrencyRate(firstRate);

            // Act
            Action act = () => currency.AddCurrencyRate(secondRate);

            // Assert
            act.Should().Throw<UnprocessableException>();
        }

        [Fact]
        public void FindCurrentRate_CurrentRateExists_ReturnCurrentRate()
        {
            // Arrange
            var currency = TestEntityFactory.CreateCurrency("USD");

            var currentRate = CurrencyRate.Create(
                1,
                currency,
                validFrom: DateTimeOffset.UtcNow.AddHours(-1),
                validTo: null);

            currency.AddCurrencyRate(currentRate);

            // Act
            var result = currency.FindCurrentRate();

            // Assert
            result.Should().Be(currentRate);
        }

    }
}
