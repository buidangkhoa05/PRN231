namespace Repository.Common
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);


        IGenericRepository<TEntity> Resolve<TEntity>()
           where TEntity : class;

        TEntityRepository Resolve<TEntity, TEntityRepository>()
            where TEntity : class
            where TEntityRepository : IGenericRepository<TEntity>;
    }
}
