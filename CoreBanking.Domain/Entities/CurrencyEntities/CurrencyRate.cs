using CoreBanking.Domain.Common;

namespace CoreBanking.Domain.Entities.CurrencyEntities
{
    public class CurrencyRate : BaseEntity
    {
        public Currency Currency { get; set; }
        public decimal Rate { get; set; }
        public DateTimeOffset ValidFrom { get; set; }
        public DateTimeOffset? ValidTo { get; set; }
        public int CurrencyId { get; set; }
    }
}
