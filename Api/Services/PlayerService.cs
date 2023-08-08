using Base;
using Base.Models;

namespace Api.Services;


public interface IPlayerService
{
    IEnumerable<Player> GetAll();
    Player GetById( string steamId );
    Player Create( string steamId );
    Player Update( string steamId, Player.UpdateRequest request );
    bool Delete( string steamId );
	Player GetOrCreate( string steamId );
	Player GiveExperience( string steamId, ulong xpAmount );
}

public partial class PlayerService : IPlayerService
{
	private DatabaseContext Db;

	public PlayerService( DatabaseContext db )
	{
		Db = db;
	}

	public Player Create( string steamId )
	{
		var player = new Player( steamId )
        {
            Experience = 0
        };

        Db.Players.Add( player );

		Db.SaveChanges();

		return player;
	}

	public bool Delete( string steamId )
	{
		var player = GetById( steamId );
		if ( player == null ) return false;

		Db.Players.Remove( player );
		Db.SaveChanges();

		return true;
	}

	public IEnumerable<Player> GetAll()
	{
		return Db.Players.AsEnumerable();
	}

	public Player GetById( string steamId )
	{
		return Db.Players.FirstOrDefault( x => x.SteamId.Equals( steamId ) );
	}

	public Player Update( string steamId, Player.UpdateRequest request )
	{
		var pl = GetById( steamId );

		// Request
		pl.Experience = request.Experience;
		pl.MarkAsUpdated();

		Db.Update( pl );

		Db.SaveChanges();

		return pl;
	}

	public Player GetOrCreate( string steamId )
	{
		var player = Db.Players.FirstOrDefault( x => x.SteamId.Equals( steamId ) );
		if ( player == null ) player = Create( steamId );

		return player;
	}

	public Player GiveExperience( string steamId, ulong xpAmount )
	{
		var pl = GetOrCreate( steamId );
		pl.Experience += xpAmount;
		pl.MarkAsUpdated();

		Db.Update( pl );
		Db.SaveChanges();

		return pl;
	}
}
