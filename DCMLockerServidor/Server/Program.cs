using DCMLockerServidor.Server.Background;
using DCMLockerServidor.Server.Context;
using DCMLockerServidor.Server.Repositorio.Contrato;
using DCMLockerServidor.Server.Repositorio.Implementacion;
using System.Text.Json.Serialization;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddRazorPages();

// Configure Azure AD options
builder.Services.Configure<MicrosoftIdentityOptions>(builder.Configuration.GetSection("AzureAd"));
var clientSecret = Environment.GetEnvironmentVariable("AZURE_AD_CLIENT_SECRET");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"))
    .EnableTokenAcquisitionToCallDownstreamApi()
    .AddInMemoryTokenCaches();

builder.Services.Configure<ConfidentialClientApplicationOptions>(options =>
{
    options.ClientSecret = clientSecret;
});

builder.Services.AddHostedService<DCMServerController>();
builder.Services.AddHostedService<TokenDeleter>();

builder.Services.AddSingleton<ServerHub>();
builder.Services.AddSignalR();
builder.Services.AddHttpClient();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddDbContext<DcmlockerContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("ServerConnection"),
        new MySqlServerVersion(new Version(8, 0, 29))));

builder.Services.AddScoped<IEmpresaRepositorio, EmpresaRepositorio>();
builder.Services.AddScoped<ITokenRepositorio, TokenRepositorio>();
builder.Services.AddScoped<ILockerRepositorio, LockerRepositorio>();
builder.Services.AddScoped<ISizeRepositorio, SizeRepositorio>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseCors(policy =>
{
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

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
