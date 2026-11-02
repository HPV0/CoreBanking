using CoreBanking.Application.DTOs.Currency;
using CoreBanking.Application.Exceptions;
using CoreBanking.Application.Interfaces.IRepositories;
using CoreBanking.Application.Interfaces.IServices;
using CoreBanking.Application.Mappers;
using CoreBanking.Domain.Constants;
using CoreBanking.Domain.Entities.CurrencyEntities;

namespace CoreBanking.Application.Services
{
    public class CurrencyService : ICurrencyService
    {
        ICurrencyRepository _currencyRepository;
        ICurrencyRateRepository _currencyRateRepository;

        private readonly CurrencyMapper _mapper;

        public CurrencyService(ICurrencyRepository repo, CurrencyMapper mapper, ICurrencyRateRepository currencyRateRepository)
        {
            _currencyRepository = repo;
            _mapper = mapper;
            _currencyRateRepository = currencyRateRepository;
        }
        public async Task<CurrencyResponseModel> CreateAsync(CreateCurrencyRequest createClientRequest)
        {
            var currecyCodeExist = await _currencyRepository.CurrencyCodeExist(createClientRequest.CurrencyCode);

            if (currecyCodeExist)
                throw new UnprocessableException("Currency with this code already exist");

            var currency = new Currency { CurrencyCode = createClientRequest.CurrencyCode };

            var currencyRate = new CurrencyRate { Rate = createClientRequest.Rate, ValidFrom = DateTimeOffset.UtcNow, ValidTo = null };

            currency.Rates.Add(currencyRate);

            await _currencyRepository.AddAsync(currency);

            await _currencyRepository.SaveChangesAsync();

            return new CurrencyResponseModel { Id = currency.Id, CurrencyCode = currency.CurrencyCode, Rate = currencyRate.Rate };
        }


        public async Task<CurrencyResponseModel> GetCurrencyByIdAsync(int Id)
        {
            var currencyWithActiveRate = await _currencyRepository.GetCurrencyWithActiveRateByIdAsync(Id)
                ?? throw new EntityNotFoundException(nameof(Currency), Id);

            var currencyRate = FindCurrentRate(currencyWithActiveRate)
                ?? throw new EntityNotFoundException("Active currency rate not exsiting");

            return new CurrencyResponseModel { Id = Id, CurrencyCode = currencyWithActiveRate.CurrencyCode, Rate = currencyRate.Rate };
        }

        public async Task<CurrencyResponseModel> GetCurrencyByCodeAsync(string code)
        {
            if (code == null)
                throw new ArgumentException("Cannot get currency with code null.");

            var currencyWithActiveRate = await _currencyRepository.GetCurrencyWithActiveRateByCodeAsync(code)
                ?? throw new EntityNotFoundException(nameof(Currency), code);

            var currencyRate = FindCurrentRate(currencyWithActiveRate)
                ?? throw new EntityNotFoundException("Active currency rate not exsiting");

            return new CurrencyResponseModel { Id = currencyWithActiveRate.Id, CurrencyCode = currencyWithActiveRate.CurrencyCode, Rate = currencyRate.Rate };
        }

        public async Task<List<CurrencyWithAllRatesResponseModel>> GetAllCurrenciesAsync()
        {
            var currencies = await _currencyRepository.GetAllItemsAsync();

            return _mapper.ToCurrencyWithAllRatesResponseModels(currencies.ToList());
        }

        public async Task<CurrencyResponseModel> UpdateCurrencyRateByIdAsync(UpdateCurrencyRateByIdRequest updateCurrencyRateRequest)
        {
            if (updateCurrencyRateRequest.Id == BaseCurrency.Id)
                throw new UnprocessableException("Cannot update base currency rate");

            var currencyWithRate = await _currencyRepository.GetCurrencyWithActiveRateByIdAsync(updateCurrencyRateRequest.Id)
                ?? throw new EntityNotFoundException(nameof(Currency), updateCurrencyRateRequest.Id);

            var currRate = await UpdateRateAsync(currencyWithRate, updateCurrencyRateRequest.Rate);

            await _currencyRepository.SaveChangesAsync();

            return new CurrencyResponseModel { Id = currencyWithRate.Id, CurrencyCode = currencyWithRate.CurrencyCode, Rate = currRate.Rate };
        }

        public async Task<CurrencyResponseModel> UpdateCurrencyRateByCodeAsync(UpdateCurrencyRateByCodeRequest updateCurrencyRateRequest)
        {
            if (updateCurrencyRateRequest.CurrencyCode == BaseCurrency.CurrencyCode)
                throw new UnprocessableException("Cannot update base currency rate");

            var currencyWithRate = await _currencyRepository.GetCurrencyWithActiveRateByCodeAsync(updateCurrencyRateRequest.CurrencyCode)
                ?? throw new EntityNotFoundException(nameof(Currency), updateCurrencyRateRequest.CurrencyCode);

            var currRate = await UpdateRateAsync(currencyWithRate, updateCurrencyRateRequest.Rate);

            await _currencyRepository.SaveChangesAsync();

            return new CurrencyResponseModel { Id = currencyWithRate.Id, CurrencyCode = currencyWithRate.CurrencyCode, Rate = currRate.Rate };
        }

        public async Task RemoveByIdAsync(int id)
        {
            if (id == BaseCurrency.Id)
                throw new Exception("Cannot remove base currency.");

            var curr = await _currencyRepository.GetOnlyCurrencyByIdAsync(id)
                ?? throw new EntityNotFoundException(nameof(Currency), id);

            _currencyRepository.Remove(curr);

            await _currencyRepository.SaveChangesAsync();
        }


        async Task<CurrencyRate> UpdateRateAsync(Currency currencyWithRate, decimal newRate)
        {
            var newCurrencyRateTime = DateTimeOffset.UtcNow;

            var currencyRate = FindCurrentRate(currencyWithRate)
                ?? throw new EntityNotFoundException("Currency active Rate doesnot exsit");

            currencyRate.ValidTo = newCurrencyRateTime;

            var newCurrencyRate = new CurrencyRate { Rate = newRate, CurrencyId = currencyWithRate.Id, ValidFrom = newCurrencyRateTime, ValidTo = null };

            await _currencyRateRepository.AddAsync(newCurrencyRate);

            return newCurrencyRate;
        }

        public CurrencyRate? FindCurrentRate(Currency currency)
        {
            var currentRate = currency.Rates.FirstOrDefault(cr => cr.ValidTo == null);

            return currentRate;
        }

    }
}
