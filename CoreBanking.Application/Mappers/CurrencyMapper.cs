using CoreBanking.Application.DTOs.Client.CreateClient;
using CoreBanking.Application.DTOs.Currency;
using CoreBanking.Domain.Entities;
using CoreBanking.Domain.Entities.CurrencyEntities;
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

        //public partial void UpdateProductFromDTO(UpdateProductRequestDTO dto, Product product);
    }
}
