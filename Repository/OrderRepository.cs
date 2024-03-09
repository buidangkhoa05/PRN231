using BusinessObject;
using BusinessObject.Common.PagedList;
using BusinessObject.Dto;
using Microsoft.EntityFrameworkCore;
using Repository.Common;
using Repository.Helper;
using System.Linq.Dynamic.Core;

namespace Repository
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(DbContext context) : base(context)
        {
        }

        public override Task<IPagedList<Order>> SearchAsync(string keySearch, PagingQuery pagingQuery, string orderBy)
        {
            return _dbContext.Set<Order>()
                .Where(x => x.OrderId.ToString().Contains(keySearch))
                .Include(x => x.Account)
                .Include(x => x.OrderDetails)
                .AddOrderByString(orderBy)
                .ToPagedListAsync(pagingQuery);
        }

        public override async Task<IPagedList<TResult>> SearchAsync<TResult>(string keySearch, PagingQuery pagingQuery, string orderBy)
        {
            return await _dbContext.Set<Order>()
               .Where(x => string.IsNullOrEmpty(keySearch) || x.AccountId.ToString().Contains(keySearch))
               .Include(x => x.Account)
               .Include(x => x.OrderDetails)
                    .ThenInclude(x => x.FlowerBouquet)
               .AddOrderByString(orderBy)
               .SelectWithField<Order, TResult>()
               .ToPagedListAsync(pagingQuery);
        }

        public async Task<decimal> TotalPriceOfOrder(IEnumerable<OrderDetailRequest> orderDetails)
        {
            return await _dbContext.Set<FlowerBouquet>()
                .Join(orderDetails, x => x.FlowerBouquetId, y => y.FlowerBouquetId,
                (x, y) => new
                {
                    UnitPrice = x.UnitPrice,
                    Quantity = y.Quantity,
                    Discount = y.Discount
                })
                .SumAsync(x => x.UnitPrice * x.Quantity - (decimal)x.Discount);
        }
    }
}
