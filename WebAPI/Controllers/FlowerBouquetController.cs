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
    public class FlowerBouquetController : ControllerBase
    {
        private readonly IFlowerBouquetService _flowerBouquetService;
        public FlowerBouquetController(IFlowerBouquetService flowerBouquetService)
        {
            _flowerBouquetService = flowerBouquetService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _flowerBouquetService.GetFlowerBouquetById(id);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] SearchBaseReq req)
        {
            var result = await _flowerBouquetService.SearchFlowerBouquet(req);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Staff can create new flower bouquet
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Create([FromBody] FlowerBouquetRequest req)
        {
            var result = await _flowerBouquetService.CreateFlowerBouquet(req);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Staff can update flower bouquet
        /// </summary>
        /// <param name="id"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Update(int id, [FromBody] FlowerBouquetRequest req)
        {
            var result = await _flowerBouquetService.UpdateFlowerBouquet(id, req);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Staff can delete flower bouquet
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _flowerBouquetService.DeleteFlowerBouquet(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
