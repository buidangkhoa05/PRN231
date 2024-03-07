using BusinessObject.Common;
using BusinessObject.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface ISupplierService
    {
        Task<PagingApiResponse<SupplierResponse>> Search(SearchBaseReq req);
        Task<ApiResponse<bool>> CreateSupplier(SupplierRequest request);
        Task<ApiResponse<bool>> UpdateSupplier(int supplierID, SupplierRequest request);
        Task<ApiResponse<bool>> DeleteSupplier(int supplierID);
    }
}
