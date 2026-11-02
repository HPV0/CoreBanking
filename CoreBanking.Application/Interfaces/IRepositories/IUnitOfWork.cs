namespace CoreBanking.Application.Interfaces.IRepositories
{
    public interface IUnitOfWork : IDisposable
    {
        public ITransactionRepository Transactions { get; }
        bool HasActiveTransaction { get; }
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
