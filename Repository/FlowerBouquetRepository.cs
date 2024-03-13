using BusinessObject;
using BusinessObject.Common.PagedList;
using BusinessObject.Dto;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

        public async Task<IEnumerable<FlowerTopSellingResponse>> GetTopSelling()
        {
            var result = await _dbSet.AsNoTracking()
                .Include(f => f.OrderDetails)
                .Select(f => new FlowerTopSellingResponse
                {
                    FlowerBouquetId = f.FlowerBouquetId,
                    CategoryName = f.Category.CategoryName,
                    FlowerBouquetName = f.FlowerBouquetName,
                    TotalPrice = f.OrderDetails.Sum(od => od.UnitPrice * od.Quantity)
                })
                .OrderByDescending(f => f.TotalPrice)
                .Take(3)
                .ToListAsync();
            return result;
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

        public override async Task<IPagedList<TResult>> SearchAsync<TResult>(string keySearch, PagingQuery pagingQuery, string orderBy)
        {
            return await _dbSet.AsNoTracking()
                .Where(f => string.IsNullOrEmpty(keySearch) || f.FlowerBouquetName.Contains(keySearch))
                .AddOrderByString(orderBy)
                .Include(f => f.Category)
                .Include(f => f.Supplier)
                .SelectWithField<FlowerBouquet, TResult>()
                .ToPagedListAsync(pagingQuery);
        }

        public async Task<IPagedList<TResult>> SearchAsync<TResult>(SearchFlowerRequest req) where TResult : class
        {
            return await _dbSet.AsNoTracking()
                .Where(f => (string.IsNullOrEmpty(req.KeySearch) || f.FlowerBouquetName.Contains(req.KeySearch))
                            && (req.CategoryId == null || req.CategoryId == f.CategoryId)
                )
                .AddOrderByString(req.OrderBy)
                .Include(f => f.Category)
                .Include(f => f.Supplier)
                .SelectWithField<FlowerBouquet, TResult>()
                .ToPagedListAsync(req.PagingQuery);
        }
    }
}
