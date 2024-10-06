using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace HBAPI.Configuration
{
    public static class AuthenticationConfiguration
    {
        public static IServiceCollection AddAuthenticationWithJwt(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = $"https://cognito-idp.{configuration["AWS:Region"]}.amazonaws.com/{configuration["AWS:UserPoolId"]}";
                    options.Audience = configuration["AWS:UserPoolClientId"];
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = $"https://cognito-idp.{configuration["AWS:Region"]}.amazonaws.com/{configuration["AWS:UserPoolId"]}",
                        ValidAudience = configuration["AWS:UserPoolClientId"]
                    };
                });

            return services;
        }
    }
}