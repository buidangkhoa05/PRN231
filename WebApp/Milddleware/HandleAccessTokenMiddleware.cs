using BusinessObject.Common;

namespace WebApp.Milddleware
{
	public class HandleAccessTokenMiddleware : IMiddleware
	{
		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			var token = context.Session.GetString("Token") ?? "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJzdXBwb3J0MkBmdWZsb3dlcmJvdXF1ZXRzeXN0ZW0uY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6InVzZXIiLCJmdWxsbmFtZSI6IlVzZXIiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJVc2VyIiwiYWNjb3VudElEIjoiNyIsImV4cCI6MTcxMDM4NTk5OCwiaXNzIjoiVGVzdC5jb20iLCJhdWQiOiJUZXN0LmNvbSJ9.mOLcyqi9AfCd2zihoZPS9sg8WX0jhJYDR2G19wpQ8Ow";
			if (!string.IsNullOrEmpty(token))
			{
				context.Request.Headers.Add("Authorization", "Bearer " + token);
				Constant.token = token;
			}
			await next(context);
		}
	}
}
