using Microsoft.EntityFrameworkCore;
using ChatWebApp.Api.Configurations;
using ChatWebApp.Application.Commands;
using ChatWebApp.Domain.Repositories;
using ChatWebApp.Domain.Services;
using ChatWebApp.Infrastructure.Persistence.Contexts;
using ChatWebApp.Infrastructure.Repositories;
using ChatWebApp.Infrastructure.Services;

namespace ChatWebApp.Api
{
    public class Startup
    {
        public IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(LoginCommand).Assembly);
            });

            services.AddControllers();
            services.AddEndpointsApiExplorer();

            services.AddAuthConfiguration(_configuration);
            services.AddSwaggerConfiguration();

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "../ChatWebApp.Web/ClientSide/dist/chat-webapp/browser";
            });

            services.AddScoped(typeof(IRepository<>), typeof(AppRepository<>));
            services.AddScoped<IAuthService, AuthService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpaStaticFiles();
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "../ChatWebApp.Web/ClientSide";
            });
        }
    }
}
