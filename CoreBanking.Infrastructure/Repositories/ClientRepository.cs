using CoreBanking.Application.Exceptions;
using CoreBanking.Application.Interfaces.IRepositories;
using CoreBanking.Domain.Entities;
using CoreBanking.Domain.Entities.AccountEntities;
using CoreBanking.Domain.ValueObjects.Client;
using CoreBanking.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBanking.Infrastructure.Repositories
{
    public class ClientRepository : BaseRepository<Client>, IClientRepository
    {
        protected override IQueryable<Client> BaseQuery
        => _dbContext.Clients;
        public ClientRepository(ApplicationDbContext db) : base(db) { }



    }
}
