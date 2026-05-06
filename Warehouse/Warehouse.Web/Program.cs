using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warehouse.Data.Data.DTOs.AccountDTOs;
using Warehouse.Data.DbRepository;
using Warehouse.Services;
using Warehouse.Services.Services.Events;
using Warehouse.Services.Services.Exports;
using Warehouse.Services.Services.Exports.ConcreteStrategies;
using Warehouse.Web.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddDbContextFactory<WarehouseDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(cfg => {
    cfg.AddMaps(typeof(Program).Assembly);
    cfg.AddMaps(typeof(ItemService).Assembly);
});

builder.Services.AddTransient<IExportStrategy, ExportCSV>();
builder.Services.AddTransient<IExportStrategy, ExportJSON>();
builder.Services.AddTransient<IExportStrategy, ExportXML>();
builder.Services.AddTransient<Export>();

builder.Services.AddSingleton<WarehouseEventBus>();
builder.Services.AddSingleton<NotificationService>();

builder.Services.AddTransient<ItemService>();
builder.Services.AddTransient<StockUnitService>();
builder.Services.AddTransient<IdentificationService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAuthentication();   
app.UseAuthorization();   
app.UseAntiforgery();      

app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.MapPost("/auth/login", async ([FromForm] LoginDTO dto, [FromServices] IdentificationService service) =>
{
    var error = await service.login(dto);

    if (error != null)
    {
        return Results.Redirect($"/Login?error={Uri.EscapeDataString(error)}");
    }
    return Results.Redirect("/Items");
});
app.MapPost("/auth/logout", async ([FromServices] IdentificationService service) =>
{
    await service.logout();
    return Results.Redirect("/Login");
}).DisableAntiforgery();

app.Services.GetService<NotificationService>(); /// call manually

app.Run();