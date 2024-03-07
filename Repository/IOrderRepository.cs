using BusinessObject;
using BusinessObject.Dto;
using Repository.Common;
using System.Runtime.CompilerServices;

namespace Repository
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<decimal> TotalPriceOfOrder(IEnumerable<OrderDetailRequest> orderDetails);
    }
}
