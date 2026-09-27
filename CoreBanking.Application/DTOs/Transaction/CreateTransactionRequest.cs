using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBanking.Application.DTOs.Transaction
{
    public class CreateTransactionRequest
    {
        public Guid SenderId { get;  set; }
        public Guid ReceiverId { get; set; }
        public decimal? SenderExchangeRate { get; set; }
        public decimal? ReceiveExchangeRate { get; set; }
        public decimal Amount { get; set; }
    }
}
