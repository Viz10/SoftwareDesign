using FluentValidation;
using InventoryService.Application.DomainService;
using InventoryService.Application.Mappings;
using InventoryService.Infrastructure.DbRepository;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Reflection;
using Warehouse.Shared.Auth;
using Warehouse.Shared.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

builder.Services.AddDbContext<InventoryServiceDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), includeInternalTypes: true);
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(typeof(ItemMappingProfile).Assembly);
    cfg.AddMaps(typeof(StockUnitMappingProfile).Assembly);
});

builder.Services.AddSharedJwtAuth(builder.Configuration);
builder.Services.AddTransient<AuthTokenHandler>();

builder.Services.AddScoped<ItemDomainService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<CurrentUser>();

builder.Services.AddHttpClient("NotificationService",c => c.BaseAddress = new Uri(builder.Configuration["NotificationServiceURL"]!))
    .AddHttpMessageHandler<AuthTokenHandler>();

builder.Services.AddControllers(options =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor", policy =>
    {
        policy.WithOrigins(builder.Configuration["BlazorURL"]!)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseCors("AllowBlazor");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
