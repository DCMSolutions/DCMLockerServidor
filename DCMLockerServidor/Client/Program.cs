using Blazored.Modal;
using CurrieTechnologies.Razor.SweetAlert2;
using DCMLockerServidor.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Add Blazored Modal
builder.Services.AddBlazoredModal();

// Configure HttpClient for public API requests
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Configure MSAL authentication for user management
builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
    options.ProviderOptions.LoginMode = "redirect";
});

// Add authorization services
builder.Services.AddAuthorizationCore();

// Additional services
builder.Services.AddScoped<DCMLockerServidor.Client.Cliente.Config>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddSweetAlert2();

await builder.Build().RunAsync();
