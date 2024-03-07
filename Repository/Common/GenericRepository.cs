﻿using BusinessObject.Common.PagedList;
using Microsoft.EntityFrameworkCore;
using Repository.Helper;
using System.Data;
using System.Linq.Expressions;

namespace Repository.Common
{
    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;

        public GenericRepository(DbContext context)
        {
            _dbContext = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task<TEntity?> FindAsync(int entityId)
        {
            return await _dbSet.FindAsync(entityId);
        }

        public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.WhereWithExist(predicate)
                                .FirstOrDefaultAsync()
                                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<TEntity>> FindListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.WhereWithExist(predicate)
                        .ToListAsync()
                        .ConfigureAwait(false);
        }


        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.WhereStringWithExist(string.Empty)
                                .ToListAsync()
                                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<TResult>> GetAllAsync<TResult>()
            where TResult : class
        {
            return await _dbSet.WhereStringWithExist(string.Empty)
                                .SelectWithField<TEntity, TResult>()
                                .ToListAsync()
                                .ConfigureAwait(false);
        }

        public async Task<bool> IsExist(params int[] ids)
        {
            string stringIds = string.Join(",", ids);
            return await _dbSet
                .AsNoTracking()
                .WhereStringWithExist($"e.{EFRepositoryHelpers.GetPrimaryKeyName<TEntity>()} IN ({stringIds})")
                .AnyAsync();
        }

        public abstract Task<IPagedList<TEntity>> SearchAsync(string keySearch, PagingQuery pagingQuery, string orderBy);

        public abstract Task<IPagedList<TResult>> SearchAsync<TResult>(string keySearch, PagingQuery pagingQuery, string orderBy)
            where TResult : class;

        public async Task CreateAsync(params TEntity[] entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public async Task UpdateAsync(params TEntity[] entities)
        {
            _dbSet.UpdateRange(entities);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(params TEntity[] entities)
        {
            _dbSet.RemoveRange(entities);
            await Task.CompletedTask;
        }
    }
}