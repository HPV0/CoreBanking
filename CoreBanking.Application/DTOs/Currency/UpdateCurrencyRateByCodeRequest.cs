using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBanking.Application.DTOs.Currency
{
    public class UpdateCurrencyRateByCodeRequest
    {
        public string CurrencyCode { get; set; }
        public decimal Rate { get; set; }
    }
}
