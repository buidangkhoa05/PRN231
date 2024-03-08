using BusinessObject;
using BusinessObject.Common;
using BusinessObject.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IOrderService
    {
        Task<ApiResponse<bool>> CreateOrder(int createdByID, OrderRequest req);
        Task<PagingApiResponse<Order>> SearchOrder(SearchBaseReq searchReq);
    }
}
