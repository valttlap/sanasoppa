using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sanasoppa.API.Entities;

namespace Sanasoppa.API.Data.Configurations
{
    public class GameStateConfiguration : IEntityTypeConfiguration<GameState>
    {
        public void Configure(EntityTypeBuilder<GameState> builder)
        {
            builder.HasKey(g => g.Id);
            builder.Property(g => g.Name).IsRequired();
            builder.HasIndex(g => g.Name).IsUnique();
        }
    }
}
