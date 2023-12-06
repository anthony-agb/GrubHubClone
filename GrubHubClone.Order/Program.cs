using GrubHubClone.Common.Authentication;
using GrubHubClone.Common.AzureServiceBus;
using GrubHubClone.Order.Consumers;
using GrubHubClone.Order.DataAccess;
using GrubHubClone.Order.DataAccess.Repositories;
using GrubHubClone.Order.Endpoints;
using GrubHubClone.Order.Interfaces;
using GrubHubClone.Order.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using static System.Net.WebRequestMethods;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DatabaseContext>();
builder.Services.AddLogging();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddAzureServiceBus(cfg =>
{
    cfg.ConnectionString = builder.Configuration.GetConnectionString("AzureServiceBus");
});
builder.Services.AddHostedService<OrderStatusChangedConsumer>();

builder.Services.AddAuth0JwtAuthentication(cfg => 
{
    //TODO: Change to read from configuration
    cfg.Audience = "https://api.luxnex.net";
    cfg.Authority = "https://dev-qepl2m1rxmm3zt3v.us.auth0.com/";
});

builder.Services.AddAuthorization(cfg => 
{
    cfg.AddPolicy("TestPolicy", x => x.RequireClaim("scope", "read:test"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("test", () => Results.Ok()).RequireAuthorization();

app.MapOrderEndpoints();

app.UseHttpsRedirection();

app.Run();
