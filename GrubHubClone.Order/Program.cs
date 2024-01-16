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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

//Create new configuration obejct that pulls appsettings from 'appsettings' directory first
string basePath = Directory.GetCurrentDirectory();

string ConfigDirectory = Path.Combine(basePath, "Config", "appsettings.Production.json");

var Configuration = new ConfigurationBuilder()
    .SetBasePath(basePath)
    .AddJsonFile(ConfigDirectory, optional: true, reloadOnChange: true)
    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
    .Build();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(cfg =>
{
    cfg.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        In = ParameterLocation.Header,
        Name = "Authorization",
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme."
    });

    cfg.AddSecurityRequirement(new OpenApiSecurityRequirement 
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearer" }
            },
            new string[] {}
        }
    });
});

builder.Services.AddDbContext<DatabaseContext>(cfg =>
{
    string connectionString = Configuration.GetConnectionString("AzureSqlDatabase") ?? throw new NullReferenceException("Could not read Azure SQL database connection string from settings.");
    cfg.UseSqlServer(connectionString);
});

builder.Services.AddLogging();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddAzureServiceBus(cfg =>
{
    cfg.ConnectionString = Configuration.GetConnectionString("AzureServiceBus");
});
builder.Services.AddHostedService<OrderStatusChangedConsumer>();

IdentityModelEventSource.ShowPII = true;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = Configuration.GetValue<string>("Auth0:Authority") ?? throw new NullReferenceException("JWT authority could not be read from configuration.");
    options.Audience = Configuration.GetValue<string>("Auth0:Audience") ?? throw new NullReferenceException("JWT audience could not be read from configuration.");
});

builder.Services.AddAuthorization(cfg =>
{
    var scopeValues = new string[] { "read:test" };

    var requirement = new ClaimsAuthorizationRequirement("permissions", allowedValues: scopeValues);
    var policy = new AuthorizationPolicy(new IAuthorizationRequirement[] { requirement }, Array.Empty<string>());

    cfg.AddPolicy("AuthPolicy", policy);
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

//Test endpoint for auth
app.MapPost("test", () => Results.Ok())
    .RequireAuthorization("AuthPolicy");

app.MapOrderEndpoints();

app.UseHttpsRedirection();

app.Run();

public partial class Program { }