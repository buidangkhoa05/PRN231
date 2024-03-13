using BusinessObject;
using BusinessObject.Common;
using BusinessObject.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.ApiHelpers;
using System.Text.Json;

namespace WebApp.Pages
{
    public class CheckoutModel : PageModel
    {
        private readonly IApiHelper _apiHelper;

        [BindProperty]
        public IList<CartItem> CartItems { get; set; } = new List<CartItem>();
        [BindProperty]
        public decimal TotalPrice => CartItems.Sum(c => c.UnitPrice * c.Quantity);

        public CheckoutModel(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public void OnGet()
        {
            CartItems = GetCartItems();
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

        public async Task<IActionResult> OnPostAsync()
        {
            OnGet();

            var cartString = HttpContext.Session.GetString("Cart");
            var cartItems = JsonSerializer.Deserialize<List<CartItem>>(cartString);

            var orderDetailRequest = cartItems.Select(c => new OrderDetailRequest
            {
                FlowerBouquetId = c.FlowerBouquetId,
                Quantity = c.Quantity
            }).ToList();

            var orderRequest = new OrderRequest
            {
                OrderDetails = orderDetailRequest
            };

            var res = await CallCreateOrder(orderRequest);

            if (res != null && res.Data == true)
            {
                HttpContext.Session.Remove("Cart");
                HttpContext.Session.Remove("CartQuantity");
                return RedirectToPage("/Index");
            }
            else
            {
                return Page();
            }
        }

        private async Task<ApiResponse<bool>> CallCreateOrder(OrderRequest createReq)
        {
            var url = Constant.ApiHost + "api/order";

            var res = await _apiHelper.PostAsync<ApiResponse<bool>>(url, createReq);

            return res.Data;
        }
    }
}
