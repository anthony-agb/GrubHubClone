using GrubHubClone.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace GrubHubClone.Restaurant.DataAccess;

public class DbContext : Microsoft.EntityFrameworkCore.DbContext
{
    protected readonly IConfiguration Configuration;

    public DbContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // in memory database used for simplicity, change to a real db for production applications
        options.UseInMemoryDatabase("TestDb");
    }

    public DbSet<Venue> Venues { get; set; }
    public DbSet<Menu> Menus { get; set; }
    public DbSet<Product> Products { get; set; }
}
