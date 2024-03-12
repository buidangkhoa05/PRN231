using Microsoft.AspNetCore.Authentication.Cookies;
using WebApp.Milddleware;

namespace WebApp
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddRazorPages();
			builder.Services.AddMvc();

			builder.Services.AddSerivce();

			builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(options =>
				{
					options.LoginPath = "/login";
					options.LogoutPath = "/logout";
				});

			builder.Services.AddSession();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Error");
			}
			app.UseStaticFiles();
			app.UseSession();
			//add token to request header.
			app.UseMiddleware<HandleAccessTokenMiddleware>();

			app.UseRouting();

			app.UseAuthorization();
			app.UseAuthentication();

			app.MapRazorPages();

			app.Run();
		}
	}
}
