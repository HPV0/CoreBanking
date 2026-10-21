using CoreBanking.Domain.Entities;

namespace CoreBanking.Application.Interfaces.IRepositories
{
    public interface IClientRepository : IBaseRepository<Client>
    {
        //public Task<Client> CreateAsync(Client client);
        //public Task<List<Account>> GetAccountsByClientIdAsync(Guid Id);

        //public Task RemoveByIdAsync(Guid id);

    }
}
