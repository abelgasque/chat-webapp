using System.Reflection;
using Microsoft.OpenApi.Models;

namespace ChatWebApp.Api.Configurations
{
    public static class SwaggerConfiguration
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Chat Application",
                    Description = "Exemplo de aplicação de chat utilizando ASP.NET Core Web API",
                    Contact = new OpenApiContact
                    {
                        Name = "Abel Gasque",
                        Email = "abelgasque20@gmail.com",
                        Url = new Uri("https://abelgasque.com")
                    },
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Insira um token JWT válido no campo abaixo",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{ }
                    }
                });
            });

            return services;
        }
    }
}