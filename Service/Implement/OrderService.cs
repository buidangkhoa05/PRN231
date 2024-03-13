﻿using BusinessObject;
using BusinessObject.Common;
using BusinessObject.Dto;
using Mapster;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
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
    public class OrderService : BaseService, IOrderService
    {
        public OrderService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<ApiResponse<bool>> CreateOrder(int createdByID, OrderRequest req)
        {
            try
            {
                var flowerBouquet = (await _uOW.Resolve<FlowerBouquet, IFlowerBouquetRepository>()
                                               .FindListAsync(x => req.OrderDetails.Select(x => x.FlowerBouquetId).Any(y => y == x.FlowerBouquetId)))
                                               .OrderBy(x => x.FlowerBouquetId).ToArray();

                if (flowerBouquet == null || !flowerBouquet.Any() || flowerBouquet.Count() != req.OrderDetails.Count())
                    return Failed<bool>("Flower bouquet not found", System.Net.HttpStatusCode.BadRequest);

                if (CheckQuantity(req.OrderDetails, flowerBouquet) == false)
                    return Failed<bool>("Quantity is not enough", System.Net.HttpStatusCode.BadRequest);

                var orderDetail = req.OrderDetails.Adapt<IEnumerable<OrderDetail>>().ToList();
                orderDetail = orderDetail.Select(x =>
                {
                    var flower = flowerBouquet.FirstOrDefault(y => y.FlowerBouquetId == x.FlowerBouquetId);
                    x.UnitPrice = flower.UnitPrice;
                    return x;
                }).ToList();

                await _uOW.BeginTransactionAsync();
                {
                    var order = new Order
                    {
                        OrderDate = DateTime.Now,
                        Total = TotalPrice(req.OrderDetails, flowerBouquet),
                        OrderDetails = orderDetail,
                        AccountId = createdByID,
                        ShippedDate = null
                    };

                    await _uOW.Resolve<Order, IOrderRepository>().CreateAsync(order);
                    await _uOW.SaveChangesAsync();

                    flowerBouquet = CalculateUnitInStock(req.OrderDetails, flowerBouquet);

                    await _uOW.Resolve<FlowerBouquet, IFlowerBouquetRepository>().UpdateAsync(flowerBouquet);
                    await _uOW.SaveChangesAsync();
                }
                await _uOW.CommitTransactionAsync();

                return Success(true);
            }
            catch (Exception ex)
            {
                return Failed<bool>(ex.Message);
            }
        }

        private bool CheckQuantity(IEnumerable<OrderDetailRequest> orderDetails, FlowerBouquet[] flowers)
        {
            return flowers
                 .Join(orderDetails, x => x.FlowerBouquetId, y => y.FlowerBouquetId,
                    (x, y) => x.UnitsInStock - y.Quantity > 0)
                 .All(x => x == true);
        }

        private decimal TotalPrice(IEnumerable<OrderDetailRequest> orderDetails, FlowerBouquet[] flowers)
        {
            return flowers
                 .Join(orderDetails, x => x.FlowerBouquetId, y => y.FlowerBouquetId,
                    (x, y) => x.UnitPrice * y.Quantity - (decimal)y.Discount)
                 .Sum();
        }

        private FlowerBouquet[] CalculateUnitInStock(IEnumerable<OrderDetailRequest> orderDetails, FlowerBouquet[] flowers)
        {
            return flowers
                  .Join(orderDetails, x => x.FlowerBouquetId, y => y.FlowerBouquetId,
                  (x, y) =>
                  {
                      var flower = x;
                      x.UnitsInStock -= y.Quantity;
                      return flower;
                  }).ToArray();
        }

        public async Task<PagingApiResponse<OrderResponse>> SearchOrderByAccountID(int userID, SearchBaseReq searchReq)
        {
            try
            {
                searchReq.KeySearch = userID.ToString();

                var result = await _uOW.Resolve<Order, IOrderRepository>()
                    .SearchAsync<OrderResponse>(searchReq.KeySearch, searchReq.PagingQuery, searchReq.OrderBy);

                return Success(result);
            }
            catch (Exception ex)
            {
                return PagingFailed<OrderResponse>(ex.Message);
            }
        }

        public async Task<PagingApiResponse<OrderResponse>> SearchOrder(SearchBaseReq searchReq)
        {
            try
            {
                var result = await _uOW.Resolve<Order, IOrderRepository>()
                    .SearchAsync<OrderResponse>(searchReq.KeySearch, searchReq.PagingQuery, searchReq.OrderBy);

                return Success(result);
            }
            catch (Exception ex)
            {
                return PagingFailed<OrderResponse>(ex.Message);
            }
        }
    }

}
