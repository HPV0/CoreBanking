using CoreBanking.Domain.Entities.CurrencyEntities;

namespace CoreBanking.Application.Interfaces.IRepositories
{
    public interface ICurrencyRepository : IBaseRepository<Currency>
    {
        public Task<Currency?> GetCurrencyWithActiveRateByIdAsync(int Id);
        public Task<Currency?> GetCurrencyWithActiveRateByCodeAsync(string code);
        public Task<Currency> GetOnlyCurrencyByIdAsync(int id);
        public Task<Currency> GetOnlyCurrencyByCodeAsync(string code);
        public Task<CurrencyRate?> GetActiveCurrencyRateByCodeAsync(string code);
        public Task<CurrencyRate?> GetActiveCurrencyRateByIdAsync(int Id);
        public Task<bool> CurrencyCodeExist(string code);
    }
}
