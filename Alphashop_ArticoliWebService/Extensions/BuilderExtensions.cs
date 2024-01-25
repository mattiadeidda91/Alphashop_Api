using Alphashop_ArticoliWebService.Repository;
using Alphashop_ArticoliWebService.Repository.Interfaces;
using Alphashop_ArticoliWebService.Services;

namespace Alphashop_ArticoliWebService.Extensions
{
    public static class BuilderExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddDbContext<AlphaShopDbContext>();
            services.AddScoped<IArticoliRepository, ArticoliRepository>();
            services.AddScoped<IHttpService, HttpService>();

            //Non serve più con l'uso del Jwt token
            //services.AddScoped<IUserService, UserService>();
        }
    }
}
