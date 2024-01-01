﻿using GrubHubClone.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace GrubHubClone.Payment.DataAccess;

public class DatabaseContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public DatabaseContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // in memory database used for simplicity, change to a real db for production applications
        options.UseInMemoryDatabase("TestDb");
    }

    public DbSet<PaymentModel> Payments { get; set; }
}
