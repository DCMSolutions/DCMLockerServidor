
using DCMLockerServidor.Server.Background;
using DCMLockerServidor.Server.Context;
using DCMLockerServidor.Server.Repositorio.Contrato;
using DCMLockerServidor.Server.Repositorio.Implementacion;
//using DCMLockerServidor.Server.Repositorio.Implementacion;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container.

builder.Services.AddControllersWithViews().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddRazorPages();
//builder.Services.AddHostedService<DCMServerController>();
builder.Services.AddHostedService<DCMServerController2>();

builder.Services.AddSingleton<ServerHub>();
builder.Services.AddSignalR();
builder.Services.AddHttpClient();


builder.Services.AddDbContext<DcmlockerContext>();
builder.Services.AddScoped<IEmpresaRepositorio, EmpresaRepositorio>();
builder.Services.AddScoped<ITokenRepositorio, TokenRepositorio>();
builder.Services.AddScoped<ILockerRepositorio, LockerRepositorio>();

//builder.Services.AddScoped<IExternalRepositorio, ExternalRepositorio>();
//builder.Services.AddScoped<ILockerRepositorioOld, LockerRepositorioOld>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();

app.MapFallbackToFile("index.html");
app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapControllers();
    endpoints.MapHub<ServerHub>("/serverhub");


});

app.Run();
