using CoreBanking.Application.Exceptions;
using CoreBanking.Application.Interfaces.IRepositories;
using CoreBanking.Domain.Entities;
using CoreBanking.Domain.Entities.CurrencyEntities;
using CoreBanking.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBanking.Infrastructure.Repositories
{
    public class CurrencyRepository : BaseRepository<Currency>, ICurrencyRepository
    {
        protected override IQueryable<Currency> BaseQuery
        => _dbContext.Currencies.Include(c => c.Rates);

        protected  IQueryable<Currency> CurrentRateQuery
        => _dbContext.Currencies.Include(c => c.Rates
            .Where(r => r.ValidTo == null));

        public CurrencyRepository(ApplicationDbContext db) : base(db) { }

        public async Task<Currency> GetCurrencyWithCurrentRateByIdAsync(Guid id)
        {
            var currency = await CurrentRateQuery.FirstOrDefaultAsync(c => c.Id == id) ?? throw new EntityNotFoundException(nameof(Currency), id);
            
            return currency;
        }


        public async Task<Currency> GetCurrencyWithCurrentRateByCodeAsync(string code)
        {
            var currency = await CurrentRateQuery.FirstOrDefaultAsync(c => c.CurrencyCode == code) ?? throw new EntityNotFoundException(nameof(Currency), code);

            return currency;
        }

        public async Task<Currency> GetOnlyCurrencyByIdAsync(Guid id)
        {
            var currency = await _dbContext.Currencies.FirstOrDefaultAsync(c => c.Id == id) ?? throw new EntityNotFoundException(nameof(Currency), id);
            
            return currency;
        }
    }
}
