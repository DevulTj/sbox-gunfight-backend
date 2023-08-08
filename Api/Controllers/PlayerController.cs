using Base.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route( "api/[controller]" )]
public class PlayerController
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


    [Auth.RequireToken]
    [HttpPut]
    public Player UpdatePlayer( long steamId, Player.UpdateRequest request )
    {
        return Service.Update( steamId, request );
    }

    [Auth.RequireToken]
    [HttpPost]
    public Player Create( long steamId )
    {
        return Service.Create( steamId );
    }
}
