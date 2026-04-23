using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.BLL
{
    public static class BLLServicesExtension
    {
        public static void AddBllServices(this IServiceCollection services)
        {
            services.AddScoped<IProductManager, ProductManager>();
            services.AddScoped<ICategoryManager, CategoryManager>();
            services.AddScoped<IImageManager, ImageManager>();
            services.AddScoped<IAuthManager, AuthManager>();
            services.AddScoped<ICartManager, CartManager>();
            services.AddScoped<IOrderManager, OrderManager>();
            services.AddValidatorsFromAssembly(typeof(BLLServicesExtension).Assembly);
        }
    }
}
