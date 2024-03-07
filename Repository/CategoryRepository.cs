using BusinessObject;
using BusinessObject.Common.PagedList;
using Microsoft.EntityFrameworkCore;
using Repository.Common;
using Repository.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class CategoryRepository : GenericRepository<Category>
    {
        public CategoryRepository(DbContext context) : base(context)
        {
        }

        public override Task<IPagedList<Category>> SearchAsync(string keySearch, PagingQuery pagingQuery, string orderBy)
        {
            return _dbSet
                .AsNoTracking()
                .Where(c => string.IsNullOrEmpty(keySearch) || c.CategoryName.Contains(keySearch))
                .AddOrderByString(orderBy)
                .Include(c => c.FlowerBouquets)
                .ToPagedListAsync(pagingQuery);
        }

        public override Task<IPagedList<TResult>> SearchAsync<TResult>(string keySearch, PagingQuery pagingQuery, string orderBy)
        {
            return _dbSet
              .AsNoTracking()
              .Where(c => string.IsNullOrEmpty(keySearch) || c.CategoryName.Contains(keySearch))
              .AddOrderByString(orderBy)
              .Include(c => c.FlowerBouquets)
              .SelectWithField<Category,TResult>()
              .ToPagedListAsync(pagingQuery);
        }
    }
}
