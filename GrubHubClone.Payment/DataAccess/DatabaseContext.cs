using GrubHubClone.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace GrubHubClone.Payment.DataAccess;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions options) : base(options) { }

    public DbSet<PaymentModel> Payments { get; set; }
}
