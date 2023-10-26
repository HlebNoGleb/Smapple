using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smapple.Models;

namespace Smapple.DbContext;

public class SmappleDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public SmappleDbContext(DbContextOptions<SmappleDbContext> options) : base(options)
    {
        
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<GameUser> GameUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(user => user.Games)
            .WithMany(game => game.Users)
            .UsingEntity<GameUser>(
                l => l.HasOne<Game>(x => x.Game).WithMany(x => x.GameUsers),
                r => r.HasOne<User>(x => x.User).WithMany(x => x.GameUsers));

        modelBuilder.Entity<Game>().HasOne<User>(game => game.Host).WithMany(user => user.HostedGames);
        
        base.OnModelCreating(modelBuilder);
    }
}