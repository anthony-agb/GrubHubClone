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
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

//Create new configuration obejct that pulls appsettings from 'appsettings' directory first
string basePath = Directory.GetCurrentDirectory();

string ConfigDirectory = Path.Combine(basePath, "Config", "appsettings.Production.json");

IConfigurationRoot configuration = new ConfigurationBuilder()
    .SetBasePath(basePath)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile(ConfigDirectory, optional: true, reloadOnChange: true)
    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
    .Build();

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
    cfg.ConnectionString = configuration.GetConnectionString("AzureServiceBus");
});
builder.Services.AddHostedService<OrderStatusChangedConsumer>();

IdentityModelEventSource.ShowPII = true;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = configuration.GetValue<string>("Auth0:Authority") ?? throw new NullReferenceException("JWT authority could not be read from configuration.");
    options.Audience = configuration.GetValue<string>("Auth0:Audience") ?? throw new NullReferenceException("JWT audience could not be read from configuration.");
});

builder.Services.AddAuthorization(cfg => 
{
    var scopeValues = new string[] { "read:test" };

    var requirement = new ClaimsAuthorizationRequirement("scope", allowedValues: scopeValues);
    var policy = new AuthorizationPolicy(new IAuthorizationRequirement[] { requirement }, Array.Empty<string>());

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

//Test endpoint for auth
app.MapPost("test", () => Results.Ok())
    .RequireAuthorization("TestPolicy");

app.MapOrderEndpoints();

app.UseHttpsRedirection();

app.Run();
