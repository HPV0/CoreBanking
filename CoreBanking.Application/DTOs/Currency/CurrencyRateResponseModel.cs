using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBanking.Application.DTOs.Currency
{
    public class CurrencyRateResponseModel
    {
        public DateTimeOffset ValidFrom { get; set; }
        public DateTimeOffset? ValidTo { get; set; }
        public decimal Rate { get; set; }
    }
}
