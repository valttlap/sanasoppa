using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sanasoppa.API.Entities;

namespace Sanasoppa.API.Data.Configurations;
public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedOnAdd();

        builder.Property(e => e.HasStarted)
            .HasDefaultValue(false)
            .IsRequired();

        builder.HasMany(g => g.Players)
            .WithOne(p => p.Game)
            .HasForeignKey(p => p.GameId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(g => g.Rounds)
            .WithOne(r => r.Game)
            .HasForeignKey(r => r.GameId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(g => g.CurrentRound)
            .WithMany()
            .HasForeignKey(g => g.CurrentRoundId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(g => g.GameState)
            .HasConversion<int>()
            .IsRequired()
            .HasDefaultValue(GameState.NotStarted);
        
        builder.HasOne(g => g.Host)
            .WithMany()
            .HasForeignKey(g => g.HostId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(g => g.Name)
            .IsUnique();
    }
}
