using HBAPI.Converters;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;

namespace HBAPI.Configuration
{
    public static class ControllerConfiguration
    {
        public static IServiceCollection AddControllersWithOptions(this IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.Converters.Add(new DateOnlyConverter());
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            return services;
        }
    }
}