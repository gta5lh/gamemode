using Newtonsoft.Json;

namespace Gamemode.ApiClient.Models
{
	public class GangWarStatistics
	{
		public GangWarStatistics(int fractionID)
		{
			this.FractionID = fractionID;
		}

		public GangWarStatistics(int fractionID, short KillsNumber)
		{
			this.FractionID = fractionID;
			this.KillsNumber = KillsNumber;
		}

		[JsonProperty("fraction_id")]
		public int FractionID { get; set; }

		[JsonProperty("kills_number")]
		public short KillsNumber { get; set; }
	}
}
