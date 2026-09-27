using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBanking.Application.DTOs.Currency
{
    public class UpdateCurrencyRateByIdRequest
    {
        public Guid Id { get; set; }
        public decimal Rate { get; set; }
    }
}
