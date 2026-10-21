using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBanking.Application.DTOs.Transaction
{
    public class TransactionResponseModel
    {
        public Guid Id { get; set; }
        public List<TransactionDetailResponseModel> Details { get; set; }
    }
}
