using Microsoft.EntityFrameworkCore;
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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Explanation>()
                .HasKey(e => e.Id); // Configures the primary key for the Explanation entity

            builder.Entity<Explanation>()
                .HasOne(e => e.Round)
                .WithMany()
                .HasForeignKey(e => e.RoundId); // Configures a many-to-one relationship between Explanation and Round entities and a foreign key

            builder.Entity<Explanation>()
                .HasOne(e => e.Player)
                .WithMany()
                .HasForeignKey(e => e.PlayerId); // Configures a many-to-one relationship between Explanation and Player entities and a foreign key

            builder.Entity<Game>()
                .HasKey(g => g.Id); // Configures the primary key for the Game entity

            builder.Entity<Game>()
                .Property(g => g.CreatedAt)
                .HasDefaultValueSql("NOW()"); // Configures the default value for the CreatedAt property

            builder.Entity<Game>()
                .HasIndex(g => g.ConnectionId)
                .IsUnique();

            builder.Entity<Game>()
                .HasMany(g => g.Players)
                .WithOne(p => p.Game)
                .HasForeignKey(p => p.GameId); // Configures a one-to-many relationship between Game and Player entities and a foreign key

            builder.Entity<Game>()
                .HasOne(g => g.CurrentRound)
                .WithMany()
                .HasForeignKey(g => g.CurrentRoundId); // Configures a many-to-one relationship between Game and Round entities and a foreign key

            builder.Entity<Player>()
                .HasKey(p => p.Id); // Configures the primary key for the Player entity

            builder.Entity<Round>()
                .HasKey(r => r.Id); // Configures the primary key for the Round entity

            builder.Entity<Round>()
                .HasOne(r => r.Game)
                .WithMany()
                .HasForeignKey(r => r.GameId); // Configures a many-to-one relationship between Round and Game entities and a foreign key

        }

    }
}
