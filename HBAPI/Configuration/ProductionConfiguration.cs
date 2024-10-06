using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace HBAPI.Configuration
{
    public static class ProductionConfiguration
    {
        public static IApplicationBuilder UseProductionErrorHandling(this IApplicationBuilder app)
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts(); // Enforce HTTPS in production

            return app;
        }
    }
}