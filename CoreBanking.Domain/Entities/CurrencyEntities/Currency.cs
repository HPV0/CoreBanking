using CoreBanking.Domain.Common;

namespace CoreBanking.Domain.Entities.CurrencyEntities
{
    public class Currency : BaseEntity
    {
        public string CurrencyCode { get; set; }
        public bool IsBaseCurrency{ get; set; }
        public List<CurrencyRate> Rates { get; set; } = new();
    }
}

