using BusinessObject.Common;
using BusinessObject.Common.PagedList;
using BusinessObject.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.ApiHelpers;

namespace WebApp.Pages
{
	public class IndexModel : PageModel
	{
		private readonly IApiHelper _apiHelper;

		[BindProperty]
		public IEnumerable<CategoryResponse> Categories { get; set; } = new List<CategoryResponse>();

        [BindProperty]
        public IEnumerable<FlowerBouquetResponse> Flowers { get; set; } = new List<FlowerBouquetResponse>();

        public IndexModel(IApiHelper apiHelper)
		{
			_apiHelper = apiHelper;
		}

		public async Task OnGetAsync(int? selectedCategory = null, string? keySearch = null)
		{
			var categories =  await CallGetCategoryAPI();
			Categories = (categories?.PagingData?.Data ?? new PagedList<CategoryResponse>());

			var flowers = await CallGetFlowerAPI(selectedCategory, keySearch);
			Flowers = (flowers?.PagingData?.Data ?? new PagedList<FlowerBouquetResponse>());

		}

        private async Task<PagingApiResponse<FlowerBouquetResponse>?> CallGetFlowerAPI(
			int? categoryID = null, string? keySearch = null, 
			int pageNumber = 1, int pageSize = 10)
        {
            var url = Constant.ApiHost + Endpoints.GET_SearchFlower;

            var param = new Dictionary<string, string>();
            param.Add("PagingQuery.PageNumber", pageNumber.ToString());
            param.Add("PagingQuery.PageSize", pageSize.ToString());
			param.Add("CategoryId", categoryID?.ToString());
			param.Add("KeySearch", keySearch);

            var res = await _apiHelper.GetAsync<PagingApiResponse<FlowerBouquetResponse>>(url, param);
            return res.Data;
        }

        private async Task<PagingApiResponse<CategoryResponse>?> CallGetCategoryAPI()
		{
			var url = Constant.ApiHost + Endpoints.GET_SearchCategory;

			var param = new Dictionary<string, string>();
			param.Add("PagingQuery.PageNumber", "1");
			param.Add("PagingQuery.PageSize", "50");

			var res = await _apiHelper.GetAsync<PagingApiResponse<CategoryResponse>>(url, param);
			return res.Data;
		}
	}
}
