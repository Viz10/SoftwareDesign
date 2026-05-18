using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Warehouse.Client;
using Warehouse.Client.Auth;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// JWT Storage
builder.Services.AddBlazoredLocalStorage();

// Auth
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthStateProvider>();
builder.Services.AddScoped(sp => (JwtAuthStateProvider)sp.GetRequiredService<AuthenticationStateProvider>());
builder.Services.AddTransient<AuthTokenHandler>();

// API clients
var api = builder.Configuration.GetSection("ApiSettings");
builder.Services.AddHttpClient("AccountService",
    c => c.BaseAddress = new Uri(api["AccountService"]!))
    .AddHttpMessageHandler<AuthTokenHandler>();

builder.Services.AddHttpClient("InventoryService",
    c => c.BaseAddress = new Uri(api["InventoryService"]!))
    .AddHttpMessageHandler<AuthTokenHandler>();

builder.Services.AddHttpClient("OrderService",
    c => c.BaseAddress = new Uri(api["OrderService"]!))
    .AddHttpMessageHandler<AuthTokenHandler>();

await builder.Build().RunAsync();