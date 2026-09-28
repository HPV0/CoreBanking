using CoreBanking.Application.DTOs.Client.CreateClient;
using CoreBanking.Application.DTOs.Currency;
using CoreBanking.Application.DTOs.Transaction;
using CoreBanking.Domain.Entities;
using CoreBanking.Domain.Entities.CurrencyEntities;
using CoreBanking.Domain.Entities.TransactionEntities;
using Riok.Mapperly.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBanking.Application.Mappers
{
    [Mapper]
    public partial class CurrencyMapper
    {

        public partial CurrencyResponseModel CurrencyToCurrencyResponseModel(Currency currency);
        public partial List<CurrencyResponseModel> CurrencyToCurrencyResponseModels(List<Currency> currencies);

        public partial CurrencyWithAllRatesResponseModel CurrencyToCurrencyWithAllRatesResponseModel(Currency currency);

        private partial CurrencyRateResponseModel CurrencyRateToCurrencyRateResponseModel(CurrencyRate rate);
        private List<CurrencyRateResponseModel> MapRates(
        IReadOnlyCollection<CurrencyRate> rates)
        {
            return rates
                .OrderByDescending(x => x.ValidFrom)
                .Select(CurrencyRateToCurrencyRateResponseModel)
                .ToList();
        }

        public partial List<CurrencyWithAllRatesResponseModel> CurrencyToCurrencyWithAllRatesResponseModels(List<Currency> currencies);


        //public partial void UpdateProductFromDTO(UpdateProductRequestDTO dto, Product product);
    }
}
