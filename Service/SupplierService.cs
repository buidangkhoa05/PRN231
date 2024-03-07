using BusinessObject;
using BusinessObject.Common;
using BusinessObject.Dto;
using Mapster;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class SupplierService : BaseService, ISupplierService
    {
        public SupplierService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<PagingApiResponse<SupplierResponse>> Search(SearchBaseReq req)
        {
            try
            {
                var suppliers = await _uOW.Resolve<Supplier>()
                    .SearchAsync<SupplierResponse>(req.KeySearch, req.PagingQuery, req.OrderBy);

                return Success(suppliers);
            }
            catch (Exception ex)
            {
                return PagingFailed<SupplierResponse>(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> CreateSupplier(SupplierRequest request)
        {
            try
            {
                await _uOW.Resolve<Supplier>().CreateAsync(request.Adapt<Supplier>());
                await _uOW.SaveChangesAsync();

                return Success(true);
            }
            catch (Exception ex)
            {
                return Failed<bool>(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> UpdateSupplier(int supplierID, SupplierRequest request)
        {
            try
            {
                var supplier = await _uOW.Resolve<Supplier>().FindAsync(supplierID);

                if(supplier == null)
                    return Failed<bool>("Supplier not found");

                request.Adapt(supplier);

                await _uOW.Resolve<Supplier>().UpdateAsync(supplier);
                await _uOW.SaveChangesAsync();

                return Success(true);
            }
            catch (Exception ex)
            {
                return Failed<bool>(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> DeleteSupplier(int supplierID)
        {
            try
            {
                var supplier = await _uOW.Resolve<Supplier>().FindAsync(supplierID);

                if (supplier == null)
                    return Failed<bool>("Supplier not found");

                await _uOW.Resolve<Supplier>().DeleteAsync(supplier);
                await _uOW.SaveChangesAsync();

                return Success(true);
            }
            catch (Exception ex)
            {
                return Failed<bool>(ex.Message);
            }
        }
    }
}
