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
    [Authorize(Roles = "Staff")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] SearchBaseReq searchReq)
        {
            var result = await _categoryService.Search(searchReq);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryRequest createReq)
        {
            var result = await _categoryService.CreateCategory(createReq);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("{categoryId}")]
        public async Task<IActionResult> Update(int categoryId, [FromBody] CategoryRequest createReq)
        {
            var result = await _categoryService.UpdateCategory(categoryId, createReq);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> Delete(int categoryId)
        {
            var result = await _categoryService.DeleteCategory(categoryId);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
