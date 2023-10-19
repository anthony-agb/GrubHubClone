using GrubHubClone.Restaurant.DataAccess;
using GrubHubClone.Restaurant.DataAccess.Repositories;
using GrubHubClone.Restaurant.Endpoints;
using GrubHubClone.Restaurant.Interfaces;
using GrubHubClone.Restaurant.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DbContext>();
builder.Services.AddTransient<IVenueRepository, VenueRepository>();
builder.Services.AddTransient<IVenueService, VenueService>();
builder.Services.AddTransient<IMenuService, MenuService>();
builder.Services.AddTransient<IMenuRepository, MenuRepository>();
builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapMenuEndpoints();
app.MapVenueEndpoints();

app.UseHttpsRedirection();

app.Run();