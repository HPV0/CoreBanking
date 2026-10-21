using CoreBanking.Application.DTOs.Currency;

namespace CoreBanking.Application.Interfaces.IServices
{
    public interface ICurrencyService
    {
        public Task<CurrencyResponseModel> CreateAsync(CreateCurrencyRequest createClientRequest);
        public Task<CurrencyResponseModel> UpdateCurrencyRateByIdAsync(UpdateCurrencyRateByIdRequest updateCurrencyRateRequest);
        public Task<CurrencyResponseModel> UpdateCurrencyRateByCodeAsync(UpdateCurrencyRateByCodeRequest updateCurrencyRateRequest);
        public Task<CurrencyResponseModel> GetCurrencyByIdAsync(Guid Id);
        public Task<List<CurrencyWithAllRatesResponseModel>> GetAllCurrenciesAsync();
        public Task RemoveByIdAsync(Guid Id);
        public Task<CurrencyResponseModel> GetRateCurrencyByCodeAsync(string code);

    }
}
