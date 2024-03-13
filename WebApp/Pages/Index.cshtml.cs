using BusinessObject;
using BusinessObject.Common;
using BusinessObject.Common.PagedList;
using BusinessObject.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.ApiHelpers;
using System.Text.Json;

namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IApiHelper _apiHelper;

        [BindProperty]
        public int CurrentPage { get; set; }
        [BindProperty]
        public int TotalPages { get; set; }
        [BindProperty]
        public int PageSize { get; set; }
        [BindProperty]
        public int TotalCount { get; set; }
        [BindProperty]
        public bool HasPrevious => CurrentPage > 1;
        [BindProperty]
        public bool HasNext => CurrentPage < TotalPages;

        [BindProperty]
        public IEnumerable<CategoryResponse> Categories { get; set; } = new List<CategoryResponse>();

        [BindProperty]
        public IEnumerable<FlowerBouquetResponse> Flowers { get; set; } = new List<FlowerBouquetResponse>();

        [BindProperty]
        public IEnumerable<FlowerTopSellingResponse> TopSellingList { get; set; } = new List<FlowerTopSellingResponse>();

        [BindProperty]
        public IEnumerable<CartItem> CartItems { get; set; } = new List<CartItem>();
        [BindProperty]
        public decimal TotalPrice => CartItems.Sum(c => c.UnitPrice * c.Quantity);

        public IndexModel(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task OnGetAsync(int? selectedCategory = null, string? keySearch = null, int pageNumer = 1)
        {
            var categories = await CallGetCategoryAPI();
            Categories = (categories?.PagingData?.Data ?? new PagedList<CategoryResponse>());

            var flowers = await CallGetFlowerAPI(selectedCategory, keySearch, pageNumer);
            Flowers = (flowers?.PagingData?.Data ?? new PagedList<FlowerBouquetResponse>());

            TotalPages = flowers?.PagingData?.TotalPages ?? 0;
            CurrentPage = flowers?.PagingData?.CurrentPage ?? 0;
            PageSize = flowers?.PagingData?.PageSize ?? 9;
            TotalCount = flowers?.PagingData?.TotalCount ?? 0;

            var topSellingList = await CallGetTopSellingFlowerAPI();
            TopSellingList = topSellingList?.Data ?? new List<FlowerTopSellingResponse>();

            CartItems = GetCartItems();
        }

        private async Task<PagingApiResponse<FlowerBouquetResponse>?> CallGetFlowerAPI(
            int? categoryID = null, string? keySearch = null,
            int pageNumber = 1, int pageSize = 9)
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

        private async Task<ApiResponse<IEnumerable<FlowerTopSellingResponse>>?> CallGetTopSellingFlowerAPI()
        {
            var url = Constant.ApiHost + "api/flowerBouquet/top-selling";

            var res = await _apiHelper.GetAsync<ApiResponse<IEnumerable<FlowerTopSellingResponse>>>(url, new());
            return res.Data;
        }

        public async Task<IActionResult> OnPostAsync(int flowerID)
        {
            await OnGetAsync();

            IList<CartItem>? cartItems = GetCartItems();

            var flower = await CallGetFlowerByIDAsync(flowerID);

            if (flower != null)
            {
                var cartItem = cartItems.FirstOrDefault(c => c.FlowerBouquetId == flowerID);
                if (cartItem != null)
                {
                    cartItem.Quantity++;
                }
                else
                {
                    cartItems.Add(new CartItem
                    {
                        FlowerBouquetId = flower.FlowerBouquetId,
                        FlowerBouquetName = flower.FlowerBouquetName,
                        Quantity = 1,
                        UnitPrice = flower.UnitPrice
                    });
                }

                CartItems = cartItems;
            }

            HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cartItems));
            HttpContext.Session.SetString("CartQuantity", JsonSerializer.Serialize(cartItems.Count()));

            return RedirectToPage("/Index");
        }

        private IList<CartItem> GetCartItems()
        {
            IList<CartItem>? cartItems;
            var cartString = HttpContext.Session.GetString("Cart");

            if(string.IsNullOrEmpty(cartString))
                return new List<CartItem>();

            cartItems = !string.IsNullOrEmpty(cartString) ? JsonSerializer.Deserialize<List<CartItem>>(cartString)
                                                            : new List<CartItem>();

            return cartItems;
        }

        private async Task<FlowerBouquetResponse> CallGetFlowerByIDAsync(int flowerID)
        {
            var url = Constant.ApiHost + $"api/flowerBouquet/{flowerID}";

            var res = await _apiHelper.GetAsync<ApiResponse<FlowerBouquetResponse>>(url, null);

            return res.Data.Data;
        }
    }
}
