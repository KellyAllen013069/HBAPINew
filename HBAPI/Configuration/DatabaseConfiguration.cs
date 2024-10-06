using HBAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace HBAPI.Configuration
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<HbDbContext>(options =>
                options.UseMySql(
                    configuration.GetConnectionString("DefaultConnection"),
                    new MySqlServerVersion(new Version(8, 0, 21))
                )
            );

            return services;
        }
    }
}