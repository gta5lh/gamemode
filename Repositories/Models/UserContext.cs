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
        public DbSet<Fraction> Fractions { get; set; }
        public DbSet<FractionRank> FractionRanks { get; set; }
        public DbSet<Weapon> Weapons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseLoggerFactory(new NLogLoggerFactory()).EnableSensitiveDataLogging(true).UseNpgsql("Host=localhost;Database=gta;Username=postgres;Password=postgres", x => x.MigrationsHistoryTable("migrations_history"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdminRank>().HasData(
                new AdminRank { Id = 1, Name = "Junior" },
                new AdminRank { Id = 2, Name = "Middle" },
                new AdminRank { Id = 3, Name = "Senior" },
                new AdminRank { Id = 4, Name = "Lead" },
                new AdminRank { Id = 5, Name = "Owner" });

            modelBuilder.Entity<Fraction>().HasData(
                new Fraction { Id = 1, Name = "Bloods" },
                new Fraction { Id = 2, Name = "Ballas" },
                new Fraction { Id = 3, Name = "The Families" },
                new Fraction { Id = 4, Name = "Vagos" },
                new Fraction { Id = 5, Name = "Marabunta Grande" });

            modelBuilder.Entity<FractionRank>().HasData(
                new FractionRank { Id = 1, Tier = 1, Name = "Bloods1", FractionId = 1, RequiredExperienceToRankUp = 21 },
                new FractionRank { Id = 2, Tier = 2, Name = "Bloods2", FractionId = 1, RequiredExperienceToRankUp = 35 },
                new FractionRank { Id = 3, Tier = 3, Name = "Bloods3", FractionId = 1, RequiredExperienceToRankUp = 49 },
                new FractionRank { Id = 4, Tier = 4, Name = "Bloods4", FractionId = 1, RequiredExperienceToRankUp = 63 },
                new FractionRank { Id = 5, Tier = 5, Name = "Bloods5", FractionId = 1, RequiredExperienceToRankUp = 77 },
                new FractionRank { Id = 6, Tier = 6, Name = "Bloods6", FractionId = 1, RequiredExperienceToRankUp = 91 },
                new FractionRank { Id = 7, Tier = 7, Name = "Bloods7", FractionId = 1, RequiredExperienceToRankUp = 105 },
                new FractionRank { Id = 8, Tier = 8, Name = "Bloods8", FractionId = 1, RequiredExperienceToRankUp = 119 },
                new FractionRank { Id = 9, Tier = 9, Name = "Bloods9", FractionId = 1, RequiredExperienceToRankUp = 140 },
                new FractionRank { Id = 10, Tier = 10, Name = "Bloods10", FractionId = 1, RequiredExperienceToRankUp = 0 },

                new FractionRank { Id = 11, Tier = 1, Name = "Блайд", FractionId = 2, RequiredExperienceToRankUp = 21 },
                new FractionRank { Id = 12, Tier = 2, Name = "Бастер", FractionId = 2, RequiredExperienceToRankUp = 35 },
                new FractionRank { Id = 13, Tier = 3, Name = "Крэкер", FractionId = 2, RequiredExperienceToRankUp = 49 },
                new FractionRank { Id = 14, Tier = 4, Name = "Гун бро", FractionId = 2, RequiredExperienceToRankUp = 63 },
                new FractionRank { Id = 15, Tier = 5, Name = "Ап бро", FractionId = 2, RequiredExperienceToRankUp = 77 },
                new FractionRank { Id = 16, Tier = 6, Name = "Гангстер", FractionId = 2, RequiredExperienceToRankUp = 91 },
                new FractionRank { Id = 17, Tier = 7, Name = "Федерал блок", FractionId = 2, RequiredExperienceToRankUp = 105 },
                new FractionRank { Id = 18, Tier = 8, Name = "Фолкс", FractionId = 2, RequiredExperienceToRankUp = 119 },
                new FractionRank { Id = 19, Tier = 9, Name = "Райч нига", FractionId = 2, RequiredExperienceToRankUp = 140 },
                new FractionRank { Id = 20, Tier = 10, Name = "Биг вилли", FractionId = 2, RequiredExperienceToRankUp = 0 },

                new FractionRank { Id = 21, Tier = 1, Name = "Bloods11", FractionId = 3, RequiredExperienceToRankUp = 21 },
                new FractionRank { Id = 22, Tier = 2, Name = "Bloods12", FractionId = 3, RequiredExperienceToRankUp = 35 },
                new FractionRank { Id = 23, Tier = 3, Name = "Bloods13", FractionId = 3, RequiredExperienceToRankUp = 49 },
                new FractionRank { Id = 24, Tier = 4, Name = "Bloods14", FractionId = 3, RequiredExperienceToRankUp = 63 },
                new FractionRank { Id = 25, Tier = 5, Name = "Bloods15", FractionId = 3, RequiredExperienceToRankUp = 77 },
                new FractionRank { Id = 26, Tier = 6, Name = "Bloods16", FractionId = 3, RequiredExperienceToRankUp = 91 },
                new FractionRank { Id = 27, Tier = 7, Name = "Bloods17", FractionId = 3, RequiredExperienceToRankUp = 105 },
                new FractionRank { Id = 28, Tier = 8, Name = "Bloods18", FractionId = 3, RequiredExperienceToRankUp = 119 },
                new FractionRank { Id = 29, Tier = 9, Name = "Bloods19", FractionId = 3, RequiredExperienceToRankUp = 140 },
                new FractionRank { Id = 30, Tier = 10, Name = "Bloods20", FractionId = 3, RequiredExperienceToRankUp = 0 },

                new FractionRank { Id = 31, Tier = 1, Name = "Bloods21", FractionId = 4, RequiredExperienceToRankUp = 21 },
                new FractionRank { Id = 32, Tier = 2, Name = "Bloods22", FractionId = 4, RequiredExperienceToRankUp = 35 },
                new FractionRank { Id = 33, Tier = 3, Name = "Bloods23", FractionId = 4, RequiredExperienceToRankUp = 49 },
                new FractionRank { Id = 34, Tier = 4, Name = "Bloods24", FractionId = 4, RequiredExperienceToRankUp = 63 },
                new FractionRank { Id = 35, Tier = 5, Name = "Bloods25", FractionId = 4, RequiredExperienceToRankUp = 77 },
                new FractionRank { Id = 36, Tier = 6, Name = "Bloods26", FractionId = 4, RequiredExperienceToRankUp = 91 },
                new FractionRank { Id = 37, Tier = 7, Name = "Bloods27", FractionId = 4, RequiredExperienceToRankUp = 105 },
                new FractionRank { Id = 38, Tier = 8, Name = "Bloods28", FractionId = 4, RequiredExperienceToRankUp = 119 },
                new FractionRank { Id = 39, Tier = 9, Name = "Bloods29", FractionId = 4, RequiredExperienceToRankUp = 140 },
                new FractionRank { Id = 40, Tier = 10, Name = "Bloods30", FractionId = 4, RequiredExperienceToRankUp = 0 },

                new FractionRank { Id = 41, Tier = 1, Name = "Bloods31", FractionId = 5, RequiredExperienceToRankUp = 21 },
                new FractionRank { Id = 42, Tier = 2, Name = "Bloods32", FractionId = 5, RequiredExperienceToRankUp = 35 },
                new FractionRank { Id = 43, Tier = 3, Name = "Bloods33", FractionId = 5, RequiredExperienceToRankUp = 49 },
                new FractionRank { Id = 44, Tier = 4, Name = "Bloods34", FractionId = 5, RequiredExperienceToRankUp = 63 },
                new FractionRank { Id = 45, Tier = 5, Name = "Bloods35", FractionId = 5, RequiredExperienceToRankUp = 77 },
                new FractionRank { Id = 46, Tier = 6, Name = "Bloods36", FractionId = 5, RequiredExperienceToRankUp = 91 },
                new FractionRank { Id = 47, Tier = 7, Name = "Bloods37", FractionId = 5, RequiredExperienceToRankUp = 105 },
                new FractionRank { Id = 48, Tier = 8, Name = "Bloods38", FractionId = 5, RequiredExperienceToRankUp = 119 },
                new FractionRank { Id = 49, Tier = 9, Name = "Bloods39", FractionId = 5, RequiredExperienceToRankUp = 140 },
                new FractionRank { Id = 50, Tier = 10, Name = "Bloods40", FractionId = 5, RequiredExperienceToRankUp = 0 });

            modelBuilder.Entity<FractionRank>().HasIndex(u => new { u.Id, u.Tier }).IsUnique();
            modelBuilder.Entity<FractionRank>().HasIndex(u => new { u.Id, u.Name }).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Name).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<Weapon>().HasIndex(w => new { w.UserId, w.Hash }).IsUnique();
        }
    }
}
