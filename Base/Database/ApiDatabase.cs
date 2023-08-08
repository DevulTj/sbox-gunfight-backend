using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Base;

public class DatabaseContext : DbContext
{
    public DbSet<Models.Player> Players { get; set; }
    public DbSet<Models.PlayerLoadout> Loadouts { get; set; }

    public DbSet<Models.Match> MatchHistory { get; set; }
    public DbSet<Models.MatchPlayer> MatchPlayers { get; set; }


	string SqlConnectionString => @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=gunfight-local;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

    protected override void OnConfiguring( DbContextOptionsBuilder options )
    {
        Console.WriteLine("ApiDatabaseContext -> OnConfiguring");

		options.UseSqlServer( SqlConnectionString )
			.LogTo( Console.WriteLine, LogLevel.Information )
			.EnableSensitiveDataLogging()
			.EnableDetailedErrors();
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
