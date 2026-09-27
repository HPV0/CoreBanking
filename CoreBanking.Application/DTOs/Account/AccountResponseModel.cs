using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBanking.Application.DTOs.Account
{
    public class AccountResponseModel
    {
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
    }
}
