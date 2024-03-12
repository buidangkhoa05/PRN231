using Service.ApiHelpers;
using WebApp.Milddleware;

namespace WebApp
{
	public static class RegisterServices
	{
		public static void AddSerivce(this IServiceCollection service)
		{
			service.AddScoped(typeof(HandleAccessTokenMiddleware));

			service.AddScoped<IApiHelper, ApiHelper>();
		}

	}
}
