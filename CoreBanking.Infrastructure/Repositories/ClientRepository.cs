using CoreBanking.Application.Interfaces.IRepositories;
using CoreBanking.Domain.Entities;
using CoreBanking.Infrastructure.Data.Context;

namespace CoreBanking.Infrastructure.Repositories
{
    public class ClientRepository : BaseRepository<Client>, IClientRepository
    {
        protected override IQueryable<Client> BaseQuery
        => _dbContext.Clients;

        public ClientRepository(ApplicationDbContext db) : base(db) { }
    }
}
