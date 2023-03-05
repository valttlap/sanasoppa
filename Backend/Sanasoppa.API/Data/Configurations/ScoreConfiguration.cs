using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sanasoppa.API.Entities;

namespace Sanasoppa.API.Data.Configurations
{
    public class ScoreConfiguration : IEntityTypeConfiguration<Score>
    {
        public void Configure(EntityTypeBuilder<Score> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => new { e.PlayerId, e.RoundId }).IsUnique();
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.Points).IsRequired();
            builder.Property(e => e.PlayerId).IsRequired();
            builder.Property(e => e.RoundId).IsRequired();

            builder.HasOne(s => s.Player)
                .WithMany(p => p.Scores)
                .HasForeignKey(s => s.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}