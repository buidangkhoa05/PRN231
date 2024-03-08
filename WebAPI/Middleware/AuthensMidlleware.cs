using Service;
using System.Security.Claims;

namespace WebAPI.Middleware
{
    public class AuthensMidlleware : IMiddleware
    {

        private readonly IAccountService _accountService;

        public AuthensMidlleware(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer", "").Trim();
            if (!string.IsNullOrEmpty(token))
            {
                var claims = await _accountService.GetAuthenticatedAccount(token);
                context.User = new ClaimsPrincipal(new ClaimsIdentity(claims, "Bearer"));
            }
            await next(context);
        }
    }
}
