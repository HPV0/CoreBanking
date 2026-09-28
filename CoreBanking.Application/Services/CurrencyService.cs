using CoreBanking.Application.DTOs.Client.CreateClient;
using CoreBanking.Application.DTOs.Currency;
using CoreBanking.Application.Exceptions;
using CoreBanking.Application.Interfaces.IRepositories;
using CoreBanking.Application.Interfaces.IServices;
using CoreBanking.Application.Mappers;
using CoreBanking.Domain.Constants;
using CoreBanking.Domain.Entities;
using CoreBanking.Domain.Entities.AccountEntities;
using CoreBanking.Domain.Entities.CurrencyEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBanking.Application.Services
{
    public class CurrencyService : ICurrencyService
    {

        ICurrencyRepository _currencyRepository;
        private readonly CurrencyMapper _mapper;
        public CurrencyService(ICurrencyRepository repo, CurrencyMapper mapper)
        {
            _currencyRepository = repo;
            _mapper = mapper;
        }
        public async Task<CurrencyResponseModel> CreateAsync(CreateCurrencyRequest createClientRequest)
        {

            var currency = Currency.Create(createClientRequest.CurrencyCode);
            var currencyRate = CurrencyRate.Create(createClientRequest.Rate, currency, DateTimeOffset.UtcNow, null);
            currency.AddCurrencyRate(currencyRate);
            await _currencyRepository.AddAsync(currency);
            await _currencyRepository.SaveChangesAsync();

            return new CurrencyResponseModel { CurrencyCode = currency.CurrencyCode, Rate = currencyRate.Rate };
        }


        public async Task<CurrencyResponseModel> GetCurrencyByIdAsync(Guid Id)
        {
            var currency = await _currencyRepository.GetCurrencyWithCurrentRateByIdAsync(Id);
            var currencyRate = currency.FindCurrentRate();
            return new CurrencyResponseModel{ CurrencyCode = currency.CurrencyCode, Rate = currencyRate.Rate };
        }    
        

        public async Task<CurrencyResponseModel> UpdateCurrencyRateByIdAsync(UpdateCurrencyRateByIdRequest updateCurrencyRateRequest)
        {
            var currency = await _currencyRepository.GetCurrencyWithCurrentRateByIdAsync(updateCurrencyRateRequest.Id);


            currency.UpdateCurrentRate(updateCurrencyRateRequest.Rate);
            var currRate = currency.FindCurrentRate();

            await _currencyRepository.SaveChangesAsync();

            return new CurrencyResponseModel {CurrencyCode = currency.CurrencyCode, Rate = currRate.Rate };
        }

        public async Task<CurrencyResponseModel> UpdateCurrencyRateByCodeAsync(UpdateCurrencyRateByCodeRequest updateCurrencyRateRequest)
        {
            var currency = await _currencyRepository.GetCurrencyWithCurrentRateByCodeAsync(updateCurrencyRateRequest.CurrencyCode);

            currency.UpdateCurrentRate(updateCurrencyRateRequest.Rate);
            var currRate = currency.FindCurrentRate();

            await _currencyRepository.SaveChangesAsync();

            return new CurrencyResponseModel { CurrencyCode = currency.CurrencyCode, Rate = currRate.Rate };
        }

        public async Task<CurrencyResponseModel> GetRateCurrencyByCodeAsync(string code)
        {
            if (code == null)
                throw new ArgumentException("Cannot get currency with code null.");

            var currency = await _currencyRepository.GetCurrencyWithCurrentRateByCodeAsync(code);
            var currencyRate = currency.FindCurrentRate();
            return new CurrencyResponseModel { CurrencyCode = currency.CurrencyCode, Rate = currencyRate.Rate };
        }

        public async Task RemoveByIdAsync(Guid id)
        {
            if (id == BaseCurrency.Id)
                throw new Exception("Cannot remove base currency.");

            var curr = await _currencyRepository.GetOnlyCurrencyByIdAsync(id) ?? throw new EntityNotFoundException(nameof(Currency), id);
            _currencyRepository.Remove(curr);
            await _currencyRepository.SaveChangesAsync();
        }

        public async Task<List<CurrencyWithAllRatesResponseModel>> GetAllCurrenciesAsync()
        {
            var currencies = await _currencyRepository.GetAllItemsAsync();
            foreach (var cur in currencies)
            {
                Console.WriteLine("Curr");
                foreach (var rate in cur.Rates) {
                    Console.WriteLine(rate);
                }
            }
            return _mapper.CurrencyToCurrencyWithAllRatesResponseModels(currencies.ToList());
        }
    }
}
