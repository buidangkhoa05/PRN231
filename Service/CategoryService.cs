using BusinessObject;
using BusinessObject.Common;
using BusinessObject.Dto;
using Mapster;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class CategoryService : BaseService, ICategoryService
    {
        public CategoryService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<PagingApiResponse<CategoryResponse>> Search(SearchBaseReq req)
        {
            try
            {
                var categories = await _uOW.Resolve<Category>().SearchAsync<CategoryResponse>(req.KeySearch, req.PagingQuery, req.OrderBy);

                return Success(categories);
            }
            catch (Exception ex)
            {
                return PagingFailed<CategoryResponse>(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> CreateCategory(CategoryRequest createReq)
        {
            try
            {
                await _uOW.Resolve<Category>().CreateAsync(createReq.Adapt<Category>());
                await _uOW.SaveChangesAsync();
                return Success(true);
            }
            catch (Exception ex)
            {
                return Failed<bool>(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> UpdateCategory(int categoryID, CategoryRequest createReq)
        {
            try
            {
                var category = await _uOW.Resolve<Category>().FindAsync(categoryID);

                if (category == null)
                    return Failed<bool>("Category not found", HttpStatusCode.NotFound);

                createReq.Adapt(category);

                await _uOW.Resolve<Category>().CreateAsync(category);
                await _uOW.SaveChangesAsync();

                return Success(true);
            }
            catch (Exception ex)
            {
                return Failed<bool>(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> DeleteCategory(int categoryId)
        {
            try
            {
                var category = await _uOW.Resolve<Category>().FindAsync(categoryId);

                if (category == null)
                    return Failed<bool>("Category not found", HttpStatusCode.NotFound);

                await _uOW.Resolve<Category>().DeleteAsync(category);

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
