using Base;
using Base.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services;

public interface IMatchService
{
	IEnumerable<Match.WithPlayers> GetForPlayer( long steamId );
	IEnumerable<Match.WithPlayers> GetAll( int take = 100, int skip = 0 );

	bool Submit( MatchSubmitRequest request );
}

public partial class MatchService : IMatchService
{
	private DatabaseContext Db;

	public MatchService( DatabaseContext db )
	{
		Db = db;
	}

	public IEnumerable<Match.WithPlayers> GetAll( int take = 100, int skip = 0 )
	{
		var matches = Db.MatchHistory
			.AsQueryable()
			.Skip( skip )
			.Take( take )
			.AsEnumerable();

		var matchesWithPlayers = new List<Match.WithPlayers>();

		foreach ( var match in matches )
		{
			var playersFromMatch = Db.MatchPlayers.Where( x => match.Id == x.MatchId ).AsEnumerable();

			var entry = new Match.WithPlayers()
			{
				Match = match,
				Players = playersFromMatch
			};

			matchesWithPlayers.Add( entry );
		}

		return matchesWithPlayers;
	}

	public IEnumerable<Match.WithPlayers> GetForPlayer( long steamId )
	{
		var matches = Db.MatchPlayers
			.AsQueryable()
			// Find match players that have our steamid
			.Where( x => x.PlayerSteamId == steamId )
			// Include match object
			.Include( x => x.Match )
			.Select( x => x.Match )
			.AsEnumerable();

		var matchesWithPlayers = new List<Match.WithPlayers>();

		foreach ( var match in matches )
		{
			var playersFromMatch = Db.MatchPlayers.Where( x => match.Id == x.MatchId ).AsEnumerable();

			var entry = new Match.WithPlayers()
			{
				Match = match,
				Players = playersFromMatch
			};

			matchesWithPlayers.Add( entry );
		}

		return matchesWithPlayers;
	}

	public bool Submit( MatchSubmitRequest request )
	{
		var match = new Match()
		{
			ServerSteamId = request.ServerSteamId,
			GamemodeIdent = request.GamemodeIdent,
			MapIdent = request.MapIdent
		};

		(DateTimeOffset Start, DateTimeOffset End) timeOffset = match.GetTimeOffsets( request.GameLength );
		match.StartTime = timeOffset.Start;
		match.EndTime = timeOffset.End;

		Db.MatchHistory.Add( match );

		Db.SaveChanges();

		foreach ( var player in request.Players )
		{
			Db.MatchPlayers.Add( new MatchPlayer()
			{
				Match = match,
				PlayerSteamId = player.PlayerSteamId,
				KeyValues = player.KeyValues
			} );
		}

		Db.SaveChanges();

		return true;
	}
}
