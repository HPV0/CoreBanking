using CoreBanking.Application.DTOs.Transaction;
using CoreBanking.Domain.Constants;
using CoreBanking.Domain.Entities.TransactionEntities;
using Riok.Mapperly.Abstractions;

namespace CoreBanking.Application.Mappers
{
    [Mapper]
    public partial class TransactionMapper
    {

        [MapProperty(
            nameof(TransactionDetail.AccountFromId),
            nameof(TransactionDetailResponseModel.SenderId))]
        [MapProperty(
            nameof(TransactionDetail.AccountToId),
            nameof(TransactionDetailResponseModel.ReceiverId))]
        public partial TransactionDetailResponseModel
            ToTransactionDetailResponseModel(TransactionDetail transactionDetail);

        public partial List<TransactionDetailResponseModel>
            ToTransactionDetailResponseModels(List<TransactionDetail> transactionDetails);

        [MapProperty(
            nameof(Transaction.Details),
            nameof(TransactionResponseModel.Details),
            Use = nameof(ToTransactionDetailResponseModels))]
        public partial TransactionResponseModel
            ToTransactionResponseModel(Transaction transaction);

        public partial List<TransactionResponseModel>
            ToTransactionResponseModels(List<Transaction> transactions);

        private List<TransactionDetailResponseModel> ToTransactionDetailResponseModelsSorted(
            List<TransactionDetail> transactionDetails)
        {
            return ToTransactionDetailResponseModels(transactionDetails)
                .OrderByDescending(detail => detail.ReceiverId == BankAccount.Id)
                .ToList();
        }
    }
}