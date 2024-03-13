using BusinessObject;
using BusinessObject.Common;
using BusinessObject.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.ApiHelpers;
using System.Text.Json;

namespace WebApp.Pages
{
    public class OrderModel : PageModel
    {
        private readonly IApiHelper _apiHelper;

        public OrderModel(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        [BindProperty]
        public IList<CartItem> CartItems { get; set; } = new List<CartItem>();
        [BindProperty]
        public decimal TotalPrice => CartItems.Sum(c => c.UnitPrice * c.Quantity);

        [BindProperty]
        public IEnumerable<OrderResponse> Orders { get; set; } = new List<OrderResponse>();


        public async Task OnGetAsync()
        {
            CartItems = GetCartItems();
            Orders = await GetOrders();
        }

        private IList<CartItem> GetCartItems()
        {
            IList<CartItem>? cartItems;
            var cartString = HttpContext.Session.GetString("Cart");

            if (string.IsNullOrEmpty(cartString))
                return new List<CartItem>();

            cartItems = !string.IsNullOrEmpty(cartString) ? JsonSerializer.Deserialize<List<CartItem>>(cartString)
                                                            : new List<CartItem>();

            return cartItems;
        }

        private async Task<IList<OrderResponse>> GetOrders()
        {
            var url = Constant.ApiHost + $"api/Order/me";
            var param = new Dictionary<string, string>();
            param.Add("PagingQuery.PageNumber", "1");
            param.Add("PagingQuery.PageSize", "50");
            param.Add("OrderBy", "orderDate:desc");

            var res = await _apiHelper.GetAsync<PagingApiResponse<OrderResponse>>(url, param);

            return res.Data.PagingData.Data;
        }
    }
}
