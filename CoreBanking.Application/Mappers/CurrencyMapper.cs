using CoreBanking.Application.DTOs.Currency;
using CoreBanking.Domain.Entities.CurrencyEntities;
using Riok.Mapperly.Abstractions;

namespace CoreBanking.Application.Mappers
{
    [Mapper]
    public partial class CurrencyMapper
    {

        public partial CurrencyResponseModel ToCurrencyResponseModel(Currency currency);
        public partial List<CurrencyResponseModel> ToCurrencyResponseModels(List<Currency> currencies);

        public partial CurrencyWithAllRatesResponseModel ToCurrencyWithAllRatesResponseModel(Currency currency);

        private partial CurrencyRateResponseModel ToCurrencyRateResponseModel(CurrencyRate rate);
        private List<CurrencyRateResponseModel> MapRates(
        IReadOnlyCollection<CurrencyRate> rates)
        {
            return rates
                .OrderByDescending(x => x.ValidFrom)
                .Select(ToCurrencyRateResponseModel)
                .ToList();
        }

        public partial List<CurrencyWithAllRatesResponseModel> ToCurrencyWithAllRatesResponseModels(List<Currency> currencies);
    }
}
