using Microsoft.EntityFrameworkCore;
using Sanasoppa.API.Data.Configurations;
using Sanasoppa.Domain.Entities;

namespace Sanasoppa.DataContext.Context;
public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    { }
    public DbSet<Game> Games { get; set; } = default!;
    public DbSet<Player> Players { get; set; } = default!;
    public DbSet<Round> Rounds { get; set; } = default!;
    public DbSet<Explanation> Explanations { get; set; } = default!;
    public DbSet<Vote> Votes { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new GameConfiguration());
        builder.ApplyConfiguration(new RoundConfiguration());
        builder.ApplyConfiguration(new ExplanationConfiguration());
        builder.ApplyConfiguration(new PlayerConfiguration());
        builder.ApplyConfiguration(new VoteConfiguration());
    }
}
