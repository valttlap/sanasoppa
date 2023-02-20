﻿using Microsoft.EntityFrameworkCore;
using Rappakalja.API.Data.Configurations;
using Rappakalja.API.Entities;

namespace Rappakalja.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        { }
        public DbSet<Game> Games { get; set; } = default!;
        public DbSet<Player> Players { get; set; } = default!;
        public DbSet<Round> Rounds { get; set; } = default!;
        public DbSet<Explanation> Explanation { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new GameConfiguration());
            modelBuilder.ApplyConfiguration(new RoundConfiguration());
            modelBuilder.ApplyConfiguration(new ExplanationConfiguration());
            modelBuilder.ApplyConfiguration(new PlayerConfiguration());
        }
    }
}
