using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Base.Models;

public class Weapon : BaseModel
{
	[Key]
	public Guid Id { get; set; }
    public string Name { get; set; }

	[NotMapped]
	public ICollection<string> Attachments { get; set; }

	public string AttachmentsString
	{
		get { return string.Join( ";", Attachments ); }
		set { Attachments = value.Split( ';' ).ToList(); }
	}
}
