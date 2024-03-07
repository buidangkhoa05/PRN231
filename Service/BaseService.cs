using BusinessObject.Common;
using BusinessObject.Common.PagedList;
using Repository.Common;
using System.Net;

namespace Service
{
    public class BaseService
    {
        protected readonly IUnitOfWork _uOW;
        public BaseService(IUnitOfWork unitOfWork)
        {
            _uOW = unitOfWork;
        }

        protected ApiResponse<T> Success<T>(T data = default, string? message = null)
        {
            return new ApiResponse<T>
            {
                Data = data,
                StatusCode = HttpStatusCode.OK,
                Message = message
            };
        }

        protected PagingApiResponse<T> Success<T>(IPagedList<T> data = default, string message = null)
        {
            return new PagingApiResponse<T>
            {
                StatusCode = HttpStatusCode.OK,
                Message = message,
                Data = new PagingResponse<T>
                {
                    Data = data,
                    CurrentPage = data.CurrentPage,
                    PageSize = data.PageSize,
                    TotalCount = data.TotalCount,
                    TotalPages = data.TotalPages
                }
            };
        }

        protected ApiResponse<T> Success<T>(string? message = null)
        {
            return new ApiResponse<T>
            {
                StatusCode = HttpStatusCode.OK,
                Message = message
            };
        }

        protected ApiResponse<T> Failed<T>(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            return new ApiResponse<T>
            {
                StatusCode = statusCode,
                Message = message
            };
        }

        protected PagingApiResponse<T> PagingFailed<T>(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            return new PagingApiResponse<T>
            {
                StatusCode = statusCode,
                Message = message
            };
        }
    }
}
