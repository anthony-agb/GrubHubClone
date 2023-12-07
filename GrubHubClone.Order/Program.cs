using GrubHubClone.Common.Authentication;
using GrubHubClone.Common.AzureServiceBus;
using GrubHubClone.Order.Consumers;
using GrubHubClone.Order.DataAccess;
using GrubHubClone.Order.DataAccess.Repositories;
using GrubHubClone.Order.Endpoints;
using GrubHubClone.Order.Interfaces;
using GrubHubClone.Order.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
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

IdentityModelEventSource.ShowPII = true;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = "https://dev-qepl2m1rxmm3zt3v.us.auth0.com/";
    options.Audience = "https://api.luxnex.net";
    //options.Authority = configuration.Authority;
    //options.Audience = configuration.Audience;
});

//builder.Services.AddAuth0JwtAuthentication(cfg => 
//{
//    //TODO: Change to read from configuration
//    cfg.Audience = "https://api.luxnex.net";
//    cfg.Authority = "https://dev-qepl2m1rxmm3zt3v.us.auth0.com/";
//});

builder.Services.AddAuthorization(cfg => 
{
    var scopeValues = new string[] { "read:test" };

    var requirement = new ClaimsAuthorizationRequirement("scope", allowedValues: scopeValues);
    var policy = new AuthorizationPolicy(new IAuthorizationRequirement[] { requirement }, new string[0]);
    cfg.AddPolicy("TestPolicy", policy);
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

app.MapPost("test", () => Results.Ok()).RequireAuthorization("TestPolicy");

app.MapOrderEndpoints();

app.UseHttpsRedirection();

app.Run();
