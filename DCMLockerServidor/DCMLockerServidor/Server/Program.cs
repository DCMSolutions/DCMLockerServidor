using DCMLockerServidor.Server.Background;
using DCMLockerServidor.Server.Repositorio.Contrato;
using DCMLockerServidor.Server.Repositorio.Implementacion;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddHostedService<DCMServerController>();

builder.Services.AddSingleton<ServerHub>();
builder.Services.AddSignalR();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IExternalRepositorio, ExternalRepositorio>();
builder.Services.AddScoped<ILockerRepositorio, LockerRepositorio>();

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
