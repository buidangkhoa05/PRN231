using BusinessObject.Common;
using BusinessObject.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        //search order
        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> SearchOrder([FromQuery] SearchBaseReq searchReq)
        {
            var response = await _orderService.SearchOrder(searchReq);

            return StatusCode((int)response.StatusCode, response);
        }
    }
}
