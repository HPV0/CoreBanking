using CoreBanking.Application.Interfaces.IRepositories;
using CoreBanking.Domain.Entities.CurrencyEntities;
using CoreBanking.Infrastructure.Data.Context;

namespace CoreBanking.Infrastructure.Repositories
{
    public class CurrencyRateRepository : BaseRepository<CurrencyRate>, ICurrencyRateRepository
    {
        protected override IQueryable<CurrencyRate> BaseQuery
        => _dbContext.CurrencyRates;
        public CurrencyRateRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
