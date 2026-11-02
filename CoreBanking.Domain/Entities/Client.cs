using CoreBanking.Domain.Common;
using CoreBanking.Domain.Entities.AccountEntities;

namespace CoreBanking.Domain.Entities
{
    public class Client : BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public DateOnly? Birthday { get; set; }
        public string PassportNumber { get; set; }
        public List<Account>? Accounts { get; set; }
    }
}
