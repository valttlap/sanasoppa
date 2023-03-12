using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sanasoppa.API.Entities;

namespace Sanasoppa.API.Data.Configurations
{
    public class PlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.Username).IsRequired();

            builder.HasIndex(e => e.ConnectionId)
                .IsUnique();

            builder.Property(e => e.TotalPoints)
                .HasDefaultValue(0)
                .IsRequired();
        }
    }
}
