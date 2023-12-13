using Blazored.Modal;
using CurrieTechnologies.Razor.SweetAlert2;
using DCMLockerServidor.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredModal();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<DCMLockerServidor.Client.Cliente.Config>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddSweetAlert2();

await builder.Build().RunAsync();
