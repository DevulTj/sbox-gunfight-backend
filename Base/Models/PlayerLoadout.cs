using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Base.Models;

public class PlayerLoadout
{
    [ForeignKey( "SteamId" )]
    public Player Player { get; set; }

    [Key]
    public Guid LoadoutId { get; set; }

    [Key]
    public long SteamId { get; set; }
   
    public Weapon Primary { get; set; }
    public Weapon Secondary { get; set; }
}
