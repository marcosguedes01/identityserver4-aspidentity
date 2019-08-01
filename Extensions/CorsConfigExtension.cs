using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Grw.Gin.Auth.Extensions
{
    public static class CorsConfigExtension
    {
        public static void AddCorsConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var origins = configuration.GetSection("ApplicationSettings").GetSection("CorsPolicyDefaultOrigins").GetChildren()
                .Select(x => x.Value)
                .ToArray();

            services.AddCors(options =>
            {
                // this defines a CORS policy called "default"
                options.AddPolicy("default", policy =>
                {
                    policy.WithOrigins(origins)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }

        public static void UseCorsConfig(this IApplicationBuilder app)
        {
            app.UseCors("default");
        }
    }
}
