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
    public Player GetPlayer( string steamId )
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
    [HttpPost]
    public Player UpdatePlayer( Player.UpdateRequest request )
    {
        return Service.Update( SteamId, request );
    }

	[Auth.UseAuthenticatedSteamId]
	[Auth.RequireToken]
	[HttpPost( "xp" )]
	public Player GiveXP( Player.GiveExperienceRequest request )
	{
		return Service.GiveExperience( SteamId, request.Experience );
	}
}
