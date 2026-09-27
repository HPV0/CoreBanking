using CoreBanking.Domain.Entities;
using CoreBanking.Domain.Entities.AccountEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBanking.Application.Interfaces.IRepositories
{
    public interface IClientRepository : IBaseRepository<Client>
    {
        //public Task<Client> CreateAsync(Client client);
        //public Task<List<Account>> GetAccountsByClientIdAsync(Guid Id);

        //public Task RemoveByIdAsync(Guid id);

    }
}
