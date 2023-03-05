using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sanasoppa.API.Entities;

namespace Sanasoppa.API.Data.Configurations
{
    public class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.HasStarted).HasDefaultValue(false).IsRequired();

            builder.HasMany(e => e.Players)
                .WithOne(e => e.Game)
                .HasForeignKey(e => e.GameId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(e => e.Rounds)
                .WithOne(e => e.Game)
                .HasForeignKey(e => e.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(g => g.CurrentRound)
                .WithOne()
                .HasForeignKey<Game>(g => g.CurrentRoundId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(g => g.CurrentGameState)
                .WithOne()
                .HasForeignKey<Game>(g => g.GameState)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(g => g.Name)
                .IsUnique();
        }
    }
}
