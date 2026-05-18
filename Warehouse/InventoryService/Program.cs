using InventoryService.Application.Mappings;
using InventoryService.Infrastructure.DbRepository;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Reflection;
using Warehouse.Shared.Common;
using Warehouse.Shared.Auth;
using InventoryService.Application.DomainService;

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
builder.Services.AddScoped<ItemDomainService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<CurrentUser>();

builder.Services.AddHttpClient("NotificationService",c => c.BaseAddress = new Uri("https://localhost:7222"));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor", policy =>
    {
        policy.WithOrigins("https://localhost:7061")
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
