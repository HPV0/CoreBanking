using CoreBanking.Application.DTOs.Account;
using CoreBanking.Application.DTOs.Currency;
using CoreBanking.Application.DTOs.Transaction;
using CoreBanking.Domain.Entities.AccountEntities;
using CoreBanking.Domain.Entities.CurrencyEntities;
using CoreBanking.Domain.Entities.TransactionEntities;
using Riok.Mapperly.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBanking.Application.Mappers
{
    [Mapper]
    public partial class TransactionMapper
    {
        [MapProperty(nameof(TransactionDetail.AccountFromId), nameof(TransactionDetailResponseModel.SenderId))]
        [MapProperty(nameof(TransactionDetail.AccountToId), nameof(TransactionDetailResponseModel.ReceiverId))]
        public partial TransactionDetailResponseModel TransactionDetailToTransactionDetailResponseModel(TransactionDetail transactionDetail);
        public partial List<TransactionDetailResponseModel> TransactionDetailsToTransactionDetailResponseModels(List<TransactionDetail> transactionDetails);

        //public partial void UpdateProductFromDTO(UpdateProductRequestDTO dto, Product product);
    }
}
