using BusinessObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Reflection;

namespace Repository.Common;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _dbContext;
    private readonly Dictionary<string, object> _repositoryDictionary;
    private IDbContextTransaction? transaction;

    public UnitOfWork()
    {
        _dbContext = new FuflowerBouquetManagementV4Context();
        _repositoryDictionary = new Dictionary<string, object>();
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (transaction != null)
            throw new InvalidOperationException("Transaction has already been started.");

        transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (transaction == null)
                throw new InvalidOperationException("Transaction has not been started.");

            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (transaction == null)
            throw new InvalidOperationException("Transaction has not been started.");

        await transaction.RollbackAsync(cancellationToken);
    }

    public IGenericRepository<TEntity> Resolve<TEntity>()
        where TEntity : class
    {
        var respository = _repositoryDictionary.GetValueOrDefault(typeof(TEntity).Name);
        if (respository == null)
        {
            var classType = GetClassImplementingInterface(typeof(IGenericRepository<TEntity>));
            respository = Activator.CreateInstance(classType, _dbContext);

            ArgumentNullException.ThrowIfNull(respository, $"Cannot create instance of repository {classType.Name}");

            _repositoryDictionary.Add(typeof(TEntity).Name, respository);
        }
        return (IGenericRepository<TEntity>)respository;
    }

    public TEntityRepository Resolve<TEntity, TEntityRepository>()
        where TEntity : class
        where TEntityRepository : IGenericRepository<TEntity>
    {
        var respository = _repositoryDictionary.GetValueOrDefault(typeof(TEntity).Name);

        if (respository == null)
        {
            if (typeof(TEntityRepository).IsInterface)
            {
                var classRepository = GetClassImplementingInterface(typeof(TEntityRepository));
                respository = Activator.CreateInstance(classRepository, _dbContext);
            }
            else
            {
                respository = Activator.CreateInstance(typeof(TEntityRepository), _dbContext);
            }

            ArgumentNullException.ThrowIfNull(respository, $"Cannot create instance of repository {typeof(TEntityRepository).Name}");

            _repositoryDictionary.Add(typeof(TEntity).Name, respository);
        }

        return (TEntityRepository)respository;
    }

    private Type GetClassImplementingInterface(Type interfaceType)
    {
        var genericType = interfaceType.GenericTypeArguments.FirstOrDefault();

        Type? type = null;
        if (genericType != null)
        {
            type = Assembly.GetExecutingAssembly().GetTypes()
                .FirstOrDefault(t => t.IsClass == true && t.IsAbstract == false
                                                       && (t.GetInterface(interfaceType.Name)
                                                           ?.GetGenericArguments()
                                                           ?.Any(a => a.Name == genericType.Name) ?? false));
        }
        else
        {
            type = Assembly.GetExecutingAssembly().GetTypes()
                .FirstOrDefault(t => t.IsClass == true && t.IsAbstract == false
                                                       && t.GetInterface(interfaceType.Name) != null);
        }


        ArgumentNullException.ThrowIfNull(type, $"Cannot find class implementing interface {interfaceType.Name}");
        return type;
    }

    #region Destructor
    private bool isDisposed = false;
    protected virtual void Dispose(bool disposing)
    {
        if (isDisposed)
            return;

        if (disposing)
        {
            _dbContext.Dispose();
        }

        isDisposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~UnitOfWork()
    {
        Dispose(false);
    }
    #endregion Destructor
}

