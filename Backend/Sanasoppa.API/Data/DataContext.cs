using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sanasoppa.API.Data.Configurations;
using Sanasoppa.API.Entities;

namespace Sanasoppa.API.Data
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int,
        IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        { }
        public DbSet<Game> Games { get; set; } = default!;
        public DbSet<Player> Players { get; set; } = default!;
        public DbSet<Round> Rounds { get; set; } = default!;
        public DbSet<Explanation> Explanation { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new GameConfiguration());
            builder.ApplyConfiguration(new RoundConfiguration());
            builder.ApplyConfiguration(new ExplanationConfiguration());
            builder.ApplyConfiguration(new PlayerConfiguration());
            builder.ApplyConfiguration(new AppRoleConfiguration());
            builder.ApplyConfiguration(new AppUserConfiguration());
        }
    }
}
