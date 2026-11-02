using CoreBanking.Domain.Common;
using CoreBanking.Domain.Entities.AccountEntities;

namespace CoreBanking.Domain.Entities.TransactionEntities
{
    public class Transaction : BaseEntity
    {
        public int? TransactionId { get; set; }
        public Account AccountFrom { get; set; }
        public Account AccountTo { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal Amount { get; set; }
        public int AccountFromId { get; set; }
        public int AccountToId { get; set; }
    }

}
