using CoreBanking.Domain.Entities;

namespace CoreBanking.Application.Interfaces.IRepositories
{
    public interface IClientRepository : IBaseRepository<Client>
    {
        public Task<bool> PassportNumberExistsAsync(string code);
        public Task<Client> GetBankClientAsync();
    }
}
