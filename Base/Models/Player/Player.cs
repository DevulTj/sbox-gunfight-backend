using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Base.Models;

public class Player : BaseModel
{
	[DatabaseGenerated( DatabaseGeneratedOption.None )]
	[Key]
	public long SteamId { get; set; }

	public ulong Experience { get; set; }

	public Player( long steamId )
	{
		SteamId = steamId;
	}

	/// <summary>
	/// Update request to be used by <see cref="Api.Services.IPlayerService"/>
	/// </summary>
	public struct UpdateRequest
	{
		public ulong Experience { get; set; }
	}
}
