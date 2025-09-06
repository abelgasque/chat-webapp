using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSpaStaticFiles(configuration => {
    configuration.RootPath = "ChatWebApp.Web/ClientApp/dist";
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSpaStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.UseSpa(spa => {
    spa.Options.SourcePath = "ChatWebApp.Web/ClientApp";
    
    if (app.Environment.IsDevelopment())
    {
        spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
    }
});

app.Run();
