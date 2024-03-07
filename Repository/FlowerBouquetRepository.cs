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
    public class FlowerBouquetRepository : GenericRepository<FlowerBouquet>, IFlowerBouquetRepository
    {
        public FlowerBouquetRepository(DbContext context) : base(context)
        {
        }

        public override async Task<IPagedList<FlowerBouquet>> SearchAsync(string keySearch, PagingQuery pagingQuery, string orderBy)
        {
            return await _dbSet.AsNoTracking()
                 .Where(f => string.IsNullOrEmpty(keySearch) || f.FlowerBouquetName.Contains(keySearch))
                 .AddOrderByString(orderBy)
                 .Include(f => f.Category)
                 .Include(f => f.Supplier)
                 .ToPagedListAsync(pagingQuery);
        }

        public override Task<IPagedList<TResult>> SearchAsync<TResult>(string keySearch, PagingQuery pagingQuery, string orderBy)
        {
            throw new NotImplementedException();
        }
    }
}
