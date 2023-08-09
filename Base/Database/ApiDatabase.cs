using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Base;

public class DatabaseContext : DbContext
{
    public DbSet<Models.Player> Players { get; set; }
    public DbSet<Models.PlayerLoadout> Loadouts { get; set; }

    public DbSet<Models.Match> MatchHistory { get; set; }
    public DbSet<Models.MatchPlayer> MatchPlayers { get; set; }


	public static string ConnectionString;

    protected override void OnConfiguring( DbContextOptionsBuilder options )
    {
		options.UseNpgsql( ConnectionString );
	}

    protected override void OnModelCreating( ModelBuilder modelBuilder )
    {
		// Players
		modelBuilder.Entity<Models.Player>();

		// MatchHistory
		modelBuilder.Entity<Models.Match>();

		modelBuilder.Entity<Models.MatchPlayer>()
			.HasKey( x => new { x.MatchId, x.PlayerSteamId } );

		// Loadouts
		modelBuilder.Entity<Models.PlayerLoadout>()
			.HasKey( x => new { x.LoadoutId, x.SteamId } );

		// Loadouts -> Weapon
		modelBuilder.Entity<Models.Weapon>();
	}
}
