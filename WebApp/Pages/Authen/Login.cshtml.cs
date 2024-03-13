using BusinessObject.Common;
using BusinessObject.Dto;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.ApiHelpers;
using System.Security.Claims;

namespace WebApp.Pages.Authen
{
    public class LoginModel : PageModel
    {
        private readonly IApiHelper _apiHelper;

        public LoginModel(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Password { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var url = Constant.ApiHost + "api/account/login";

            var logRequest = new LoginRequest
            {
                Username = Username,
                Password = Password
            };

            var loginRes = await _apiHelper.PostAsync<ApiResponse<LoginResponse>>(url, logRequest);

            if (loginRes == null)
                return Page();

            var loginData = loginRes.Data.Data;

            HttpContext.Session.SetString("Token", loginData.AccessToken);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, loginData.Role),
                new Claim(ClaimTypes.Name, loginData.Fullname)
            };

            var claimsIdentity = new ClaimsIdentity(claims, "login");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(claimsPrincipal);

            return RedirectToPage("/Index");
        }
    }
}
