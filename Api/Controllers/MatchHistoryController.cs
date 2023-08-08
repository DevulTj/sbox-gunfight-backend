using Api.Services;
using Base;
using Base.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

[ApiController]
[Route( "api/[controller]" )]
public class MatchHistoryController
{
	private IMatchService Service;

	public MatchHistoryController( IMatchService service )
	{
		Service = service;

		Console.WriteLine( $"MatchHistoryController {service}" );
	}

	[Auth.RequireToken]
	[HttpGet]
	public IEnumerable<Match.WithPlayers> Get()
	{
		return Service.GetAll();
	}

	[Auth.RequireToken]
	[HttpGet( "{steamId}" )]
	public IEnumerable<Match.WithPlayers> GetForId( long steamId )
	{
		return Service.GetForPlayer( steamId );
	}

	[Auth.RequireToken]
    [HttpPost]
    public bool Submit( MatchSubmitRequest request )
    {
        return Service.Submit( request );
    }
}
