using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ChatWebApp.Infrastructure.Persistence.Contexts;
using ChatWebApp.Domain.Repositories;
using ChatApi.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
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

builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "ChatWebApp.Web/ClientApp/dist";
});

builder.Services.AddScoped(typeof(IRepository<>), typeof(AppRepository<>));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSpaStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.UseSpa(spa =>
{
    spa.Options.SourcePath = "ChatWebApp.Web/ClientApp";

    if (app.Environment.IsDevelopment())
    {
        spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
    }
});

app.Run();
