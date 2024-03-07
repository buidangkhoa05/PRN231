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
    public class SupplierRepository : GenericRepository<Supplier>
    {
        public SupplierRepository(DbContext context) : base(context)
        {
        }

        public override Task<IPagedList<Supplier>> SearchAsync(string keySearch, PagingQuery pagingQuery, string orderBy)
        {
            return _dbSet.AsNoTracking()
                        .Where(x => string.IsNullOrEmpty(keySearch) || x.SupplierName.Contains(keySearch))
                        .AddOrderByString(orderBy)
                        .Include(x => x.FlowerBouquets)
                        .ToPagedListAsync(pagingQuery);
        }

        public override Task<IPagedList<TResult>> SearchAsync<TResult>(string keySearch, PagingQuery pagingQuery, string orderBy)
        {
            return _dbSet.AsNoTracking()
                        .Where(x => string.IsNullOrEmpty(keySearch) || x.SupplierName.Contains(keySearch))
                        .AddOrderByString(orderBy)
                        .Include(x => x.FlowerBouquets)
                        .SelectWithField<Supplier, TResult>()
                        .ToPagedListAsync(pagingQuery);
        }
    }
}
