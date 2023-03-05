using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sanasoppa.API.Entities;

namespace Sanasoppa.API.Data.Configurations
{
    public class ExplanationConfiguration : IEntityTypeConfiguration<Explanation>
    {
        public void Configure(EntityTypeBuilder<Explanation> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.Text).IsRequired();

            builder.Property(e => e.IsRight).IsRequired();

            builder.HasOne(e => e.Player)
                .WithMany(p => p.Explanations)
                .HasForeignKey(e => e.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Round)
                .WithMany(r => r.Explanations)
                .HasForeignKey(e => e.RoundId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
