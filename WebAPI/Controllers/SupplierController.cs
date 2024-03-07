using BusinessObject.Common;
using BusinessObject.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;
        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] SearchBaseReq req)
        {
            var result = await _supplierService.Search(req);
            return StatusCode((int) result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SupplierRequest req)
        {
            var result = await _supplierService.CreateSupplier(req);
            return StatusCode((int) result.StatusCode, result);
        }

        [HttpPut("{supplierID}")]
        public async Task<IActionResult> Update(int supplierID, [FromBody] SupplierRequest req)
        {
            var result = await _supplierService.UpdateSupplier(supplierID, req);
            return StatusCode((int) result.StatusCode, result);
        }

        [HttpDelete("{supplierID}")]
        public async Task<IActionResult> Delete(int supplierID)
        {
            var result = await _supplierService.DeleteSupplier(supplierID);
            return StatusCode((int) result.StatusCode, result);
        }
    }
}
