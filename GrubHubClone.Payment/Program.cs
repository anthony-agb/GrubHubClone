

using GrubHubClone.Common.AzureServiceBus;
using GrubHubClone.Common.ServerSentEvents;
using GrubHubClone.Payment.Consumers;
using GrubHubClone.Payment.DataAccess;
using GrubHubClone.Payment.DataAccess.Repositories;
using GrubHubClone.Payment.Endpoints;
using GrubHubClone.Payment.Interfaces;
using GrubHubClone.Payment.Services;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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

builder.Services.AddTransient<IPaymentRepository, PaymentRepository>();
builder.Services.AddTransient<IPaymentService, PaymentService>();

builder.Services.AddAzureServiceBus(cfg =>
{
    cfg.ConnectionString = builder.Configuration.GetConnectionString("AzureServiceBus");
});
builder.Services.AddHostedService<OrderCreatedConsumer>();

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

builder.Services.AddCors(policyBuilder =>
    policyBuilder.AddDefaultPolicy(policy =>
        policy.WithOrigins("*").AllowAnyHeader().AllowAnyHeader())
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseCors();
app.AddPaymentEndpoints();

app.UseHttpsRedirection();

app.Run();
