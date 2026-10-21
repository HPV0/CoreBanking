using CoreBanking.Domain.Entities.CurrencyEntities;

namespace CoreBanking.Application.Interfaces.IRepositories
{
    public interface ICurrencyRepository : IBaseRepository<Currency>
    {
        public Task<Currency> GetCurrencyWithCurrentRateByIdAsync(Guid Id);
        public Task<Currency> GetCurrencyWithCurrentRateByCodeAsync(string code);
        public Task AddCurrencyRateAsync(CurrencyRate currencyRate);
        public Task<Currency> GetOnlyCurrencyByIdAsync(Guid id);
    }
}
