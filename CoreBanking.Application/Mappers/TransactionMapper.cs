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
            nameof(Transaction.AccountFromId),
            nameof(TransactionDetailResponseModel.SenderId))]
        [MapProperty(
            nameof(Transaction.AccountToId),
            nameof(TransactionDetailResponseModel.ReceiverId))]
        public partial TransactionDetailResponseModel
            ToTransactionDetailResponseModel(Transaction transactionDetail);

        public partial List<TransactionDetailResponseModel>
            ToTransactionDetailResponseModels(List<Transaction> transactionDetails);

        public TransactionResponseModel ToTransactionResponseModel(
            List<Transaction> transactions)
        {
            if (transactions == null || transactions.Count == 0)
            {
                return new TransactionResponseModel
                {
                    Id = null,
                    Details = []
                };
            }

            return new TransactionResponseModel
            {
                Id = transactions[0].TransactionId,
                Details = ToTransactionDetailResponseModels(transactions)
                    .OrderByDescending(detail => detail.ReceiverId == BankAccount.Id)
                    .ToList()
            };
        }

        public List<TransactionResponseModel> ToTransactionResponseModels(
            List<List<Transaction>> transactions)
        {
            return transactions
                .Select(ToTransactionResponseModel)
                .ToList();
        }
    }
}