using GrubHubClone.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace GrubHubClone.Order.DataAccess;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions options) : base(options) { }

    public DbSet<OrderModel> Orders { get; set; }
}
