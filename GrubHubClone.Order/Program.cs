using GrubHubClone.Common.AzureServiceBus;
using GrubHubClone.Order.Consumers;
using GrubHubClone.Order.DataAccess;
using GrubHubClone.Order.DataAccess.Repositories;
using GrubHubClone.Order.Endpoints;
using GrubHubClone.Order.Interfaces;
using GrubHubClone.Order.Services;

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
builder.Services.AddHostedService<TestConsumer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapOrderEndpoints();

app.UseHttpsRedirection();

app.Run();
