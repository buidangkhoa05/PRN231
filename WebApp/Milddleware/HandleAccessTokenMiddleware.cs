
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
			}
			await next(context);
		}
	}
}
