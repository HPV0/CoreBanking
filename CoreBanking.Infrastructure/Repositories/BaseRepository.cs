using CoreBanking.Application.Interfaces.IRepositories;
using CoreBanking.Domain.Common;
using CoreBanking.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace CoreBanking.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationDbContext _dbContext;
        protected DbSet<T> _set;
        
        // This is for making default includeing all neccery data for query.
        protected virtual IQueryable<T> BaseQuery { get; }

        public BaseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _set = _dbContext.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(Guid id)
        => await BaseQuery.SingleOrDefaultAsync(e => e.Id == id);

        public virtual async Task<IEnumerable<T>> GetByAllAsync()
        => await BaseQuery.AsNoTracking().ToListAsync();

        public async Task<IEnumerable<T>> GetFilteredAsync(Expression<Func<T, bool>> filter)
        => await BaseQuery.AsNoTracking().Where(filter).ToListAsync();

        public async Task SaveChangesAsync()
        => await _dbContext.SaveChangesAsync();

        public async virtual Task AddAsync(T entity)
        => await _set.AddAsync(entity);

        public virtual void Remove(T entity)
        => _set.Remove(entity);

    }
}
