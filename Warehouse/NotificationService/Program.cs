using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using NotificationService.Hubs;
using Scalar.AspNetCore;
using System.Reflection;
using Warehouse.Shared.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<CurrentUser>();

builder.Services.AddSharedJwtAuth(builder.Configuration);
builder.Services.AddTransient<AuthTokenHandler>();

builder.Services.AddHttpClient("AccountService", c => c.BaseAddress = new Uri(builder.Configuration["AccountServiceURL"]!))
    .AddHttpMessageHandler<AuthTokenHandler>();

builder.Services.AddSignalR();
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
                .AllowAnyMethod()
                .AllowCredentials();
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

app.MapHub<ChatHub>("/hubs/chat");

app.Run();
