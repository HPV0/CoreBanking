using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBanking.Application.DTOs.Currency
{
    public class CurrencyWithAllRatesResponseModel
    {
        public string CurrencyCode { get; set; }
        public List<CurrencyRateResponseModel> Rates { get; set; }
    }
}
