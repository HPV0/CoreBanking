using CoreBanking.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace CoreBanking.Application.Interfaces.IRepositories
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        public Task<TEntity?> GetByIdAsync(Guid id);

        public Task<IEnumerable<TEntity>> GetAllItemsAsync();

        public Task<IEnumerable<TEntity>> GetFilteredAsync(Expression<Func<TEntity, bool>> filter);

        public Task SaveChangesAsync();

        public Task AddAsync(TEntity entity);
        public void Remove(TEntity entity);

    }
}
