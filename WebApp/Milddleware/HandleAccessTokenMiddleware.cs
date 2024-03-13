using BusinessObject.Common;

namespace WebApp.Milddleware
{
	public class HandleAccessTokenMiddleware : IMiddleware
	{
		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			var token = context.Session.GetString("Token");
			if (!string.IsNullOrEmpty(token))
			{
				context.Request.Headers.Add("Authorization", "Bearer " + token);
				Constant.token = token;
			}
			else if (!context.Request.Path.Equals("/authen/login", StringComparison.OrdinalIgnoreCase))

            {
				context.Response.Redirect("/authen/login");
				return;
			}
			
			await next(context);
		}
	}
}
