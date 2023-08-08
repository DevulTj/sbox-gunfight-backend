using Base;
using Base.Models;

namespace Api.Services;


public interface IPlayerService
{
    IEnumerable<Player> GetAll();
    Player GetById( long steamId );
    Player Create( long steamId );
    Player Update( long steamId, Player.UpdateRequest request );
    bool Delete( long steamId );
}

public partial class PlayerService : IPlayerService
{
	private DatabaseContext Db;

	public PlayerService( DatabaseContext db )
	{
		Db = db;
	}

	public Player Create( long steamId )
	{
		var player = new Player( steamId )
        {
            Experience = 0
        };

        Db.Players.Add( player );

		Db.SaveChanges();

		return player;
	}

	public bool Delete( long steamId )
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

	public Player GetById( long steamId )
	{
		return Db.Players.FirstOrDefault( x => x.SteamId == steamId );
	}

	public Player Update( long steamId, Player.UpdateRequest request )
	{
		var player = GetById( steamId );

		// Request
		player.Experience = request.Experience;

		Db.SaveChanges();

		return player;
	}
}
