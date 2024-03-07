using Repository.Common;
using Service;
using System.Runtime.CompilerServices;
using WebAPI.Middleware;

namespace WebAPI.Configurations
{
    public static class RegisterServices
    {

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(AuthensMidlleware));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IFlowerBouquetService, FlowerBouquetService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IOrderService, OrderService>();

            services.AddFluentValidation();
        }
    }
}
