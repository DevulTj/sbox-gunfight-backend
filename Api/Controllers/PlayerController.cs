using Base.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route( "api/[controller]" )]
public class PlayerController : BaseController
{
    private IPlayerService Service;

	public PlayerController(IPlayerService service )
    {
        Service = service;

        Console.WriteLine( $"PlayerController {service}" );
    }

    [HttpGet( "{steamId}" )]
    public Player GetPlayer( long steamId )
    {
        return Service.GetById( steamId );
    }

    [HttpGet]
    public IEnumerable<Player> GetPlayers()
    {
        return Service.GetAll();
    }


	[Auth.UseAuthenticatedSteamId]
    [Auth.RequireToken]
    [HttpPut]
    public Player UpdatePlayer( Player.UpdateRequest request )
    {
        return Service.Update( SteamId, request );
    }

	[Auth.UseAuthenticatedSteamId]
	[Auth.RequireToken]
    [HttpPost]
    public Player Create()
    {
        return Service.Create( SteamId );
    }

	[Auth.UseAuthenticatedSteamId]
	[Auth.RequireToken]
	[HttpPut( "xp" )]
	public Player GiveXP( ulong amount )
	{
		return Service.GiveExperience( SteamId, amount );
	}

}
