using CoreBanking.Domain.Entities.CurrencyEntities;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CoreBanking.Application.Interfaces.IRepositories
{
    public interface ICurrencyRepository : IBaseRepository<Currency>
    {
        public Task<Currency> GetCurrencyWithCurrentRateByIdAsync(Guid Id);
        public Task<Currency> GetCurrencyWithCurrentRateByCodeAsync(string code);
        public Task<Currency> GetOnlyCurrencyByIdAsync(Guid id);
    }
}
