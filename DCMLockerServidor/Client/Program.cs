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

// Configure HttpClient for authenticated requests
builder.Services.AddHttpClient("AuthenticatedAPI", client =>
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Use the configured HttpClient
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("AuthenticatedAPI"));

// Configure MSAL authentication
builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
    options.ProviderOptions.LoginMode = "redirect";
    options.ProviderOptions.DefaultAccessTokenScopes.Add("api://21c24e20-4f90-4a29-8ec5-63bc4823a0d5/access_as_user");
});

// Additional services
builder.Services.AddScoped<DCMLockerServidor.Client.Cliente.Config>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddSweetAlert2();

await builder.Build().RunAsync();
