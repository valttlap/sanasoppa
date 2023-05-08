using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sanasoppa.Domain.Entities;

namespace Sanasoppa.API.Data.Configurations;
public class VoteConfiguration : IEntityTypeConfiguration<Vote>
{
    public void Configure(EntityTypeBuilder<Vote> builder)
    {
        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id).ValueGeneratedOnAdd();
        builder.HasOne(v => v.Round)
            .WithMany(r => r.Votes)
            .HasForeignKey(v => v.RoundId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(v => v.Explanation)
            .WithMany(e => e.Votes)
            .HasForeignKey(v => v.ExplanationId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(v => v.Player)
            .WithMany()
            .HasForeignKey(v => v.PlayerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
