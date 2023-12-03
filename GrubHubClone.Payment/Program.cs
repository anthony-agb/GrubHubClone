

using GrubHubClone.Common.AzureServiceBus;
using GrubHubClone.Common.ServerSentEvents;
using GrubHubClone.Payment.Consumers;
using GrubHubClone.Payment.DataAccess;
using GrubHubClone.Payment.DataAccess.Repositories;
using GrubHubClone.Payment.Endpoints;
using GrubHubClone.Payment.Interfaces;
using GrubHubClone.Payment.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DatabaseContext>();

builder.Services.AddTransient<IPaymentRepository, PaymentRepository>();
builder.Services.AddTransient<IPaymentService, PaymentService>();

builder.Services.AddAzureServiceBus(cfg =>
{
    cfg.ConnectionString = builder.Configuration.GetConnectionString("AzureServiceBus");
});
builder.Services.AddHostedService<OrderCreatedConsumer>();

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

app.UseCors();
app.AddPaymentEndpoints();

app.UseHttpsRedirection();

app.Run();
