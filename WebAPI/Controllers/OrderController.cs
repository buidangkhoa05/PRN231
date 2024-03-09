using BusinessObject.Common;
using BusinessObject.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        //create order 
        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequest request)
        {
            var userId = int.Parse(HttpContext.User.FindFirst("accountID").Value);
            var response = await _orderService.CreateOrder(userId, request);

            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// Get all order
        /// </summary>
        /// <param name="searchReq"></param>
        /// <returns></returns>
        //search all order
        [HttpGet]
        [Authorize(Roles = "Staff, Admin")]
        public async Task<IActionResult> SearchOrder([FromQuery] SearchBaseReq searchReq)
        {
            var response = await _orderService.SearchOrder(searchReq);

            return StatusCode((int)response.StatusCode, response);
        }

        /// <summary>
        /// User get user's orders 
        /// </summary>
        /// <param name="searchReq"></param>
        /// <returns></returns>
        //get order by id
        [HttpGet("me")]
        [Authorize (Roles = "User")]
        public async Task<IActionResult> GetOrderById([FromQuery] SearchBaseReq searchReq)
        {
            var userId = int.Parse(HttpContext.User.FindFirst("accountID").Value);
            var response = await _orderService.SearchOrderByAccountID(userId, searchReq);

            return StatusCode((int)response.StatusCode, response);
        }
    }
}
