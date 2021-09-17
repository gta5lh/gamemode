using Newtonsoft.Json;

namespace GamemodeClient.Models
{
	public class ShowCarPark
	{
		[JsonProperty("playerRank")]
		public long PlayerRank;

		[JsonProperty("currentVehicleId")]
		public long CurrentVehicleId;

		public ShowCarPark(long playerRank, long currentVehicleId)
		{
			this.PlayerRank = playerRank;
			this.CurrentVehicleId = currentVehicleId;
		}
	}
}
