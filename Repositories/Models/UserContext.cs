// <copyright file="UserContext.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.Repositories.Models
{
    using Microsoft.EntityFrameworkCore;
    using NLog.Extensions.Logging;

    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<AdminRank> AdminRanks { get; set; }
        public DbSet<Weapon> Weapons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseLoggerFactory(new NLogLoggerFactory()).EnableSensitiveDataLogging(true).UseNpgsql("Host=localhost;Database=gta;Username=postgres;Password=password", x => x.MigrationsHistoryTable("migrations_history"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdminRank>().HasData(
                new AdminRank { Id = 1, Name = "Junior" },
                new AdminRank { Id = 2, Name = "Middle" },
                new AdminRank { Id = 3, Name = "Senior" },
                new AdminRank { Id = 4, Name = "Lead" },
                new AdminRank { Id = 5, Name = "Owner" });

            modelBuilder.Entity<User>().HasIndex(u => u.Name).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<Weapon>().HasIndex(w => new { w.UserId, w.Hash }).IsUnique();
        }
    }
}
