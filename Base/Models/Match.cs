using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Base.Models;

public class Match
{
    [Key]
    public Guid Id { get; set; }

	[Required]
    public ulong ServerSteamId { get; set; }

	[Required]
	public string MapIdent { get; set; }

	public string GamemodeIdent { get; set; }

	public DateTimeOffset StartTime { get; set; }
	public DateTimeOffset EndTime { get; set; }

	public (DateTimeOffset, DateTimeOffset) GetTimeOffsets( TimeSpan span )
	{
		var now = DateTimeOffset.UtcNow;
		var start = now - span;
		return (start, now);
	}

	public class WithPlayers
	{
		public Match Match { get; set; }
		public IEnumerable<MatchPlayer> Players { get; set; }
	}
}

public class TimeSpanConverter : System.Text.Json.Serialization.JsonConverter<TimeSpan>
{
	public override TimeSpan Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
	{
		return TimeSpan.Parse( reader.GetString() );
	}

	public override void Write( Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options )
	{
		writer.WriteStringValue( value.ToString() );
	}
}


public struct MatchSubmitRequest
{
	public ulong ServerSteamId { get; set; }
	public string MapIdent { get; set; }
	public string GamemodeIdent { get; set; }

	[System.Text.Json.Serialization.JsonConverterAttribute( typeof( TimeSpanConverter ) )]
	public TimeSpan GameLength { get; set; }

	public List<MatchPlayerSubmitRequest> Players { get; set; }
}

public class MatchPlayer : BaseModel
{
	[Key]
	public Guid MatchId { get; set; }

	[ForeignKey( "MatchId" )] public Match Match { get; set; }

	[Key]
    public long PlayerSteamId { get; set; }

	[NotMapped]
	public Dictionary<string, string> KeyValues { get; set; }

	public string KeyValuesJson
	{
		get => JsonConvert.SerializeObject( KeyValues );
		set => KeyValues = JsonConvert.DeserializeObject<Dictionary<string, string>>( value );
	}
}


public struct MatchPlayerSubmitRequest
{
	public long PlayerSteamId { get; set; }
	public Dictionary<string, string> KeyValues { get; set; }
}
