using CoreBanking.Application.Interfaces.IRepositories;
using CoreBanking.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBanking.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private IDbContextTransaction? _currentTransaction;

        public ITransactionRepository Transactions { get; }

        public UnitOfWork(ApplicationDbContext dbContext, ITransactionRepository transactions)
        {
            _dbContext = dbContext;
            Transactions = transactions;
        }

        public void Dispose()
            => _dbContext.Dispose();

        /// <summary>
        /// Saves all changes to tracked entities.
        /// If an explicit transaction has not yet been started, the
        /// save operation itself is executed in a new transaction.
        ///// </summary>
        //public Task SaveChanges()
        //    => _dbContext.SaveChangesAsync();

        public bool HasActiveTransaction
            => _currentTransaction is not null;

        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction is not null)
            {
                throw new InvalidOperationException("A transaction is already in progress.");
            }

            _currentTransaction = await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _dbContext.SaveChangesAsync();

                _currentTransaction?.Commit();
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (_currentTransaction is not null)
                {
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_currentTransaction is null)
            {
                throw new InvalidOperationException("A transaction must be in progress to execute rollback.");
            }

            try
            {
                await _currentTransaction.RollbackAsync();
            }
            finally
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }
    }
}
