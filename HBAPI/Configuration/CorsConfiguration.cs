using Microsoft.Extensions.DependencyInjection;

namespace HBAPI.Configuration
{
    public static class CorsConfiguration
    {
        public static IServiceCollection AddCustomCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    policy => policy.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });

            return services;
        }
    }
}