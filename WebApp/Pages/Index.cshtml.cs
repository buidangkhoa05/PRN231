using BusinessObject.Common;
using BusinessObject.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.ApiHelpers;
using System.Reflection.Metadata;
using WebApp.Common;

namespace WebApp.Pages
{
	public class IndexModel : PageModel
	{
		private readonly IApiHelper _apiHelper;

		[BindProperty]
		public IEnumerable<CategoryResponse> Categories { get; set; } = new List<CategoryResponse>();

		public IndexModel()
		{
		}

		public async Task OnGetAsync()
		{
			Categories = await CallGetCategoryAPI();
		}

		private async Task<IList<CategoryResponse>> CallGetCategoryAPI()
		{
			var url = Constants.ApiHost + Endpoints.GET_Category;
			var param = new Dictionary<string, string>();
			param.Add("PagingQuery.PageNumber", "1");
			param.Add("PagingQuery.PageSize", "50");
			var res = await _apiHelper.GetAsync<IList<CategoryResponse>>(url, param);
			return res.Data;
		}
	}
}
