using System.Reflection;
using Microsoft.OpenApi.Models;

namespace ChatWebApp.Api.Configurations
{
    public static class SwaggerConfiguration
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ChatWebApp API",
                    Version = "v1",
                    Description = "API para a aplicação ChatWebApp",
                    Contact = new OpenApiContact
                    {
                        Name = "Abel Gasque",
                        Email = "abelgasque20@gmail.com"
                    }
                });
            });

            return services;
        }
    }
}