﻿using BusinessObject;
using BusinessObject.Common;
using BusinessObject.Dto;
using Mapster;
using Repository;
using Repository.Common;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implement
{
    public class FlowerBouquetService : BaseService, IFlowerBouquetService
    {
        public FlowerBouquetService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<ApiResponse<FlowerBouquetResponse>> GetFlowerBouquetById(int id)
        {
            try
            {
                var flowerBouquet = await _uOW.Resolve<FlowerBouquet>().FindAsync(f => f.FlowerBouquetId == id);
                if (flowerBouquet == null)
                {
                    return Failed<FlowerBouquetResponse>("Flower bouquet not found", System.Net.HttpStatusCode.NotFound);
                }
                return Success(flowerBouquet.Adapt<FlowerBouquetResponse>());
            }
            catch (Exception ex)
            {
                return Failed<FlowerBouquetResponse>(ex.Message);
            }
        }

        public async Task<PagingApiResponse<FlowerBouquetResponse>> SearchFlowerBouquet(SearchBaseReq req)
        {
            try
            {
                var result = await _uOW.Resolve<FlowerBouquet, IFlowerBouquetRepository>()
                                .SearchAsync<FlowerBouquetResponse>(req.KeySearch, req.PagingQuery, req.OrderBy);

                return Success(result);
            }
            catch (Exception ex)
            {
                return PagingFailed<FlowerBouquetResponse>(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> CreateFlowerBouquet(FlowerBouquetRequest req)
        {
            try
            {
                if (await _uOW.Resolve<Category>().IsExist(req.CategoryId) == false)
                    return Failed<bool>("Category not found", System.Net.HttpStatusCode.NotFound);

                if (await _uOW.Resolve<Supplier>().IsExist(req.SupplierId ?? -1) == false)
                    return Failed<bool>("SupplierId not found", System.Net.HttpStatusCode.NotFound);

                var flowerBouquet = req.Adapt<FlowerBouquet>();

                await _uOW.Resolve<FlowerBouquet>().CreateAsync(flowerBouquet);
                await _uOW.SaveChangesAsync();

                return Success(true);
            }
            catch (Exception ex)
            {
                return Failed<bool>(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> UpdateFlowerBouquet(int flowerBouquetID, FlowerBouquetRequest req)
        {
            try
            {
                if (await _uOW.Resolve<Category>().IsExist(req.CategoryId) == false)
                    return Failed<bool>("Category not found", System.Net.HttpStatusCode.NotFound);

                if (await _uOW.Resolve<Supplier>().IsExist(req.SupplierId ?? -1) == false)
                    return Failed<bool>("SupplierId not found", System.Net.HttpStatusCode.NotFound);

                var flowerBouquet = await _uOW.Resolve<FlowerBouquet>().FindAsync(flowerBouquetID);

                if (flowerBouquet == null)
                    return Failed<bool>("Flower bouquet not found", System.Net.HttpStatusCode.NotFound);

                req.Adapt(flowerBouquet);

                await _uOW.Resolve<FlowerBouquet>().UpdateAsync(flowerBouquet);
                await _uOW.SaveChangesAsync();

                return Success(true);
            }
            catch (Exception ex)
            {
                return Failed<bool>(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> DeleteFlowerBouquet(int flowerBouquetID)
        {
            try
            {
                var flowerBouquet = await _uOW.Resolve<FlowerBouquet>().FindAsync(flowerBouquetID);

                if (flowerBouquet == null)
                    return Failed<bool>("Flower bouquet not found", System.Net.HttpStatusCode.NotFound);

                await _uOW.Resolve<FlowerBouquet>().DeleteAsync(flowerBouquet);
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
