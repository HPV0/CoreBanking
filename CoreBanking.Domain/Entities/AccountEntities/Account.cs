using CoreBanking.Domain.Common;
using CoreBanking.Domain.Entities.CurrencyEntities;

namespace CoreBanking.Domain.Entities.AccountEntities
{
    public class Account : BaseEntity
    {
        public Client Client { get; set; }
        public Currency Currency { get; set; }
        public AccountBalance Balance { get; set; }
        public int ClientId { get; set; }
        public int CurrencyId { get; set; }
        public bool IsBankAccount { get; set; }
    }
}
