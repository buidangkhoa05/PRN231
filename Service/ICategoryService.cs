using BusinessObject.Common;
using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Dto;

namespace Service
{
    public interface ICategoryService
    {
        Task<PagingApiResponse<CategoryResponse>> Search(SearchBaseReq req);
        Task<ApiResponse<bool>> CreateCategory(CategoryRequest createReq);
        Task<ApiResponse<bool>> UpdateCategory(int categoryID, CategoryRequest createReq);
        Task<ApiResponse<bool>> DeleteCategory(int categoryId);
    }
}
