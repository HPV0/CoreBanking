using CoreBanking.Application.Interfaces.IRepositories;
using CoreBanking.Domain.Entities;
using CoreBanking.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CoreBanking.Infrastructure.Repositories
{
    public class ClientRepository : BaseRepository<Client>, IClientRepository
    {
        protected override IQueryable<Client> BaseQuery
        => _dbContext.Clients;

        public ClientRepository(ApplicationDbContext db) : base(db) { }

        public async Task<bool> PassportNumberExistsAsync(string code)
        {
            var passportExist = await BaseQuery.AnyAsync(cl => cl.PassportNumber == code);

            return passportExist;
        }
    }
}
