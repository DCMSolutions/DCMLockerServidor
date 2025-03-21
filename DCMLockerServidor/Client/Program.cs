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
    options.ProviderOptions.Authentication.Authority = "https://login.microsoftonline.com/32256e61-0254-4ede-9054-f4c1f46e3778";
    options.ProviderOptions.LoginMode = "redirect";
    options.ProviderOptions.DefaultAccessTokenScopes.Add("api://1a7c9d14-18ec-448a-aaf1-61a96e81b08b/access_as_user");
});

builder.Services.AddHttpClient("DCMLockerServerAPI", client =>
    client.BaseAddress = new Uri("https://server.dcm.com.ar/api"))
    .AddHttpMessageHandler(sp =>
    {
        var handler = sp.GetRequiredService<AuthorizationMessageHandler>()
            .ConfigureHandler(
                authorizedUrls: new[] { "https://server.dcm.com.ar/api" },
                scopes: new[] { "api://1a7c9d14-18ec-448a-aaf1-61a96e81b08b/access_as_user" }
            );
        return handler;
    });

// Add authorization services
builder.Services.AddAuthorizationCore();

// Additional services
builder.Services.AddScoped<DCMLockerServidor.Client.Cliente.Config>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddSweetAlert2();

await builder.Build().RunAsync();
