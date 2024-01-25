using Alphashop_PriceWebService.Repository;
using Alphashop_PriceWebService.Repository.Interfaces;

namespace Alphashop_PriceWebService.Extensions
{
    public static class ServicesExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddDbContext<AlphaShopDbContext>();
            services.AddScoped<IPriceService, PriceService>();
            services.AddScoped<IListinoService, ListinoService>();
        }
    }
}
