using Newtonsoft.Json;

namespace GamemodeClient.Models
{
	public class StartGangWar
	{
		[JsonProperty("remainingMs")]
		public double RemainingMs;

		[JsonProperty("targetFractionId")]
		public long TargetFractionId;

		public StartGangWar(double remainingMs, long targetFractionId)
		{
			this.RemainingMs = remainingMs;
			this.TargetFractionId = targetFractionId;
		}
	}
}
