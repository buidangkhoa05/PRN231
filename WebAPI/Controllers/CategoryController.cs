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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Search([FromQuery] SearchBaseReq searchReq)
        {
            var result = await _categoryService.Search(searchReq);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Staff can create new category
        /// </summary>
        /// <param name="createReq"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Create([FromBody] CategoryRequest createReq)
        {
            var result = await _categoryService.CreateCategory(createReq);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Staff can update category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="createReq"></param>
        /// <returns></returns>
        [HttpPut("{categoryId}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Update(int categoryId, [FromBody] CategoryRequest createReq)
        {
            var result = await _categoryService.UpdateCategory(categoryId, createReq);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Staff can delete category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpDelete("{categoryId}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Delete(int categoryId)
        {
            var result = await _categoryService.DeleteCategory(categoryId);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
