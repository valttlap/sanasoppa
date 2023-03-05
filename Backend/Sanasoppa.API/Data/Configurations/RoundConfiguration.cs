using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sanasoppa.API.Entities;

namespace Sanasoppa.API.Data.Configurations
{
    public class RoundConfiguration : IEntityTypeConfiguration<Round>
    {
        public void Configure(EntityTypeBuilder<Round> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.Word).IsRequired();

            builder.HasMany(e => e.Explanations)
                .WithOne(e => e.Round)
                .HasForeignKey(e => e.RoundId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.Scores)
                .WithOne(e => e.Round)
                .HasForeignKey(e => e.RoundId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
