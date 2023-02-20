using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rappakalja.API.Entities;

namespace Rappakalja.API.Data.Configurations
{
    public class ExplanationConfiguration : IEntityTypeConfiguration<Explanation>
    {
        public void Configure(EntityTypeBuilder<Explanation> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.Text).IsRequired();

            builder.HasOne(e => e.Player)
                .WithMany(e => e.Explanations)
                .HasForeignKey(e => e.PlayerId);
        }
    }
}
