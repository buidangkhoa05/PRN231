using BusinessObject.Common.PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Common
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        #region Query
        Task<TEntity?> FindAsync(int entityId);
        Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> FindListAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TResult>> GetAllAsync<TResult>() where TResult : class;
        Task<IPagedList<TEntity>> SearchAsync(string keySearch, PagingQuery pagingQuery, string orderBy);
        Task<IPagedList<TResult>> SearchAsync<TResult>(string keySearch, PagingQuery pagingQuery, string orderBy)
            where TResult : class;
        Task<bool> IsExist(params int[] ids);
        #endregion Query

        #region Command 
        Task CreateAsync(params TEntity[] entities);
        Task UpdateAsync(params TEntity[] entities);
        Task DeleteAsync(params TEntity[] ids);
        #endregion Command 
    }
}
