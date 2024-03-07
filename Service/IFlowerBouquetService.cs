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
    public interface IFlowerBouquetService
    {
        Task<ApiResponse<FlowerBouquet>> GetFlowerBouquetById(int id);
        Task<PagingApiResponse<FlowerBouquet>> SearchFlowerBouquet(SearchBaseReq req);
        Task<ApiResponse<bool>> CreateFlowerBouquet(FlowerBouquetRequest req);
        Task<ApiResponse<bool>> UpdateFlowerBouquet(int flowerBouquetID, FlowerBouquetRequest req);
        Task<ApiResponse<bool>> DeleteFlowerBouquet(int flowerBouquetID);
    }
}
