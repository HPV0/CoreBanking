using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBanking.Application.DTOs.Transaction
{
    public class TransactionDetailResponseModel
    {
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal Amount { get; set; }
    }
}
