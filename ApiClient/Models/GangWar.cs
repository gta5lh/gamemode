using Newtonsoft.Json;

namespace Gamemode.ApiClient.Models
{
	public class GangWar
	{
		[JsonProperty("id")]
		public int ID { get; set; }

		[JsonProperty("target_fraction_id")]
		public byte TargetFractionID { get; set; }

		[JsonProperty("target_fraction_name")]
		public string TargetFractionName { get; set; }

		[JsonProperty("winner_fraction_id")]
		public byte? WinnerFractionID { get; set; }

		[JsonProperty("winner_fraction_name")]
		public string? WinnerFractionName { get; set; }

		[JsonProperty("zone_id")]
		public int ZoneID { get; set; }

		[JsonProperty("finished")]
		public bool Finished { get; set; }

		[JsonProperty("x_coordinate")]
		public int XCoordinate { get; set; }

		[JsonProperty("y_coordinate")]
		public int YCoordinate { get; set; }
	}
}
