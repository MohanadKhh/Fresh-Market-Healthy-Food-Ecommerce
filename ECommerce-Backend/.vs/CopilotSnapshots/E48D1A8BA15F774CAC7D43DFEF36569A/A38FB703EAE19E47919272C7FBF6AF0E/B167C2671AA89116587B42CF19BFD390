using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace ECommerce.DAL
{
    public static class DALServicesExtension
    {
        public static void AddDALServices(this IServiceCollection services, IConfiguration conf)
        {
            //services.AddScoped<IProductRepository, ProductRepository>();
            //services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddDbContext<AppDBContext>(
            options =>
            {
                options.UseSqlServer(conf.GetConnectionString("ECommerce"));
            });
        }
    }
}
