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
    [Authorize(Roles = "Admin, Staff")]
    public class FlowerBouquetController : ControllerBase
    {
        private readonly IFlowerBouquetService _flowerBouquetService;
        public FlowerBouquetController(IFlowerBouquetService flowerBouquetService)
        {
            _flowerBouquetService = flowerBouquetService;
        }

        //get by id 
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _flowerBouquetService.GetFlowerBouquetById(id);
            return StatusCode((int)result.StatusCode, result);
        }

        //search 
        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] SearchBaseReq req)
        {
            var result = await _flowerBouquetService.SearchFlowerBouquet(req);
            return StatusCode((int)result.StatusCode, result);
        }

        //create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FlowerBouquetRequest req)
        {
            var result = await _flowerBouquetService.CreateFlowerBouquet(req);
            return StatusCode((int)result.StatusCode, result);
        }

        //update
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] FlowerBouquetRequest req)
        {
            var result = await _flowerBouquetService.UpdateFlowerBouquet(id, req);
            return StatusCode((int)result.StatusCode, result);
        }

        //delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _flowerBouquetService.DeleteFlowerBouquet(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
